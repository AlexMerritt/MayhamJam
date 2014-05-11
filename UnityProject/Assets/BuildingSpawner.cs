using UnityEngine;
using System.Linq;

public class BuildingSpawner : MonoBehaviour {
	public GameObject Building;
	public Sprite[] BuildingSprites;
	private bool flag;
	public float HealthSmall, HealthMedium, HealthBig;
	
	void Start() {
	}
	
	void Update() {
	}

	private Sprite FindSprite(string name)
	{
		return this.BuildingSprites.FirstOrDefault(sp => sp.name == name);
	}

	public void SpawnBuilding(int x, int y, BuildingType type)
	{
		float health = 1.0f;
		switch (type.FootprintSize()) {
			case 2: health = this.HealthSmall; break;
			case 3: health = this.HealthMedium; break;
			case 5: health = this.HealthBig; break;
		}
		string name = type.ToString().ToLowerInvariant();
		Sprite normal = this.FindSprite(name);
		if (normal == null) {
			Debug.LogError(string.Format("Cannot find building sprite: {0}", name));
			return;
		}
		Sprite damage = this.FindSprite(string.Concat(name, "b")) ?? normal;
		Sprite rubble = this.FindSprite(string.Concat("rubble", type.FootprintSize()));
		GameObject obj = (GameObject)GameObject.Instantiate(this.Building, new Vector3(x, y, (float)y * 0.01f - 6.0f), Quaternion.identity);
		Building bldg = obj.GetComponent<Building>();
		bldg.SetBuilding(normal, damage, rubble, type.FootprintSize(), health);
	}
}
