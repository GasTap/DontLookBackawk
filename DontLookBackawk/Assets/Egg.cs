using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour {

	public int crackTimer = 20;
	public bool broken = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		crackTimer -= 1;
		if (this.GetComponent<PlatformCollider>().grounded) {
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x * 0.99f, rigidbody2D.velocity.y);
			if (crackTimer <= 0) {
				breakEgg();
			}
			crackTimer = 30;
		}
	}

	public void breakEgg () {
		Debug.Log("BREAK");
		rigidbody2D.fixedAngle = true;
		transform.rotation = Quaternion.identity;
		transform.localScale = new Vector2(transform.localScale.x, 0.1f);
	}
}
