using UnityEngine;

public enum BuildingType
{
	Small1,
	Small2,
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
			case BuildingType.Small2:
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
				return (BuildingType)random.Next((int)BuildingType.Small1, (int)BuildingType.Small2 + 1);
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
	private Sprite current, damaged, rubble;
	private float health, damagerate;
	private float chaos;
	public void SetBuilding(Sprite normal, Sprite damaged, Sprite rubble, int size, float health, float chaos)
	{
		this.current = normal;
		this.damaged = damaged;
		this.rubble = rubble;
		SpriteRenderer render = this.GetComponent<SpriteRenderer>();
		render.sprite = normal;
		BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
		collider.size = new Vector2(size, size);
		collider.center = new Vector2(size * 0.5f, size * 0.5f);
		this.damagerate = 1.0f / health;
		this.chaos = chaos;
	}

	void Start() {
		this.health = 1.0f;
	}

	void Update() {
	}

	public void Damage()
	{
		Sprite sp;
		this.health -= this.damagerate * Time.deltaTime;
		if (this.health < 0.0f) {
			CityManager mgr = GameObject.Find("City").GetComponent<CityManager>();
			if (mgr != null)
				mgr.AddChaos(this.chaos);
			else
				Debug.LogError("Could not find city manager to add chaos");
			sp = this.rubble;
			BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
			Destroy(collider);
			Destroy(this);
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -0.1f);
		} else if (this.health < 0.8) {
			sp = this.damaged;
		} else {
			return;
		}
		SpriteRenderer render = this.GetComponent<SpriteRenderer>();
		if (sp == null)
			Destroy(render);
		else
			render.sprite = sp;
	}
}
