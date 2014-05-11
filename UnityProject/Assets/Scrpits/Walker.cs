using UnityEngine;
using System.Collections;
using System;

public class Walker : FourDirectionMob {
	enum WalkerState {
		Stand,
		Walk,
		Flee
	}

	WalkerState curState;
	float stateTimer;
	float stateTimeMin = 0.3f;
	float stateTimeMax = 1.0f;
	
	float wanderMoveChance = 0.4f;
	
	float fleeDetectRange = 1.5f;
	
	GameObject monster;
	public GameObject corpseObject;

	// Use this for initialization
	new public void Start () {
		//give this person a bit of color
		gameObject.renderer.material.color = new Color(
			UnityEngine.Random.Range(0.9f, 1.0f),
			UnityEngine.Random.Range(0.8f, 1.0f),
			UnityEngine.Random.Range(0.6f, 1.0f));
			
		monster = GameObject.FindWithTag("monster");
	
		PickState();
	}
	
	// Update is called once per frame
	new public void Update () {
		bool didUpdate = UpdateState(curState);
		
		if (!didUpdate) {
			PickState();
		}
		
		base.Update();
	}
	
	// automatically pick a state to enter
	void PickState() {
		if (monster != null && Vector2.Distance(transform.position, monster.transform.position) <= fleeDetectRange) {
			EnterState(WalkerState.Flee);
		} else if (UnityEngine.Random.value >= wanderMoveChance) {
			EnterState(WalkerState.Stand);
		} else {
			EnterState(WalkerState.Walk);
		}
	}

	// forcibly transition to a new state
	void SwitchState(WalkerState newState) {
		LeaveState (curState);
		EnterState (newState);
		curState = newState;
	}
	
	// logic to perform when a state is started
	void EnterState(WalkerState enterState) {
		stateTimer = UnityEngine.Random.Range(stateTimeMin, stateTimeMax);
		
		switch (enterState) {
		case WalkerState.Stand:
			//do nothing.
			break;
		case WalkerState.Walk:
			RandomizeFacing();
			break;
		case WalkerState.Flee:
			Vector3 relPos = transform.position - monster.transform.position;
			
			Direction newDirection = VecToDirection(relPos);
			
			float turnChance = UnityEngine.Random.value;
			if (turnChance <= 0.15) {
				newDirection = TurnRight(newDirection);
			} else if (turnChance <= 0.3) {
				newDirection = TurnLeft(newDirection);
			}
			
			SetFacing(newDirection);
			
			break;
		}
		
		curState = enterState;
	}

	// perform the state update and return true if state was updated, false if it ended
	bool UpdateState(WalkerState updateState) {
		stateTimer -= Time.deltaTime;
		if (stateTimer <= 0) {
			LeaveState(curState);
			return false;
		}
	
		switch (updateState) {
		case WalkerState.Stand:
			//do nothing.
			break;
		case WalkerState.Walk:
			Move(curDirection, walkSpeed);
			break;
		case WalkerState.Flee:
			Move(curDirection, runSpeed);
			break;
		}
		
		return true;
	}

	// logic to perform when a state is ended
	void LeaveState(WalkerState leaveState) {
		switch (leaveState) {
		case WalkerState.Stand:
			//do nothing.
			break;
		case WalkerState.Walk:
			//do nothing.
			break;
		case WalkerState.Flee:
			break;
		}
	}
	
	//goodnight, sweet prince
	public void Die() {
		GameObject.Instantiate(corpseObject, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
