using UnityEngine;
using System.Collections;
using System;

public class FourDirectionMob : MonoBehaviour {
	public enum Direction {
		North,
		West,
		South,
		East
	}

	public Direction curDirection;
	
	public float walkSpeed;
	public float runSpeed;
	public bool moving;
	
	public Animator anim;

	// Use this for initialization
	public void Start () {
		
	}
	
	// Update is called once per frame
	public void Update () {
		anim.SetBool("Moving", moving);
		
		//reset movement variable
		moving = false;
	}
	
	public bool RandomizeFacing () {
		Array values = Enum.GetValues(typeof(Direction));
		Direction randomDirection = (Direction)values.GetValue(UnityEngine.Random.Range(0, values.Length));
		return SetFacing(randomDirection);
	}

	// change the direction this mob is facing
	// returns whether the direction was changed (did mot match current direction)
	public bool SetFacing (Direction newDirection) {
		if (curDirection != newDirection) {
			curDirection = newDirection;
			
			anim.SetInteger("MoveDirection", (int)newDirection);
			
//			switch (newDirection) {
//			case Direction.North:
//				gameObject.renderer.material.color = Color.white;
//				break;
//			case Direction.East:
//				gameObject.renderer.material.color = Color.green;
//				break;
//			case Direction.South:
//				gameObject.renderer.material.color = Color.blue;
//				break;
//			case Direction.West:
//				gameObject.renderer.material.color = Color.yellow;
//				break;
//			}
			
			return true;
		}

		return false;
	}

	public bool Move (Direction moveDirection, float moveSpeed) {
		if (true) { //TODO: check collision in desired direction?
			SetFacing (moveDirection);
			
			moving = true;

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
	
	public Direction VecToDirection(Vector3 dir) {
		if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) {
			return dir.x > 0 ? Direction.East : Direction.West;
		} else {
			return dir.y > 0 ? Direction.North : Direction.South;
		}
	}
	
	public Direction TurnRight(Direction dir) {
		return (Direction)(((int)dir + 3) % 4);
	}
	
	public Direction TurnLeft(Direction dir) {
		return (Direction)(((int)dir + 1) % 4);
	}
	
	public Direction TurnAround(Direction dir) {
		return (Direction)(((int)dir + 2) % 4);
	}
}
