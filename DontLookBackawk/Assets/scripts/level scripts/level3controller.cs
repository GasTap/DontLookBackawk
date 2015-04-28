using UnityEngine;
using System.Collections;

public class level3controller : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (GlobalGameState.playerPreviousLevel != "level2") { return; }
		var bottomSpawn = GameObject.Find("BottomSpawn");
		var player = GameObject.Find("PlayerInput").GetComponent<PlayerInputSystem>().controlledActor;

		Vector2 position = player.transform.position;
		Vector2 velocity = player.GetComponent<Rigidbody2D>().velocity;

		position.x = bottomSpawn.transform.position.x;
		position.y = bottomSpawn.transform.position.y;
		velocity.y = 6;
		velocity.x = 2;
		// TODO safer way to do this
		player.transform.localScale = new Vector2(1,1);

		player.transform.position = position;
		player.GetComponent<Rigidbody2D>().velocity = velocity;
	}
}
