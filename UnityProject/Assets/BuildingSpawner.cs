using UnityEngine;
using System.Linq;

public class BuildingSpawner : MonoBehaviour {
	public GameObject Building;
	public Sprite[] BuildingSprites;
	private bool flag;
	
	void Start() {
	}
	
	void Update() {
	}
	
	public void SpawnBuilding(int x, int y, BuildingType type)
	{
		string name = type.ToString().ToLowerInvariant();
		Sprite sprite = this.BuildingSprites.FirstOrDefault(sp => sp.name == name);
		if (sprite == null) {
			Debug.LogError(string.Format("Cannot find building sprite: {0}", name));
			return;
		}
		GameObject obj = (GameObject)GameObject.Instantiate(this.Building, new Vector3(x, y, (float)y * 0.01f - 6.0f), Quaternion.identity);
		SpriteRenderer render = obj.GetComponent<SpriteRenderer>();
		render.sprite = sprite;
	}
}
