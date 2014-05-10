using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public struct TileCoordinates {
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
		get { return new Vector2(this.loc.x, this.loc.y); }
	}

	public bool DoesContain(Vector2 point)
	{
		return false;
	}
}

public enum Terrain {
	Lot,
	Sidewalk11, Sidewalk12, Sidewalk13, Sidewalk21, Sidewalk22, Sidewalk23, Sidewalk31, Sidewalk32, Sidewalk33,
	Intersection,
	RoadYellowHoriz, RoadYellowVert, RoadWhiteHoriz, RoadWhiteVert
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
internal class CityGenerator 
{
	/// <summary>
	/// Minimum amount of space to place a road of the given size.
	/// </summary>
	private const int RoadSize1 = 30, RoadSize2 = 100;

	private readonly Terrain[,] terrain;
	private readonly int width, height;
	private readonly List<CityBuilding> buildings;
	private readonly System.Random random;

	public CityGenerator(int width, int height, int seed)
	{
		this.terrain = new Terrain[width, height];
		this.width = width;
		this.height = height;
		this.random = new System.Random(seed);
	}

	public void Generate()
	{
		this.Generate(0, 0, this.width, this.height, new ArraySegment<BuildingType>(null, 0, 0));
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
		int roadloc, roadsize;
		ArraySegment<BuildingType> spec1, spec2;
		if (width > height) {
			if (this.PlaceRoad(width, out roadloc, out roadsize)) {
				this.Split(specials, out spec1, out spec2);
				this.RectFill(x + roadloc, y, roadsize, height, Terrain.RoadYellowVert);
				this.Generate(x, y, roadloc, height, spec1);
				this.Generate(x + roadloc + roadsize, y, width - roadloc - roadsize, height, spec2);
				return;
			}
		} else {
			if (this.PlaceRoad(height, out roadloc, out roadsize)) {
				this.Split(specials, out spec1, out spec2);
				this.RectFill(x, y + roadloc, width, roadsize, Terrain.RoadYellowHoriz);
				this.Generate(x, y, width, roadloc, spec1);
				this.Generate(x, y + roadloc + roadsize, width, height - roadloc - roadsize, spec2);
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
			for (int ypos = x; ypos < y + height; ypos++) {
				this.terrain[xpos, ypos] = tile;
			}
		}
	}
}

public class City : MonoBehaviour {
	const int Width = 500, Height = 500;



	private Terrain[,] terrain;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
