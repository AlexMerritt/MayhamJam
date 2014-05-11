using UnityEngine;
using System.Collections;

public class Monster : FourDirectionMob {
	public float health;
	public float damage;
	public float attackSpeed;
	
	public float curMomentum;
	float maxMomentum = 50f;
	float slapStrength = 2.0f;
	float enticeStrength = 8.0f;
	
	float enticeTurnThreshold = 20f;
	
	float wanderThreshold = 5.0f;
	float wanderCooldownTime = 1.5f;
	float wanderCooldown;
	
	float baseDecay = 10.0f;

	public GameObject deadHuman;

	public Vector2 stompSize;
	public float stompPos;

	// Use this for initialization
	new public void Start () {
		RandomizeFacing();
		wanderCooldown = wanderCooldownTime;
	}
	
	// Update is called once per frame
	new public void Update () {
		//handle input
		if (Input.GetKeyDown (KeyCode.Space)) {
			Slap();
		}
		
		if (Input.GetMouseButtonDown(0)) {
			//Yeah this is a really silly way to do it but meh
			var pos = Camera.main.WorldToScreenPoint(transform.position);
			var dir = Input.mousePosition - pos;
			var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			angle = (angle + 315) % 360;
			FourDirectionMob.Direction enticeDirection = (FourDirectionMob.Direction)Mathf.Floor(angle / 90);
			
			Entice(enticeDirection);
		}
		
		//TODO: check for things to respond to
		
		//wander if momentum is critically low
		if (curMomentum < wanderThreshold && wanderCooldown <= 0) {
			RandomizeFacing();
			wanderCooldown = wanderCooldownTime;
		}
		
		//if nothing interesting, just move forward
		float moveSpeed = Mathf.Max(walkSpeed, runSpeed * (curMomentum / maxMomentum));
		Move (curDirection, moveSpeed);
		
		//update stomp box
		Rect stompBox = new Rect(
			transform.position.x-stompSize.x*0.5f,
			transform.position.y-stompSize.y*0.5f + stompPos,
			stompSize.x,
			stompSize.y);

		Collider2D[] hits = Physics2D.OverlapAreaAll(
			new Vector2(stompBox.xMin, stompBox.yMax),
			new Vector2(stompBox.xMax, stompBox.yMax));
		foreach (Collider2D hit in hits) {
			Building bldg = hit.GetComponent<Building>();
			if (bldg != null)
				bldg.Damage();
		}

		//crush puny humans
		GameObject[] humans = GameObject.FindGameObjectsWithTag("human");
		foreach (GameObject human in humans) {
			if (stompBox.Contains(human.transform.position)) {
				ParticleHelper.Instance.Splat(human.transform.position);
				human.GetComponent<Walker>().Die();
			}
		}
		
		//lose momentum over time
		curMomentum -= baseDecay * Time.deltaTime;
		if (curMomentum < 0) curMomentum = 0;
		
		//decrement timer(s)
		if (wanderCooldown > 0)
			wanderCooldown -= Time.deltaTime;
			
		
	}
	
	//punish the monster, slowing it down and hurting its feelings
	public void Slap() {
		//TODO: play slap animation
		curMomentum = Mathf.Max(curMomentum - slapStrength, 0);
	}
	
	//encourage the monster in a certain direction. if the direction differs from
	//the current direction, will reduce momentum
	public void Entice(Direction dir) {
		//TODO: play entice animation
		if (dir == curDirection) {
			curMomentum = Mathf.Min(curMomentum + enticeStrength, maxMomentum);
		} else {
			curMomentum = Mathf.Max(curMomentum - enticeStrength, 0);
			
			if (curMomentum < enticeTurnThreshold && dir != TurnAround(curDirection)) {
				SetFacing(dir);
			}
		}
	}
}
