using UnityEngine;
using System.Collections;

public class ChickenAITest : AIBehaviour {

	int i = -100;
	ActorComponent ac;

	void Start () {
		this.ac = this.GetComponent<ActorComponent>();
	}

	void Update () {

		this.gameObject.SendMessage("UpdateActor");

		i += 1;

		if (i >= 0) {
			if (i == 0) {
				ac.control_special();
			} else {
				ac.control_right();
			}
		} else {
			ac.control_left();
		}
		if (i > 100) {
			i = -100;
			ac.control_jump();
		}
	}
}
