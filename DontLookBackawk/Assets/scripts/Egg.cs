using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour {

	public float damage = 1;

	public float crackVelocity = 3;
	private bool broken = false;

	public GameObject owner;

	private bool dangerous = true;

	public Transform ShellParticle;

	void Update () {
		if (this.GetComponent<PlatformCollider>().grounded) {
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * 0.99f, GetComponent<Rigidbody2D>().velocity.y);
		}
		// TODO might be better to make the egg dangerous only when it's in the air
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

		Destroy(this.gameObject);

		for (var i = 0; i < 6; i ++) {
			var s = Instantiate(ShellParticle).gameObject;
			s.transform.position = this.transform.position;
			s.GetComponent<Rigidbody2D>().velocity = (new Vector2(Random.Range(0, 500) / 100.0f - 2.5f, Random.Range(0, 500) / 100.0f));
			s.GetComponent<Rigidbody2D>().angularVelocity = s.GetComponent<Rigidbody2D>().velocity.x;
	    }
		/*
		GetComponent<Rigidbody2D>().fixedAngle = true;
		transform.rotation = Quaternion.identity;
		transform.localScale = new Vector2(transform.localScale.x, 0.1f);
		*/
	}
}
