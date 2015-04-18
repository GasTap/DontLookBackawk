using UnityEngine;
using System.Collections;

public class CameraScaler : MonoBehaviour {
	void Start () {
		Screen.SetResolution(800, 600, false);
		/*
		Screen.SetResolution(800, 600, true);
		float UnitsPerPixel = 1f / 50f;

		Debug.Log(Screen.height);
		Debug.Log("camera scale " + Screen.height / 2f * UnitsPerPixel);

		Camera.main.orthographicSize =
			Screen.height / 2f // ortho-size is half the screen height...
				* UnitsPerPixel;
		*/
	}
}
