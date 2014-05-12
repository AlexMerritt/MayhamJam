using UnityEngine;
using System.Collections;
using System;

public class Driver : FourDirectionMob {
	private const float aggroRange = 4.0f;
	private Intersection nextIntersection;
	private Direction curDirection;

	// Use this for initialization
	new public void Start () {
	}

	private Direction ChooseDirection(Intersection src)
	{
		GameObject monster = GameObject.FindWithTag("monster");
		float distToMonster = Vector2.Distance(src.Coordinates, monster.transform.position);
		if (distToMonster <= aggroRange) {
			float bestDist = 1e10f;
			Direction bestDir = default(Direction);
			for (int i = 0; i < 4; i++) {
				Intersection inter = src.GetNeighbor((Direction)i);
				if (inter == null)
					continue;
				float dist = Vector2.Distance(inter.Coordinates, monster.transform.position);
				if (dist < bestDist) {
					bestDist = dist;
					bestDir = (Direction)i;
				}
			}
			return bestDir;
		} else {
			Intersection inter;
			Direction d;
			do {
				d = (Direction)UnityEngine.Random.Range(0, 4);
				inter = src.GetNeighbor((Direction)d);
			} while (inter == null);
			return d;
		}
	}

	public void SetIntersection(Intersection inter)
	{
		Debug.Log(string.Format("Set intersection. Null? {0}", inter == null));
		Vector2 loc2 = inter.Coordinates;
		Vector3 loc = new Vector3(loc2.x, loc2.y, -1.0f);
		this.transform.position = loc;
		this.curDirection = this.ChooseDirection(inter);
		this.nextIntersection = inter.GetNeighbor(this.curDirection);
	}

	// Update is called once per frame
	new public void Update () {
		if (this.nextIntersection == null) {
			Debug.Log("No intersection driver");
			return;
		}
		// float distToMonster = Vector2.Distance(transform.position, monster.transform.position);
		if (false) {//distToMonster <= attackRange) {
			//ATTACK
		} else if (nextIntersection.Contains(transform.position)) {
			Debug.Log("NEXT INTERSECTION");
			this.SetIntersection(this.nextIntersection);
		} else {
			Debug.Log("MOVING to "+nextIntersection.Coordinates+" from position "+transform.position+" with speed "+runSpeed+" in direction "+curDirection);
			//transform.Translate(Vector3.right * 10);
			Move(curDirection, runSpeed);
		}
		
		base.Update();
	}
	
	//just rotate sprites and don't bother with animations
	new public bool SetFacing (Direction newDirection) {
		if (curDirection != newDirection) {
			curDirection = newDirection;
			
			int angle = ((int)curDirection * 90 + 270) % 360;
			// gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
			
			return true;
		}
		
		return false;
	}
	/*
	public void FaceIntersection(Intersection inter) {
		Vector2 deltaPos = (Vector2)transform.position - inter.Coordinates;
		var angle = Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;
		angle = (angle + 315) % 360;
		FourDirectionMob.Direction faceDirection = (FourDirectionMob.Direction)Mathf.Floor(angle / 90);
		SetFacing(faceDirection);
	}

	
	//goodnight, sweet prince
	public void Die() {
		dead = true;
		anim.SetBool("Dead", true);
	}
	
	public void PickIntersection() {

	}
	*/
}
