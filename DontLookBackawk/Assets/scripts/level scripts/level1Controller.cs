using UnityEngine;
using System.Collections;

public class level1Controller : MonoBehaviour {

	private GameObject player;
	public Transform BlackOverlay;

	private bool sequenceEnabled = false;

	void Start () {
		sequenceEnabled = GlobalGameState.playedOpeningAnimation == false;
		if (sequenceEnabled) {
			player = GameObject.FindGameObjectWithTag("Player");
			player.transform.position = new Vector2(-5, -2.3f);
			Debug.Log(player);
			player.SendMessage("disableControl");
			player.SendMessage("disablePhysics");
			// TODO add fadein prefab to scene
			Instantiate(BlackOverlay, new Vector3(0,0,-2), Quaternion.identity);
		}
	}

	private int i = 0;
	// TODO fade in, have chicken sleeping animation, animation wakes up, replace animation with player, enable controls
	void Update()
	{
		if (!sequenceEnabled) { return; }
		i += 1;
		if (i < 4000 / 16) { return; } 

		GameObject.Find("MusicManager").SendMessage("switchMusicByName", "4");

		player.SendMessage("enableControl");
		player.SendMessage("enablePhysics");
		player.SendMessage("releaseFromNest");

		Vector2 velocity = player.GetComponent<Rigidbody2D>().velocity;

		velocity.y = 3;
		velocity.x = 1;

		player.GetComponent<Rigidbody2D>().velocity = velocity;

		StopCoroutine("DoSequence");
		GlobalGameState.playedOpeningAnimation = true;
		Destroy(this);
	}
}
