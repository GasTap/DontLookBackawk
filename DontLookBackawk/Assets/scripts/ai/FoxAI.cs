using UnityEngine;
using System.Collections;

public class FoxAI : AIBehaviour {

	private int stateTimer = 0;
	private enum State {
		IDLE,
		WALKING_LEFT,
		WALKING_RIGHT,
		JUMPING,
		JUMP,
		FALLING,
		ATTACKING
	}
	private State currentState = State.IDLE;

	private int direction = 1;

	ActorComponent ps;
	void Start () {
		ps = this.gameObject.GetComponent<ActorComponent>();
	}

	// Update is called once per frame
	void Update () {

		ps.UpdateActor();

		stateTimer = Mathf.Max (stateTimer - 1, 0);
		if (stateTimer == 0) {
			if (currentState == State.IDLE) {
				stateTimer = 60;
				changeDirection();
				if (direction == 1) {
					currentState = State.WALKING_RIGHT;
				} else {
					currentState = State.WALKING_LEFT;
				}
			} else {
				stateTimer = 15;
				currentState = State.IDLE;
			}
		}

		if (currentState == State.IDLE) {
			ps.control_still();
		} else if (currentState == State.WALKING_LEFT) {
			ps.control_left();
		} else if (currentState == State.WALKING_RIGHT) {
			ps.control_right();
		}
	}

	void changeDirection () {
		direction *= -1;
	}
}
