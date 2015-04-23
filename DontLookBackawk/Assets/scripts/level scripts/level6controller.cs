using UnityEngine;
using System.Collections;

public class level6controller : MonoBehaviour {
	void Start () {
		GameObject.Find("MusicManager").SendMessage("switchMusicByName", "1");
	}
}
