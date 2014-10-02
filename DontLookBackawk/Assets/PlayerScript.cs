using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{

    float jumpPow = 9f;
    float flyPow = 3f;
    float horAccel = 0.5f;
	float horAccelAir = 0.2f;
	float maxHorSpeed = 3.0f;
    bool grounded = true;
	public GameObject eggPrefab;

	float eggBoosVelAir   = -2f;
	float eggBoosVelGround = -4f;
	float eggDistX  = 0.5f;
	float eggDistY  = -0.2f;

	float originalScale = 1;

	int layEggTimer = 0;

	int willJump = 0;

	bool previouslyGrounded = false;

	Animator animator;

    // Use this for initialization
    void Start()
    {
		originalScale = this.transform.localScale.x;
		animator = this.GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (willJump <= 0 && grounded)
            {
				animator.SetBool("jumped", true);
				willJump = 8;
            } else
            {
				if (rigidbody2D.velocity.y <= 0) {
	                Vector2 newVelocity = new Vector2(rigidbody2D.velocity.x, (rigidbody2D.velocity.y + flyPow <= 0) ? rigidbody2D.velocity.y + flyPow : 0);
	                rigidbody2D.velocity = newVelocity;
				}
            }
		}
		animator.SetBool("walking", false);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
			if (getGrounded()) {
				animator.SetBool("walking", true);
			}
			Vector2 newVelocity = new Vector2(constrainVel(rigidbody2D.velocity.x-(getGrounded() ? horAccel : horAccelAir)), rigidbody2D.velocity.y);
            rigidbody2D.velocity = newVelocity;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
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

		this.transform.localScale = new Vector2(this.rigidbody2D.velocity.x / (Mathf.Abs(this.rigidbody2D.velocity.x)), this.transform.localScale.y);

		if (layEggTimer <= 0 && Input.GetKeyDown (KeyCode.LeftShift)) {
			layEgg();
		}

		layEggTimer -= 1;

		previouslyGrounded = grounded;
    }

	bool getGrounded () { return GetComponent<PlatformCollider>().grounded;}
	float getDir () { return (transform.localScale.x / Mathf.Abs(transform.localScale.x)); }

	void layEgg () {
		layEggTimer = 50;
		Vector2 newVelocity = new Vector2(rigidbody2D.velocity.x + (getGrounded() ? 0 : getDir() * 1), rigidbody2D.velocity.y - (getGrounded() ? eggBoosVelGround : eggBoosVelAir));
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
