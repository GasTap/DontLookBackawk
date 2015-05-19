using UnityEngine;
using System.Collections;

public class EagleActor : MonoBehaviour, ActorBehaviour {

	// flying
	public float accelX = 0.8f;
	public float accelY = 0.005f;
	public float maxVelX = 5f;
	public float maxVelY = 0.8f;

	// attacking
	public Vector2 atkVec = new Vector2 (-2, -10);
	public Vector2 maxAtkV = new Vector2 (10, 3);
	public bool hasVision;
	public bool hasVictim;

	private Rigidbody2D rb;

	private enum State {
		FLYING,
		ATTACKING
	}
	private State currentState = State.FLYING;

	void Start () {
		rb = this.GetComponent<Rigidbody2D>();
	}
	void Update (){
		capVelocity (rb.velocity);
	}

	public void UpdateActor (){
	}
	public void control_left (){
		if(rb.velocity.x > -maxVelX){
			rb.velocity = new Vector2 (rb.velocity.x - accelX, rb.velocity.y);
		}
		if (atkVec.x > 0 && currentState == State.FLYING) {
			atkVec.x = -atkVec.x;
		}
	}
	public void control_right (){
		if (rb.velocity.x < maxVelX) {
			rb.velocity = new Vector2 (rb.velocity.x + accelX, rb.velocity.y);
		}
		if (atkVec.x < 0 && currentState == State.FLYING) {
			atkVec.x = -atkVec.x;
		}
	}
	public void control_attack (){
		// KILL horizontal, vertical accel
		rb.velocity = new Vector2 (atkVec.x, atkVec.y);
		// apply attackvector rapidly
		this.currentState = State.ATTACKING;

		// TODO: engage hitboxes
	}
	public void control_jump (){
		if (rb.velocity.y < maxVelY) {
			rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + accelY);
		}
	}
	public void control_still (){
		// kill velocity
		//rb.velocity = new Vector2 (0, 0);
	}
	
	public void control_special (){}
	public void control_die (){}
	public void control_take_damage (float amount){}

	private Vector2 capVelocity (Vector2 capMe){
		if(rb.velocity.x < 0){
			return new Vector2(Mathf.Max(capMe.x, -maxVelX), Mathf.Min(capMe.y, maxVelY));
		}
		if(rb.velocity.x > 0){
			return new Vector2(Mathf.Min(capMe.x, maxVelX), Mathf.Min(capMe.y, maxVelY));
		}
		return new Vector2 (capMe.x, Mathf.Min(capMe.y, maxVelY));
	}
}
