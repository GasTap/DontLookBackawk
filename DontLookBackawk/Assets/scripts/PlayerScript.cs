using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour {	
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

	private float xBound = 7f;
	private float yBound = (7 * 3 / 4f);

	private bool controlEnabled = true;

	// TODO use or remove?
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

	public void OnLevelLoad (List<GameObject> onStage) {
		if (!GameController.hideDebugObjects) { return; }
		var stuff = new List<GameObject>();
		stuff.AddRange(GameObject.FindGameObjectsWithTag("Platform"));
		stuff.AddRange(GameObject.FindGameObjectsWithTag("OWPlatform"));
		stuff.AddRange(GameObject.FindGameObjectsWithTag("Spawn"));
		stuff.AddRange(GameObject.FindGameObjectsWithTag("Exit"));
		stuff.AddRange(GameObject.FindGameObjectsWithTag("Death"));
		
		foreach (GameObject i in stuff) {
			i.GetComponent<Renderer>().enabled = false;
		}
	}

	void disableControl() { setControlEnabled(false); }
	void enableControl()  { setControlEnabled(true); }
	void setControlEnabled (bool v) {
		controlEnabled = v;
	}

	void disablePhysics() { setPhysicsEnabled(false); }
	void enablePhysics()  { setPhysicsEnabled(true); }
	void setPhysicsEnabled (bool v) {
		this.gameObject.GetComponent<Rigidbody2D>().isKinematic = !v;
		this.gameObject.GetComponent<Rigidbody2D>().collisionDetectionMode = v ? CollisionDetectionMode2D.Continuous : CollisionDetectionMode2D.None;
	}

	void releaseFromNest () {
		previouslyGrounded = true;
		animator.SetBool("jumped", true);
	}

	void Start() {
		animator = this.GetComponent<Animator>();
	}

	private bool rightDown = false;
	private bool leftDown = false;
	void Update() {
		
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

		if (controlEnabled) { 
			willJump -= 1;
			if (willJump == 1) {
				Vector2 newVelocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpPow);
				GetComponent<Rigidbody2D>().velocity = newVelocity;
			}
			animator.SetInteger("flap", animator.GetInteger("flap") - 1);
			if (Input.GetKeyDown(KeyCode.X)) {
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

			if (Input.GetKey(KeyCode.LeftArrow)) {
				//Debug.Log("left");
				if (getGrounded()) {
					animator.SetBool("walking", true);
				}
				Vector2 newVelocity = new Vector2(constrainVel(GetComponent<Rigidbody2D>().velocity.x-(getGrounded() ? horAccel : horAccelAir)), GetComponent<Rigidbody2D>().velocity.y);
				GetComponent<Rigidbody2D>().velocity = newVelocity;
				//Debug.Log(newVelocity);
				setScale();
			} else if (Input.GetKey(KeyCode.RightArrow)) {
				//Debug.Log("right");
				if (getGrounded()) {
					animator.SetBool("walking", true);
				}
				Vector2 newVelocity = new Vector2(constrainVel(GetComponent<Rigidbody2D>().velocity.x+(getGrounded() ? horAccel : horAccelAir)), GetComponent<Rigidbody2D>().velocity.y);
				GetComponent<Rigidbody2D>().velocity = newVelocity;
				//Debug.Log(newVelocity);
				setScale();
			} else {
				if (grounded) {
					GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * 0.95f, GetComponent<Rigidbody2D>().velocity.y);
				}
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
		}
	}

	void setScale () {
		if (this.GetComponent<Rigidbody2D>().velocity.x != 0) {
			this.transform.localScale = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x / (Mathf.Abs(this.GetComponent<Rigidbody2D>().velocity.x)), this.transform.localScale.y);
		}
	}

	void handleLevelChange () {
		bool changed = false;
		Vector2 pos = this.transform.position;
		if (pos.x > xBound) {
			changed = GameController.playerChangeLevel(LevelData.RIGHT, new Vector2(-xBound, pos.y), this.GetComponent<Rigidbody2D>().velocity);
			pos.x = changed ? -xBound : xBound;
		} else if (pos.x < -xBound) {
			changed = GameController.playerChangeLevel(LevelData.LEFT, new Vector2(xBound, pos.y), this.GetComponent<Rigidbody2D>().velocity);
			pos.x = changed ? xBound : -xBound;
		} else if (pos.y > yBound) {
			changed = GameController.playerChangeLevel(LevelData.TOP, new Vector2(pos.x, -yBound), this.GetComponent<Rigidbody2D>().velocity);
			pos.y = yBound;
		} else if (pos.y < -yBound) {
			changed = GameController.playerChangeLevel(LevelData.BOTTOM, new Vector2(pos.x, yBound), this.GetComponent<Rigidbody2D>().velocity);
			pos.y = changed ? yBound : -yBound;
		}
		setPos(pos);
	}

	void die () {
		// TODO die animation
		Debug.Log ("die");
		GameController.reloadLevel();
		this.transform.position = new Vector2(GlobalGameState.playerEntrancePosition.x, GlobalGameState.playerEntrancePosition.y);
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
		Vector2 newVelocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x + (getGrounded() ? 0 : getDir() * 1), 
		                                  GetComponent<Rigidbody2D>().velocity.y - (getGrounded() ? eggBoostVelGround : eggBoostVelAir));
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
