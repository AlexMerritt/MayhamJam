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
	float stateTimeMin = 0.5f;
	float stateTimeMax = 1.5f;
	
	float wanderMoveChance = 0.4f;
	
	public Vector2 fleePosition;

	// Use this for initialization
	new public void Start () {
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
		if (UnityEngine.Random.value >= wanderMoveChance) {
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
			//TODO: pick direction AWAY from target
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
}
