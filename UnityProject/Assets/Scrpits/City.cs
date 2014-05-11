using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public struct TileCoordinates {
	public TileCoordinates(int x, int y) { this.x = x; this.y = y; }

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

	public Intersection(int x, int y)
	{
		this.loc = new TileCoordinates(x, y);
	}

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
	private class StreetEnd
	{
		/// <summary>
		/// Create a pair of connected street ends.
		/// </summary>
		public static void Pair(int roadsize, int location, Direction dir, out StreetEnd first, out StreetEnd second)
		{
			first = new StreetEnd { roadsize = roadsize, location = location, direction = dir };
			second = new StreetEnd { roadsize = roadsize, location = location, direction = dir.Opposite() };
			first.other = second;
			second.other = first;
		}

		/// <summary>
		/// Connect the street to an intersection.
		/// </summary>
		public void Connect(Intersection i)
		{
			if (this.other != null) {
				this.other.other = null;
				this.other.intersection = i;
			}
			if (this.intersection != null) {
				this.intersection.SetNeighbor(this.direction, i);
				i.SetNeighbor(this.direction.Opposite(), this.intersection);
			}
		}

		public int roadsize;
		public int location;
		public Direction direction;
		public Intersection intersection;
		public StreetEnd other;
	}

	/// <summary>
	/// Minimum amount of space to place a road of the given size.
	/// </summary>
	private const int RoadSize1 = 30, RoadSize2 = 100;

	public readonly Terrain[,] terrain;
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
		if (space >= RoadSize2) {
			size = 1;
		} else if (space >= RoadSize1) {
			size = 0;
		} else {
			location = 0;
			size = 0;
			return false;
		}
		location = this.random.Next(space / 3, space - space / 3);
		return true;
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

	private List<StreetEnd> GenerateRoad(int x, int y, int width, int height, ArraySegment<BuildingType> specials)
	{
		ArraySegment<BuildingType> spec1, spec2;
		this.Split(specials, out spec1, out spec2);
		int roadloc, roadsize, absloc;
		bool isHorizontal;
		List<StreetEnd> ends1, ends2;
		Direction startDir, subDir;

		// Subdivide the rectangle into two smaller ones,
		// then generate the city in each rectangle, with a road between.

		if (width > height || (width == height && this.random.Next(0, 2) == 0)) {
			if (!this.PlaceRoad(width, out roadloc, out roadsize))
				return null;
			isHorizontal = true;
			absloc = roadloc + x;
			if (roadsize == 0) {
				this.RectFill(x + roadloc,            y, 1,        height, Terrain.RoadYellowVert);
			} else {
				this.RectFill(x + roadloc - roadsize, y, roadsize, height, Terrain.RoadWhiteVert);
				this.RectFill(x + roadloc,            y, 1,        height, Terrain.RoadYellow2Vert);
				this.RectFill(x + roadloc + 1,        y, roadsize, height, Terrain.RoadWhiteVert);
			}
			ends1 = this.Generate(x, y, roadloc - roadsize, height, spec1);
			ends2 = this.Generate(x + roadloc + roadsize + 1, y, width - roadloc - roadsize - 1, height, spec2);
			startDir = Direction.Down;
			subDir = Direction.Left;
		} else {
			if (!this.PlaceRoad(height, out roadloc, out roadsize))
				return null;
			isHorizontal = false;
			absloc = roadloc + y;
			if (roadsize == 0) {
				this.RectFill(x, y + roadloc,            width, 1,        Terrain.RoadYellowHoriz);
			} else {
				this.RectFill(x, y + roadloc - roadsize, width, roadsize, Terrain.RoadWhiteHoriz);
				this.RectFill(x, y + roadloc,            width, 1,        Terrain.RoadYellow2Horiz);
				this.RectFill(x, y + roadloc + 1,        width, roadsize, Terrain.RoadWhiteHoriz);
			}
			ends1 = this.Generate(x, y, width, roadloc - roadsize, spec1);
			ends2 = this.Generate(x, y + roadloc + roadsize + 1, width, height - roadloc - roadsize - 1, spec2);
			startDir = Direction.Left;
			subDir = Direction.Down;
		}

		// Update the road intersections.

		List<StreetEnd> outputEnds = new List<StreetEnd>(2);
		List<StreetEnd> intersectionEnds = new List<StreetEnd>();

		if (ends1 != null) {
			Direction dir = subDir.Opposite();
			foreach (StreetEnd end in ends1) {
				if (end.direction == dir)
					intersectionEnds.Add(end);
				else
					outputEnds.Add(end);
			}
		}
		
		if (ends2 != null) {
			Direction dir = subDir;
			foreach (StreetEnd end in ends2) {
				if (end.direction == dir)
					intersectionEnds.Add(end);
				else
					outputEnds.Add(end);
			}
		}
		
		StreetEnd first, last;
		StreetEnd.Pair(roadsize, absloc, startDir, out first, out last);
		Intersection intersection = null;
		int lastLoc = 0;
		Debug.Log("Starting");
		foreach (StreetEnd end in intersectionEnds.OrderBy(end => end.location)) {
			Debug.Log(string.Format("Intersection at {0} {1}", end.location, end.direction));
			if (intersection == null || lastLoc != end.location) {
				if (isHorizontal)
					intersection = new Intersection(absloc, end.location);
				else
					intersection = new Intersection(end.location, absloc);
				lastLoc = end.location;
				StreetEnd first2, last2;
				StreetEnd.Pair(roadsize, absloc, startDir, out first2, out last2);
				last.Connect(intersection);
				first2.Connect(intersection);
				last = last2;
			}
			end.Connect(intersection);
			if (end.roadsize >= roadsize) {
				int start = end.location - end.roadsize;
				int size = end.roadsize * 2 + 1;
				if (isHorizontal)
					this.RectFill(x + roadloc - roadsize, start, roadsize * 2 + 1, size, Terrain.Intersection);
				else
					this.RectFill(start, y + roadloc - roadsize, size, roadsize * 2 + 1, Terrain.Intersection);
			}
		}

		outputEnds.Add(first);
		outputEnds.Add(last);
		return outputEnds;
	}
	
	/// <summary>
	/// Generate city data in the given region.
	/// </summary>
	/// <param name="x">The lower-left x coordinate.</param>
	/// <param name="y">The lower-left y coordinate.</param>
	/// <param name="width">The number of tiles wide.</param>
	/// <param name="height">The number of tiles high.</param>
	/// <param name="specials">Special buildings to place in this region.</param>
	private List<StreetEnd> Generate(int x, int y, int width, int height, ArraySegment<BuildingType> specials)
	{
		Debug.Log("HERE");
		// Debug.Log(string.Format("CityGrid.Generate {0} {1} {2} {3}", x, y, width, height));
		List<StreetEnd> result = this.GenerateRoad(x, y, width, height, specials);
		if (result != null)
			return result;

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

		return null;
	}

	private void RectFill(int x, int y, int width, int height, Terrain tile)
	{
		for (int xpos = x; xpos < x + width; xpos++) {
			for (int ypos = y; ypos < y + height; ypos++) {
				this.terrain[xpos, ypos] = tile;
			}
		}
	}

	public void SpawnGrid(GameObject gridPrefab)
	{
		const int maxMesh = 64;
		int xcount = this.width / maxMesh, ycount = this.height / maxMesh;
		for (int x = 0; x < xcount; x++) {
			int x0 = this.width * x / xcount, x1 = this.width * (x + 1) / xcount;
			for (int y = 0; y < ycount; y++) {
				int y0 = this.height * y / ycount, y1 = this.width * (y + 1) / ycount;
				GameObject obj = (GameObject)GameObject.Instantiate(gridPrefab, new Vector3(x0, y0, 0.0f), Quaternion.identity);
				Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
				this.LoadMesh(mesh, x0, y0, x1 - x0, y1 - y0);
			}
		}
	}

	/// <summary>
	/// Load a section of the city terrain into a mesh.
	/// </summary>
	private void LoadMesh(Mesh mesh, int x, int y, int width, int height)
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

	public GameObject GridObject;

	private Terrain[,] terrain;
	
	// Use this for initialization
	void Start () {
		if (this.Height < 10 || this.Width < 10) {
			Debug.LogError("City dimensions are too small!");
			return;
		}

		int seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		CityGrid grid = new CityGrid(this.Width, this.Height, seed);
		grid.Generate();
		grid.SpawnGrid(this.GridObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
