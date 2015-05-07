using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour {

	public float damage = 1;

	public float crackVelocity = 3;
	private bool broken = false;

	public GameObject owner;

	private bool dangerous = true;

	void Update () {
		if (this.GetComponent<PlatformCollider>().grounded) {
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * 0.99f, GetComponent<Rigidbody2D>().velocity.y);
		}
		dangerous = broken == false && GetComponent<Rigidbody2D>().velocity.magnitude > crackVelocity;
	}

	public void OnCollisionEnter2D (Collision2D col) {
		if (broken) { return; }
		if(col.gameObject.tag == "Actor" && col.gameObject != owner) {
			if (dangerous) {
				col.gameObject.GetComponent<ActorBehaviour>().control_take_damage(damage);
			}
			breakEgg();
		} else if (col.relativeVelocity.magnitude >= crackVelocity) {
			breakEgg();
			if (col.gameObject.tag == "Egg") {
				col.gameObject.SendMessage("breakEgg");
			}
		}
	}

	public void die () {
		Destroy(this.gameObject);
	}

	public void breakEgg () {
		if (broken) { return; }
		Debug.Log("BREAK");
		broken = true;
		GetComponent<Rigidbody2D>().fixedAngle = true;
		transform.rotation = Quaternion.identity;
		transform.localScale = new Vector2(transform.localScale.x, 0.1f);
	}
}
