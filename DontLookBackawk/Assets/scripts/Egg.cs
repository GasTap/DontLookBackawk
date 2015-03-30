using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour {

	private double crackVelocity = 7.5;
	private bool broken = false;

	void Update () {
		if (this.GetComponent<PlatformCollider>().grounded) {
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * 0.99f, GetComponent<Rigidbody2D>().velocity.y);

			if (GetComponent<Rigidbody2D>().velocity.magnitude > crackVelocity && !broken) {
				Debug.Log(GetComponent<Rigidbody2D>().velocity.magnitude);
				Debug.Log(crackVelocity);
				breakEgg();
			}
		}
	}

	public void breakEgg () {
		Debug.Log("BREAK");
		GetComponent<Rigidbody2D>().fixedAngle = true;
		transform.rotation = Quaternion.identity;
		transform.localScale = new Vector2(transform.localScale.x, 0.1f);
	}
}
