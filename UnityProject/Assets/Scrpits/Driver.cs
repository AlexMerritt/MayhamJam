using UnityEngine;
using System.Collections;
using System;

public class Driver : FourDirectionMob {
	float stateTimer;
	float stateTimeMin = 0.3f;
	float stateTimeMax = 1.0f;
	
	float aggroRange = 4.0f;
	float attackRange = 2.0f;
	
	public Intersection nextIntersection;
	public Intersection lastIntersection;
	
	public float attackTime;
	public float attackDamage;
	float attackCooldown = 0;
	//TODO: attack sounds and animation?
	
	GameObject monster;
	public City city;
	
	bool dead = false;
	
	// Use this for initialization
	new public void Start () {
		//store the monster for later use
		monster = GameObject.FindWithTag("monster");
		
		//store the city for later use
		city = GameObject.FindObjectOfType<City>();
	}
	
	// Update is called once per frame
	new public void Update () {
		if (!dead) {
			float distToMonster = Vector2.Distance(transform.position, monster.transform.position);
			if (false) {//distToMonster <= attackRange) {
				//ATTACK
			} else if (nextIntersection.Contains(transform.position)) {
				PickIntersection();
			} else {
				Debug.Log("MOVING to "+nextIntersection.Coordinates+" from position "+transform.position+" with speed "+runSpeed+" in direction "+curDirection);
				//transform.Translate(Vector3.right * 10);
				Move(curDirection, runSpeed);
			}
			
			base.Update();
		}
	}
	
	//just rotate sprites and don't bother with animations
	new public bool SetFacing (Direction newDirection) {
		if (curDirection != newDirection) {
			curDirection = newDirection;
			
			int angle = ((int)curDirection * 90 + 270) % 360;
			gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
			
			return true;
		}
		
		return false;
	}
	
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
		float distToMonster = Vector2.Distance(transform.position, monster.transform.position);
		if (distToMonster <= aggroRange) {
			float bestDist = 1000;
			Intersection bestInter = lastIntersection;
			for (int i=0; i<4; i++) {
				Intersection inter = nextIntersection.GetNeighbor((global::Direction)i);
				if (inter != null) {
					float thisDist = Vector2.Distance(nextIntersection.Coordinates, monster.transform.position);
					if (thisDist < bestDist) {
						bestDist = thisDist;
						bestInter = inter;
					}
				}
			}
		
			lastIntersection = nextIntersection;
			nextIntersection = bestInter;
		} else {
			lastIntersection = nextIntersection;
			
			while (nextIntersection.GetNeighbor((global::Direction)curDirection) == null) {
				RandomizeFacing();
			}
			
			nextIntersection = nextIntersection.GetNeighbor((global::Direction)curDirection);
		}
		
		FaceIntersection(nextIntersection);
		
		Debug.Log("PickIntersection(): Next intersection is "+nextIntersection.Coordinates+" and last intersection was "+lastIntersection.Coordinates);
	}
}
