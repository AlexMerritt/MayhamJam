using UnityEngine;

public enum BuildingType
{
	Small1,
	Medium1,
	Big1,

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

public static class BuildingTypeExtensions
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
}

public class Building : MonoBehaviour {
	void Start() {
	}

	void Update() {
	}
}
