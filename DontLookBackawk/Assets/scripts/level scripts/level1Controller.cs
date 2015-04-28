using UnityEngine;
using System.Collections;

public class level1Controller : MonoBehaviour {

	private GameObject player;
	public Transform BlackOverlay;
	public GameObject playerSpawn;

	private bool sequenceEnabled = false;

	void Start () {
		sequenceEnabled = GlobalGameState.playedOpeningAnimation == false;
		// TODO instantiate a prefab player
		if (sequenceEnabled) {
			playerSpawn.name = "Player";
			player = playerSpawn;
			player.transform.position = new Vector2(-5, -2.3f);
			GameObject.Find("PlayerInput").SendMessage("removeControlledActor");
			player.SendMessage("disablePhysics");
			Instantiate(BlackOverlay, new Vector3(0,0,-2), Quaternion.identity);
			GameObject.Find("MusicManager").SendMessage("stopMusic");
		} else {
			Destroy (playerSpawn);
		}
	}

	private int i = 0;
	void Update()
	{
		if (!sequenceEnabled) { return; }
		i += 1;
		if (i < 4000 / 16) { return; } 

		GameObject.Find("MusicManager").SendMessage("switchMusicByName", "4");

		GameObject.Find("PlayerInput").SendMessage("setControlledActor", player);
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
