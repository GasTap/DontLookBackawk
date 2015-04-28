using UnityEngine;
using System.Collections;

public class ChickenAITest : AIBehaviour {

	int i = -100;
	PlayerScript ps;

	void Start () {
		this.ps = this.GetComponent<PlayerScript>();
	}

	void Update () {

		this.gameObject.SendMessage("UpdateActor");

		i += 1;

		if (i >= 0) {
			if (i == 0) {
				ps.control_special();
			} else {
				ps.control_right();
			}
		} else {
			ps.control_left();
		}
		if (i > 100) {
			i = -100;
			ps.control_jump();
		}
	}
}
