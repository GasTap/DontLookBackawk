using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {	
	private float jumpPow = 9f;
	private float flyPow = 3f;
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

	private float xBound = 7f;
	private float yBound = 4f;

	private int stateTimer = 0;
	private enum State {
		NORMAL,
		PREJUMP,
		JUMPING,
		FALLING,
		PECKING
	}
	State currentState = State.NORMAL;
	
	private Animator animator;

	void Start() {
		this.transform.position = GameController.cachedPlayerPosition;
		animator = this.GetComponent<Animator>();
	}

	void Update() {
		
		animator.SetBool("jumped", false);
		animator.SetBool("landed", false);
		
		bool grounded = getGrounded();
		animator.SetBool("grounded", grounded);
		
		if (grounded && !previouslyGrounded) {
			animator.SetBool("landed", true);
		}
		
		willJump -= 1;
		if (willJump == 1) {
			Vector2 newVelocity = new Vector2(rigidbody2D.velocity.x, jumpPow);
			rigidbody2D.velocity = newVelocity;
		}
		
		if (Input.GetKeyDown(KeyCode.X)) {
			if (canJump()) {
				animator.SetBool("jumped", true);
				jump ();
			} else {
				if (rigidbody2D.velocity.y <= 0) {
					Vector2 newVelocity = new Vector2(rigidbody2D.velocity.x, (rigidbody2D.velocity.y + flyPow <= 0) ? rigidbody2D.velocity.y + flyPow : 0);
					rigidbody2D.velocity = newVelocity;
				}
			}
		}

		animator.SetBool("walking", false);
		if (Input.GetKey(KeyCode.LeftArrow)) {
			if (getGrounded()) {
				animator.SetBool("walking", true);
			}
			Vector2 newVelocity = new Vector2(constrainVel(rigidbody2D.velocity.x-(getGrounded() ? horAccel : horAccelAir)), rigidbody2D.velocity.y);
			rigidbody2D.velocity = newVelocity;
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			if (getGrounded()) {
				animator.SetBool("walking", true);
			}
			Vector2 newVelocity = new Vector2(constrainVel(rigidbody2D.velocity.x+(getGrounded() ? horAccel : horAccelAir)), rigidbody2D.velocity.y);
			rigidbody2D.velocity = newVelocity;
		} else {
			if (grounded) {
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x * 0.95f, rigidbody2D.velocity.y);
			}
		}

		if (this.rigidbody2D.velocity.x != 0) {
			this.transform.localScale = new Vector2(this.rigidbody2D.velocity.x / (Mathf.Abs(this.rigidbody2D.velocity.x)), this.transform.localScale.y);
		}

		if (Input.GetKeyDown(KeyCode.Z) && canPeck()) {
			peck();
		}

		if (Input.GetKeyDown (KeyCode.LeftShift) && canLayEgg()) {
			layEgg();
		}
		
		layEggTimer -= 1;
		
		previouslyGrounded = grounded;

		handleLevelChange();
		Debug.Log(this.transform.position.x);
	}

	void handleLevelChange () {
		bool changed = false;
		Vector2 pos = this.transform.position;
		bool hit = false;
		if (pos.x > xBound) {
			changed = GameController.playerChangeLevel(GameController.RIGHT, new Vector2(-xBound, pos.y), this.rigidbody2D.velocity);
			pos = GameController.cachedPlayerPosition;
			pos.x = changed ? -xBound : xBound;
			hit = true;
		} else if (pos.x < -xBound) {
			changed = GameController.playerChangeLevel(GameController.LEFT, new Vector2(xBound, pos.y), this.rigidbody2D.velocity);
			pos = GameController.cachedPlayerPosition;
			pos.x = changed ? xBound : -xBound;
			hit = true;
		} else if (pos.y > yBound) {
			changed = GameController.playerChangeLevel(GameController.TOP, new Vector2(pos.x, -yBound), this.rigidbody2D.velocity);
			pos = GameController.cachedPlayerPosition;
			pos.y = changed ? -yBound : yBound;
			hit = true;
		} else if (pos.y < -yBound) {
			changed = GameController.playerChangeLevel(GameController.BOTTOM, new Vector2(pos.x, yBound), this.rigidbody2D.velocity);
			pos = GameController.cachedPlayerPosition;
			pos.y = changed ? yBound : -yBound;
			hit = true;
		}
		setPos(pos);
	}

	void die () {
		// TODO
	}

	void setPos (Vector2 pos) {
		this.transform.position = pos;
	}

	bool canPeck () {
		return true;
	}

	void peck() {
		return;
	}
	
	bool canJump () {
		return willJump <= 0 && getGrounded();
	}

	void jump () {
		willJump = 8;
	}
	
	bool getGrounded () { return GetComponent<PlatformCollider>().grounded;}
	float getDir () { return (transform.localScale.x / Mathf.Abs(transform.localScale.x)); }

	bool canLayEgg () {
		return layEggTimer <= 0;
	}

	void layEgg () {
		layEggTimer = 50;
		Vector2 newVelocity = new Vector2(rigidbody2D.velocity.x + (getGrounded() ? 0 : getDir() * 1), 
		                                  rigidbody2D.velocity.y - (getGrounded() ? eggBoostVelGround : eggBoostVelAir));
		rigidbody2D.velocity = newVelocity;
		GameObject egg = (GameObject)Instantiate(eggPrefab,eggPos(), Quaternion.identity);
		egg.rigidbody2D.velocity = new Vector2(getDir() * (getGrounded() ? -0.2f : -1),0.2f);
		egg.rigidbody2D.angularVelocity = eggPrefab.rigidbody2D.velocity.x;
		
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
