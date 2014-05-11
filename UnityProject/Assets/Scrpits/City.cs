using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public struct TileCoordinates {
	TileCoordinates(int x, int y) { this.x = x; this.y = y; }

	/// <summary>
	/// Y position in tile coordinates.
	/// </summary>
	public readonly int x;

	/// <summary>
	/// Y position in tile coordinates.
	/// </summary>
	public readonly int y;
}

public class Intersection
{
	private Intersection left, right, up, down;
	private TileCoordinates loc;

	public Intersection GetNeighbor(Direction dir)
	{
		switch (dir) {
		case Direction.Left: return this.left;
		case Direction.Right: return this.right;
		case Direction.Up: return this.up;
		case Direction.Down: return this.down;
		default: return null;
		}
	}

	public void SetNeighbor(Direction dir, Intersection neighbor)
	{
		switch (dir) {
		case Direction.Left: this.left = neighbor; break;
		case Direction.Right: this.right = neighbor; break;
		case Direction.Up: this.up = neighbor; break;
		case Direction.Down: this.down = neighbor; break;
		}
	}

	public Vector2 Coordinates
	{
		get { return new Vector2(this.loc.x + 0.5f, this.loc.y + 0.5f); }
	}

	public bool DoesContain(Vector2 point)
	{
		return false;
	}
}

public enum Terrain {
	// Note: these are ordered to match the sprite sheet
	Sidewalk11, Sidewalk21, Sidewalk31, RoadWhiteVert,
	Sidewalk12, Sidewalk22, Sidewalk32, RoadWhiteHoriz,
	Sidewalk13, Sidewalk23, Sidewalk33, RoadYellowVert,
	Intersection, RoadYellow2Vert, RoadYellow2Horiz, RoadYellowHoriz
}

public enum BuildingType
{
	GenericSmall, GenericMedium, GenericLarge,
	PoliceStation, CityHall,
	LandmarkEiffel, LandmarkBigBen, LandmarkSydney,
	ResearchLab,
	ElementarySchool,
	PowerPlant,
	FireStation,
	Hospital,
	SewageTreatment,
	OldSteelMill
}

/// <summary>
/// A building in the generated city grid.
/// </summary>
public class CityBuilding
{
	public BuildingType type;
	public int x;
	public int y;
	public int width;
	public int height;
}

/// <summary>
/// Helper class for generating the city grid.
/// </summary>
internal class CityGrid 
{
	/// <summary>
	/// Minimum amount of space to place a road of the given size.
	/// </summary>
	private const int RoadSize1 = 30, RoadSize2 = 30;

	public readonly Terrain[,] terrain;
	public readonly List<Intersection> intersections = new List<Intersection>();
	public readonly int width, height;
	private readonly System.Random random;

	public CityGrid(int width, int height, int seed)
	{
		this.terrain = new Terrain[width, height];
		this.width = width;
		this.height = height;
		this.random = new System.Random(seed);
	}

	public void Generate()
	{
		BuildingType[] arr = new BuildingType[0];
		this.Generate(0, 0, this.width, this.height, new ArraySegment<BuildingType>(arr, 0, 0));
	}

	/// <summary>
	/// In a given linear space, figure out where to place a road.
	/// </summary>
	/// <returns><c>true</c>, if road was placed, <c>false</c> otherwise.</returns>
	/// <param name="space">The amount of space for the road.</param>
	/// <param name="location">The location for the road.</param>
	/// <param name="size">The width of the road.</param>
	private bool PlaceRoad(int space, out int location, out int size)
	{
		int maxsz;
		if (space >= RoadSize2)
			maxsz = 2;
		else if (space >= RoadSize1)
			maxsz = 1;
		else
			maxsz = 0;
		if (maxsz == 0) {
			location = 0;
			size = 0;
			return false;
		} else {
			location = this.random.Next(space / 3, space - space / 3 - (maxsz * 2 - 1));
			size = maxsz;
			return true;
		}
	}

	/// <summary>
	/// Split special buildings between two regions of roughly equal size.
	/// </summary>
	/// <param name="x">The input special buildings.</param>
	/// <param name="y">Output buildings for one region.</param>
	/// <param name="z">Output buildings for the other region.</param>
	private void Split(ArraySegment<BuildingType> x, out ArraySegment<BuildingType> y, out ArraySegment<BuildingType> z)
	{
		int amt1 = x.Count / 2, amt2 = x.Count - amt1;
		if (this.random.Next(0, 2) == 0) {
			int t = amt1;
			amt1 = amt2;
			amt2 = t;
		}
		y = new ArraySegment<BuildingType>(x.Array, x.Offset, amt1);
		z = new ArraySegment<BuildingType>(x.Array, x.Offset + amt1, amt2);
	}

	/// <summary>
	/// Generate city data in the given region.
	/// </summary>
	/// <param name="x">The lower-left x coordinate.</param>
	/// <param name="y">The lower-left y coordinate.</param>
	/// <param name="width">The number of tiles wide.</param>
	/// <param name="height">The number of tiles high.</param>
	/// <param name="specials">Special buildings to place in this region.</param>
	private void Generate(int x, int y, int width, int height, ArraySegment<BuildingType> specials)
	{
		// Debug.Log(string.Format("CityGrid.Generate {0} {1} {2} {3}", x, y, width, height));
		int roadloc, roadsize;
		ArraySegment<BuildingType> spec1, spec2;
		if (width > height) {
			if (this.PlaceRoad(width, out roadloc, out roadsize)) {
				int roadtiles = roadsize * 2 - 1;
				this.Split(specials, out spec1, out spec2);
				if (roadsize == 1) {
					this.RectFill(x + roadloc,                y, 1,            height, Terrain.RoadYellowVert);
				} else {
					this.RectFill(x + roadloc,                y, roadsize - 1, height, Terrain.RoadWhiteVert);
					this.RectFill(x + roadloc + roadsize - 1, y, 1,            height, Terrain.RoadYellow2Vert);
					this.RectFill(x + roadloc + roadsize,     y, roadsize - 1, height, Terrain.RoadWhiteVert);
				}
				this.Generate(x, y, roadloc, height, spec1);
				this.Generate(x + roadloc + roadtiles, y, width - roadloc - roadtiles, height, spec2);
				return;
			}
		} else {
			if (this.PlaceRoad(height, out roadloc, out roadsize)) {
				int roadtiles = roadsize * 2 - 1;
				this.Split(specials, out spec1, out spec2);
				if (roadsize == 1) {
					this.RectFill(x, y + roadloc,                width, roadsize,     Terrain.RoadYellowHoriz);
				} else {
					this.RectFill(x, y + roadloc,                width, roadsize - 1, Terrain.RoadWhiteHoriz);
					this.RectFill(x, y + roadloc + roadsize - 1, width, roadsize,     Terrain.RoadYellow2Horiz);
					this.RectFill(x, y + roadloc + roadsize,     width, roadsize - 1, Terrain.RoadWhiteHoriz);
				}
				this.Generate(x, y, width, roadloc, spec1);
				this.Generate(x, y + roadloc + roadtiles, width, height - roadloc - roadtiles, spec2);
				return;
			}
		}

		// Fill in sidewalk for this region.

		this.terrain[x, y] = Terrain.Sidewalk11;
		this.RectFill(x, y + 1, 1, height - 2, Terrain.Sidewalk12);
		this.terrain[x, y + height - 1] = Terrain.Sidewalk13;

		this.RectFill(x + 1, y, width - 2, 1, Terrain.Sidewalk21);
		this.RectFill(x + 1, y + 1, width - 2, height - 2, Terrain.Sidewalk22);
		this.RectFill(x + 1, y + height - 1, width - 1, 1, Terrain.Sidewalk23);

		this.terrain[x + width - 1, y] = Terrain.Sidewalk31;
		this.RectFill(x + width - 1, y + 1, 1, height - 2, Terrain.Sidewalk32);
		this.terrain[x + width - 1, y + height - 1] = Terrain.Sidewalk33;
	}

	private void RectFill(int x, int y, int width, int height, Terrain tile)
	{
		for (int xpos = x; xpos < x + width; xpos++) {
			for (int ypos = y; ypos < y + height; ypos++) {
				this.terrain[xpos, ypos] = tile;
			}
		}
	}

	/// <summary>
	/// Load a section of the city terrain into a mesh.
	/// </summary>
	public void LoadMesh(Mesh mesh, int x, int y, int width, int height)
	{
		Vector3[] vertex = new Vector3[width * height * 4];
		Vector2[] uv = new Vector2[width * height * 4];
		int[] index = new int[width * height * 6];
		for (int xpos = 0; xpos < width; xpos++) {
			for (int ypos = 0; ypos < height; ypos++) {
				int n = (xpos * height) + ypos;

				vertex[n * 4 + 0] = new Vector3(xpos + 0, ypos + 0, 0);
				vertex[n * 4 + 1] = new Vector3(xpos + 1, ypos + 0, 0);
				vertex[n * 4 + 2] = new Vector3(xpos + 0, ypos + 1, 0);
				vertex[n * 4 + 3] = new Vector3(xpos + 1, ypos + 1, 0);

				Terrain tile = this.terrain[x + xpos, y + ypos];
				int tx = (int)tile & 3, ty = ((int)tile >> 2) & 3;
				uv[n * 4 + 0] = new Vector2(0.25f * (tx + 0), 0.25f * (ty + 0));
				uv[n * 4 + 1] = new Vector2(0.25f * (tx + 1), 0.25f * (ty + 0));
				uv[n * 4 + 2] = new Vector2(0.25f * (tx + 0), 0.25f * (ty + 1));
				uv[n * 4 + 3] = new Vector2(0.25f * (tx + 1), 0.25f * (ty + 1));

				index[n * 6 + 0] = n * 4 + 0;
				index[n * 6 + 1] = n * 4 + 2;
				index[n * 6 + 2] = n * 4 + 1;
				index[n * 6 + 3] = n * 4 + 1;
				index[n * 6 + 4] = n * 4 + 2;
				index[n * 6 + 5] = n * 4 + 3;
			}
		}
		mesh.Clear();
		mesh.vertices = vertex;
		mesh.uv = uv;
		mesh.triangles = index;
	}
}

public class City : MonoBehaviour {
	public int Width;
	public int Height;

	private Terrain[,] terrain;
	private List<Intersection> intersections;
	
	// Use this for initialization
	void Start () {
		if (this.Height < 10 || this.Width < 10) {
			Debug.LogError("City dimensions are too small!");
			return;
		}

		int seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		CityGrid grid = new CityGrid(this.Width, this.Height, seed);
		grid.Generate();
		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		grid.LoadMesh(mesh, 0, 0, 64, 64);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public Intersection RandomIntersection()
	{
		int idx = UnityEngine.Random.Range(0, this.intersections.Count);
		return this.intersections[idx];
	}
}
