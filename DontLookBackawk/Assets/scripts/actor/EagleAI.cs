using UnityEngine;
using System.Collections;

public class EagleAI : AIBehaviour {

	private int timer; // count up from zero

	private Vector2 lineOfSight;

	private Vector2 center; // "center of mass" for patrol area
	private float dy = 0.8f; // allowed deviance y velocity
	private float deadZoneX = 1f; // small area where bird doesnt accel 

	private enum State {
		PATROL,
		ATTACKING,
		RETREATING,
		PANIC
		//WANDER // no aggro + explore new areas
	}

	private State currentState = State.PATROL;
	private EagleActor actor;

	void Start () {
		actor = this.gameObject.GetComponent<EagleActor>();
		center.x = 0f;
		center.y = 3f;
		lineOfSight = actor.atkVec;
		timer = 0;
	}

	void Update () {

		timer++;

		// ! patrol until target spotted (visionBox Component)
		if (currentState == State.PATROL) {
			
			if(this.transform.position.y < center.y - dy){
				if(timer > 5){
					actor.control_jump();
					if(timer < 8){
						timer = 0;
					}
				}
			}
			if(this.transform.position.x < center.x - deadZoneX){
				actor.control_right();
			}
			if(this.transform.position.x > center.x + deadZoneX){
				actor.control_left();
			}
			/*
			lineOfSight = actor.atkVec;
			RaycastHit2D target = Physics2D.Raycast(transform.position, lineOfSight);

			if(target.collider.gameObject.ToString() == "Player"){
				Debug.Log("found player!");
				timer = 0;
				currentState = State.ATTACKING;
			}
			if(target.collider.gameObject.ToString() == "Egg"){
				Debug.Log("found egg!");
				timer = 0;
				currentState = State.ATTACKING;
			}
			*/
		}

		// ! do dive at some fixed velocity
		// ! need to collide with ground and sweep along ground for a bit
		if (currentState == State.ATTACKING) {
			actor.control_attack();
			if(timer > 25){
				timer = 0;
				currentState = State.RETREATING;
			}
		}

		// thankfully, we are using the same collider for OWPlatforms.
		// what if its not one way?...
		if (currentState == State.RETREATING) {
			if(actor.transform.position.y < center.y){
				actor.control_jump ();
			}
			if(timer > 50){
				timer = 0;
				currentState = State.PANIC;
			}
		}

		// something wrong happens itd be funny if the bird panics
		if (currentState == State.PANIC) {}
		// TODO decide if it goes below the stage, does it return?
	}

}
