using UnityEngine;
using System.Collections;

public class EagleAI : AIBehaviour {

	private int accelDirection = -1;
	private int timer; // count up from zero

	private Vector2 atkVec;

	private Vector2 center; // "center of mass" for patrol area
	private float hover = 1.5f; // allowed deviance from center
	private float range = 6.6f;

	private enum EagleState {
		PATROL,
		ATTACKING,
		RETREATING,
		PANIC
		//WANDER // no aggro + explore new areas
	}

	private EagleState currentState = EagleState.PATROL;
	private EagleActor ac;

	void Start () {
		ac = this.gameObject.GetComponent<EagleActor>();
		center.x = 0f;
		center.y = 3f;
		timer = 0;
	}

	void Update () {

		timer++;

		// ! patrol until target spotted (visionBox Component)
		if (currentState == EagleState.PATROL) {
			RaycastHit2D target = Physics2D.Raycast(transform.position, atkVec);
			if(target.collider.gameObject.ToString == "Player"){
				
			}
			if(target.collider.gameObject.ToString == "Egg"){
				
			}
		}

		// ! do dive at some fixed velocity
		// ! need to collide with ground and sweep along ground for a bit
		if (currentState == EagleState.ATTACKING) {

		}

		// thankfully, we are using the same collider for OWPlatforms.
		// what if its not one way?...
		if (currentState == EagleState.RETREATING) {
			
		}

		// something wrong happens itd be funny if the bird panics
		if (currentState == EagleState.PANIC) {

		}

		// TODO decide if it goes below the stage, does it return?
	}

	void changeDirection () {
		accelDirection *= -1;
	}
}
