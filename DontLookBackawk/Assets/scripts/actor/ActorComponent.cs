using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorComponent : MonoBehaviour, ActorBehaviour {	
	private float jumpPow = 10f;
	private float flyPow = 5f;
	private float currentFlyPow = 5f;
	private float horAccel = 0.5f;
	private float horAccelAir = 0.2f;
	private float maxHorSpeed = 3.0f;
	
	public GameObject eggPrefab;
	
	private float eggBoostVelAir   = -2f;
	private float eggBoostVelGround = -4f;
	private float eggDistX  = 0.5f;
	private float eggDistY  = -0.2f;
	
	private int layEggTimer = 0;	
	private int willJump = 0;	
	private bool previouslyGrounded = false;
	
	// TODO use or remove?
	private int stateTimer = 0;
	private enum State {
		IDLE,
		WALKING_LEFT,
		WALKING_RIGHT,
		JUMPING,
		JUMP,
		FALLING,
		FLAPPING,
		LAYING,
		PECKING
	}
	State currentState = State.IDLE;
	
	private PlayerInputSystem inputSystem;
	private Animator animator;
	private PlatformCollider platformCollider;
	
	void releaseFromNest () {
		previouslyGrounded = true;
		animator.SetBool("jumped", true);
	}
	
	void Start() {
		animator = this.GetComponent<Animator>();
		inputSystem = GameObject.Find("PlayerInput").GetComponent<PlayerInputSystem>();
		platformCollider = GetComponent<PlatformCollider>();
	}
	
	public void UpdateActor() {
		
		animator.SetBool("jumped", false);
		animator.SetBool("landed", false);
		
		bool grounded = getGrounded();
		animator.SetBool("grounded", grounded);
		
		if (grounded && !previouslyGrounded) {
			animator.SetBool("landed", true);
			currentFlyPow = flyPow;
		}
		animator.SetBool("fall", false);
		if (!grounded && previouslyGrounded && willJump <= 0) {
			animator.SetBool("fall", true);
		}
		
		animator.SetBool("walking", false);
		
		willJump -= 1;
		if (willJump == 1) {
			Vector2 newVelocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpPow);
			GetComponent<Rigidbody2D>().velocity = newVelocity;
		}
		animator.SetInteger("flap", animator.GetInteger("flap") - 1);
		
		layEggTimer -= 1;
		
		previouslyGrounded = grounded;
	}
	
	void setScale () {
		if (this.GetComponent<Rigidbody2D>().velocity.x != 0) {
			this.transform.localScale = new Vector2(
				Mathf.Sign(this.GetComponent<Rigidbody2D>().velocity.x), 
				this.transform.localScale.y
				);
		}
	}
	
	// TODO make all this stuff an interface
	public bool physicsEnabled = true;
	void disablePhysics() { setPhysicsEnabled(false); }
	void enablePhysics() { setPhysicsEnabled(true); }
	void setPhysicsEnabled (bool v) {
		physicsEnabled = v;
		this.gameObject.GetComponent<Rigidbody2D>().isKinematic = !v;
		this.gameObject.GetComponent<Rigidbody2D>().collisionDetectionMode = v ? 
			CollisionDetectionMode2D.Continuous : 
				CollisionDetectionMode2D.None;
	}
	
	public void control_left () {
		if (getGrounded()) {
			animator.SetBool("walking", true);
		}
		Vector2 newVelocity = new Vector2(
			constrainVel(GetComponent<Rigidbody2D>().velocity.x-(getGrounded() ? horAccel : horAccelAir)), 
			GetComponent<Rigidbody2D>().velocity.y
			);
		GetComponent<Rigidbody2D>().velocity = newVelocity;
		setScale();
	}
	
	public void control_right () {
		if (getGrounded()) {
			animator.SetBool("walking", true);
		}
		Vector2 newVelocity = new Vector2(constrainVel(GetComponent<Rigidbody2D>().velocity.x+(getGrounded() ? horAccel : horAccelAir)), GetComponent<Rigidbody2D>().velocity.y);
		GetComponent<Rigidbody2D>().velocity = newVelocity;
		setScale();
	}
	
	public void control_still () {
		if (getGrounded()) {
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * 0.95f, GetComponent<Rigidbody2D>().velocity.y);
		}
	}
	
	public void control_jump () {
		if (canJump()) {
			animator.SetBool("jumped", true);
			jump ();
		} else {
			// flap
			if (GetComponent<Rigidbody2D>().velocity.y <= 0) {
				//Vector2 newVelocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, (GetComponent<Rigidbody2D>().velocity.y + flyPow <= 0) ? GetComponent<Rigidbody2D>().velocity.y + flyPow : 0);
				Vector2 newVelocity = new Vector2(
					GetComponent<Rigidbody2D>().velocity.x, 
					currentFlyPow >= 0 ? currentFlyPow : GetComponent<Rigidbody2D>().velocity.y/2
					);
				currentFlyPow -= 1;
				GetComponent<Rigidbody2D>().velocity = newVelocity;
				animator.SetInteger("flap", 20);
			}
		}
	}
	
	public void control_attack () {
		if (canPeck()) {
			peck();
		}
	}
	
	public void control_special () {
		if (canLayEgg()) {
			layEgg();
		}
	}
	
	public void control_die () {
		// TODO die animation
		Debug.Log ("die");
		if (inputSystem.controlledActor == this.gameObject) {
			GameController.reloadLevel();
		} else {
			Destroy(this.gameObject);
		}
	}
	
	public void die () {
		control_die();
	}
	
	void setPos (Vector2 pos) {
		this.transform.position = pos;
	}
	
	bool canPeck () {
		// TODO
		return true;
	}
	
	void peck() {
		// TODO
		return;
	}
	
	bool canJump () {
		return willJump <= 0 && getGrounded();
	}
	
	void jump () {
		willJump = 8;
	}
	
	bool getGrounded () { 
		return platformCollider.grounded;
	}
	float getDir () { 
		return Mathf.Sign(transform.localScale.x); 
	}
	
	bool canLayEgg () {
		return layEggTimer <= 0;
	}
	
	void layEgg () {
		layEggTimer = 50;
		Vector2 newVelocity = new Vector2(
			GetComponent<Rigidbody2D>().velocity.x + (getGrounded() ? 0 : getDir() * 1), 
			GetComponent<Rigidbody2D>().velocity.y - (getGrounded() ? eggBoostVelGround : eggBoostVelAir)
			);
		GetComponent<Rigidbody2D>().velocity = newVelocity;
		GameObject egg = (GameObject)Instantiate(eggPrefab,eggPos(), Quaternion.identity);
		egg.GetComponent<Rigidbody2D>().velocity = new Vector2(getDir() * (getGrounded() ? -0.2f : -1),0.2f);
		egg.GetComponent<Rigidbody2D>().angularVelocity = eggPrefab.GetComponent<Rigidbody2D>().velocity.x;
		
		transform.position = new Vector2(transform.position.x, transform.position.y + 0.2f);
	}
	
	Vector2 eggPos () {
		return new Vector2(this.transform.position.x - getDir() * eggDistX,
		                   this.transform.position.y + eggDistY);
	}
	
	float constrainVel (float v) {
		return Mathf.Min(Mathf.Max(v, -maxHorSpeed), maxHorSpeed);
	}
}
