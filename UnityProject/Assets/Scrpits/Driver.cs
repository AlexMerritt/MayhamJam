﻿using UnityEngine;
using System.Collections;
using System;

public class Driver : FourDirectionMob {
	float stateTimer;
	float stateTimeMin = 0.3f;
	float stateTimeMax = 1.0f;
	
	float aggroRange = 4.0f;
	float attackRange = 2.0f;
	
	Intersection nextIntersection;
	Intersection lastIntersection;
	
	public float attackTime;
	public float attackDamage;
	float attackCooldown = 0;
	//TODO: attack sounds and animation?
	
	GameObject monster;
	public GameObject corpseObject;
	
	public City city;
	
	// Use this for initialization
	new public void Start () {
		//store the monster for later use
		monster = GameObject.FindWithTag("monster");
		
		lastIntersection = city.RandomIntersection();
		transform.position = lastIntersection.Coordinates;
		nextIntersection = lastIntersection;
	}
	
	// Update is called once per frame
	new public void Update () {
		float distToMonster = Vector2.Distance(transform.position, monster.transform.position);
		if (distToMonster <= attackRange) {
			//ATTACK
		} else if (nextIntersection.Contains(transform.position)) {
			PickIntersection();
		} else {
			float moveSpeed = distToMonster <= aggroRange ? runSpeed : walkSpeed;
			Move(curDirection, moveSpeed);
		}
		
		base.Update();
	}
	
	//just rotate sprites and don't bother with animations
	new public bool SetFacing (Direction newDirection) {
		if (curDirection != newDirection) {
			curDirection = newDirection;
			
			int angle = (int)curDirection * 90;
			gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
			
			return true;
		}
		
		return false;
	}
	
	//TODO: communicate with city map
	public bool InIntersection() {
		return false;
	}
	
	//goodnight, sweet prince
	public void Die() {
		GameObject.Instantiate(corpseObject, transform.position, Quaternion.identity);
		Destroy(gameObject);
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
	}
}
