using UnityEngine;
using System.Collections;

public class RemoveWhenOffScreen : MonoBehaviour {

	Transform t;
	float boundX = 7;
	float boundY;
	void Start () {
		t = this.gameObject.transform;
		boundY = boundX * (3.0f / 4.0f);
	}

	void Update () {
		var p = t.position;
		// TODO factor in width and height
		if (p.x < -boundX) {
			Destroy(this.gameObject);
		} else if (p.x > boundX) {
			Destroy(this.gameObject);
		} else if (p.y < -boundY) {
			Destroy(this.gameObject);
		} else if (p.y > boundY) {
			Destroy(this.gameObject);
		}
	}
}
