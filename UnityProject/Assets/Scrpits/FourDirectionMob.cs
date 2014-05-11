using UnityEngine;
using System.Collections;

public class FourDirectionMob : MonoBehaviour {
	public enum Direction {
		North,
		East,
		South,
		West
	}

	public Direction curDirection;
	
	public bool running;
	public float walkSpeed;
	public float runSpeed;

	// Use this for initialization
	void Start () {
		//TODO: set initial facing
	}
	
	// Update is called once per frame
	void Update () {
		//TODO: remove this control
		if (Input.GetKey (KeyCode.LeftShift)) {
			running = true;
		} else {
			running = false;
		}
		
		if (Input.GetKey (KeyCode.W)) {
			Move (Direction.North);
		}
		if (Input.GetKey (KeyCode.D)) {
			Move (Direction.East);
		}
		if (Input.GetKey (KeyCode.S)) {
			Move (Direction.South);
		}
		if (Input.GetKey (KeyCode.A)) {
			Move (Direction.West);
		}
	}

	// change the direction this mob is facing
	// returns whether the direction was changed (did mot match current direction)
	public bool SetFacing (Direction newDirection) {
		if (curDirection != newDirection) {
			curDirection = newDirection;

			switch (newDirection) {
			case Direction.North:
				gameObject.renderer.material.color = Color.red;
				break;
			case Direction.East:
				gameObject.renderer.material.color = Color.green;
				break;
			case Direction.South:
				gameObject.renderer.material.color = Color.blue;
				break;
			case Direction.West:
				gameObject.renderer.material.color = Color.yellow;
				break;
			}
			
			return true;
		}

		return false;
	}

	public bool Move (Direction moveDirection) {
		if (true) { //TODO: check collision in desired direction
			this.SetFacing (moveDirection);

			float moveSpeed = running ? runSpeed : walkSpeed;

			//translate object
			switch (moveDirection) {
				case Direction.North:
					transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
					break;
				case Direction.East:
					transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
					break;
				case Direction.South:
					transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
					break;
				case Direction.West:
					transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
					break;
			}

			return true;
		}

		return false;
	}
}
