using UnityEngine;
using System.Collections;

public class fadein : MonoBehaviour {

	public int duration = 1000;

	private Renderer renderer;

	void Start () {
		renderer = this.GetComponent<Renderer>();
		var c = renderer.material.color;
		c.a = 1;
		renderer.material.color = c;
	}

	private float i = 0;
	void Update () {
		i += 16.666f; // TODO use actual time difference
		if (i > duration) {
			Destroy (this.gameObject);
		}
		var v = 1 - (i / duration);
		var c = renderer.material.color;
		c.a = v;
		renderer.material.color = c;
	}
}
