using UnityEngine;
using System.Collections;

public class fadein : MonoBehaviour {

	public int duration = 1000;

	private Renderer rendererComponent;

	void Start () {
		rendererComponent = this.GetComponent<Renderer>();
		var c = rendererComponent.material.color;
		c.a = 1;
		rendererComponent.material.color = c;
	}

	private float i = 0;
	void Update () {
		i += 16.666f; // TODO use actual time difference
		if (i > duration) {
			Destroy (this.gameObject);
		}
		var v = 1 - (i / duration);
		var c = rendererComponent.material.color;
		c.a = v;
		rendererComponent.material.color = c;
	}
}
