using UnityEngine;
using System.Collections;

public class OpeningController : MonoBehaviour {

	private GameObject player;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = new Vector2(-5, -2.5f);
		Debug.Log(player);
		player.SendMessage("disableControl");
		player.SendMessage("disablePhysics");
	}

	private int i = 0;
	// TODO fade in, have chicken sleeping animation, animation wakes up, replace animation with player, enable controls
	void Update()
	{
		i += 1;
		if (i < 1000 / 16) { return; } 
		player.SendMessage("enableControl");
		player.SendMessage("enablePhysics");
		StopCoroutine("DoSequence");
		Destroy(this);
	}
}
