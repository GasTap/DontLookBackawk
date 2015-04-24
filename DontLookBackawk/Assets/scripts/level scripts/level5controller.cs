using UnityEngine;
using System.Collections;

public class level5controller : MonoBehaviour {

	void Start () {
		GameObject.Find("MusicManager").SendMessage("fadeOut", "0");
	}
}
