using UnityEngine;
using System.Collections;

public class PlayerInputSystem : MonoBehaviour {

	public GameObject controlledActor;

	public GameObject getControlActor () {
		return controlledActor;
	}

	public void setControlledActor (GameObject a) {
		controlledActor = a;
	}

	public void removeControlledActor () {
		controlledActor = null;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			var actors = GameObject.FindGameObjectsWithTag("Actor");
			if (controlledActor == null) {
				controlledActor = actors[0];
				if (controlledActor.GetComponent<AIBehaviour>()) {
					controlledActor.GetComponent<AIBehaviour>().enabled = false;
				}
			} else {
				var currentPlayer = 0;
				var c = 0;
				foreach (var actor in actors) {
					if (actor == controlledActor) {
						currentPlayer = c;
						break;
					}
					c += 1;
				}
				var aib = controlledActor.GetComponent<AIBehaviour>();
				if (aib) {
					aib.enabled = true;
				}
				controlledActor = actors[(c + 1) % actors.Length];
				aib = controlledActor.GetComponent<AIBehaviour>();
				if (aib) {
					aib.enabled = false;
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.S)) {
			// TODO spawn player at mouse
		}

		if (controlledActor == null) { return; }

		controlledActor.SendMessage("UpdateActor");

		if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) {
			controlledActor.SendMessage("control_still");
		} else if (Input.GetKey(KeyCode.LeftArrow)) {
			controlledActor.SendMessage("control_left");
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			controlledActor.SendMessage("control_right");
		} else {
			controlledActor.SendMessage("control_still");
		}
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			controlledActor.SendMessage("control_special");
		}
		if (Input.GetKeyDown (KeyCode.X)) {
			controlledActor.SendMessage("control_jump");
		}
		if (Input.GetKeyDown (KeyCode.Z)) {
			controlledActor.SendMessage("control_attack");
		}
	}
}
