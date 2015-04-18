using UnityEngine;
using System.Collections;

public class level1Controller : MonoBehaviour {

	private GameObject player;

	private bool sequenceEnabled = false;

	void Start () {
		sequenceEnabled = GlobalGameState.playedOpeningAnimation == false;
		Debug.Log(GlobalGameState.playedOpeningAnimation);
		if (sequenceEnabled) {
			player = GameObject.FindGameObjectWithTag("Player");
			player.transform.position = new Vector2(-5, -2.5f);
			Debug.Log(player);
			player.SendMessage("disableControl");
			player.SendMessage("disablePhysics");
		}
	}

	private int i = 0;
	// TODO fade in, have chicken sleeping animation, animation wakes up, replace animation with player, enable controls
	void Update()
	{
		if (!sequenceEnabled) { return; }
		i += 1;
		if (i < 1000 / 16) { return; } 
		player.SendMessage("enableControl");
		player.SendMessage("enablePhysics");
		StopCoroutine("DoSequence");
		GlobalGameState.playedOpeningAnimation = true;
		Destroy(this);
	}
}
