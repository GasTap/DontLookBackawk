using UnityEngine;
using System.Collections;

public class invisible : MonoBehaviour {
	void Start () {
		this.gameObject.GetComponent<Renderer>().enabled = false;
	}
}
