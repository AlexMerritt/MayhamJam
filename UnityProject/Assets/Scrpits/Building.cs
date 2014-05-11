using UnityEngine;

public enum BuildingType
{
	Small1,
	Medium1,
	Big1,
	Big2,

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

public static class BuildingTypeUtil
{
	public static int FootprintSize(this BuildingType type)
	{
		switch (type) {
			case BuildingType.Small1:
				return 2;
			case BuildingType.Medium1:
				return 3;
			default:
				return 5;
		}
	}

	public static BuildingType Random(System.Random random, int size)
	{
		switch (size) {
			case 1:
				return (BuildingType)random.Next((int)BuildingType.Small1, (int)BuildingType.Small1 + 1);
			case 2:
				return (BuildingType)random.Next((int)BuildingType.Medium1, (int)BuildingType.Medium1 + 1);
			case 3:
				return (BuildingType)random.Next((int)BuildingType.Big1, (int)BuildingType.Big2 + 1);
			default:
				Debug.LogError("Invalid building size");
				return (BuildingType)0;
		}
	}
}

public class Building : MonoBehaviour {
	void Start() {
	}

	void Update() {
	}
}
