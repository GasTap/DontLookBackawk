using UnityEngine;
using System.Collections;

public class CameraScaler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		float scale = Mathf.Ceil(Screen.height/8);
		Camera.main.orthographicSize = Screen.height / 2f / scale;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
