using UnityEngine;
using System.Collections;

public class TestControls : FourDirectionMob {
	// Update is called once per frame
	new public void Update () {
		//TODO: remove this control
		bool running = Input.GetKey (KeyCode.LeftShift);
		float moveSpeed = running ? runSpeed : walkSpeed;
		
		if (Input.GetKey (KeyCode.W)) {
			Move (Direction.North, moveSpeed);
		}
		if (Input.GetKey (KeyCode.D)) {
			Move (Direction.East, moveSpeed);
		}
		if (Input.GetKey (KeyCode.S)) {
			Move (Direction.South, moveSpeed);
		}
		if (Input.GetKey (KeyCode.A)) {
			Move (Direction.West, moveSpeed);
		}
		
		base.Update ();
	}
}
