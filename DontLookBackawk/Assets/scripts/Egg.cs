using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour {

	private double crackVelocity = 7.5;
	private bool broken = false;

	void Update () {
		if (this.GetComponent<PlatformCollider>().grounded) {
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x * 0.99f, rigidbody2D.velocity.y);

			if (rigidbody2D.velocity.magnitude > crackVelocity && !broken) {
				Debug.Log(rigidbody2D.velocity.magnitude);
				Debug.Log(crackVelocity);
				breakEgg();
			}
		}
	}

	public void breakEgg () {
		Debug.Log("BREAK");
		rigidbody2D.fixedAngle = true;
		transform.rotation = Quaternion.identity;
		transform.localScale = new Vector2(transform.localScale.x, 0.1f);
	}
}
