using UnityEngine;
using System.Collections;

public class PlayerInputSystem : MonoBehaviour {

	public GameObject controlledActor;
	public Transform Player;

	public GameObject getControlActor () {
		return controlledActor;
	}

	public void setControlledActor (GameObject a) {
		controlledActor = a;
	}

	public void removeControlledActor () {
		controlledActor = null;
	}

	private void removeControl () {
		if (controlledActor.GetComponent<AIBehaviour>()) {
			controlledActor.GetComponent<AIBehaviour>().enabled = true;
		}
		controlledActor = null;
	}

	private void assumeControl (GameObject a) {
		if (controlledActor != null) {
			removeControl();
		}
		controlledActor = a;
		if (controlledActor.GetComponent<AIBehaviour>()) {
			controlledActor.GetComponent<AIBehaviour>().enabled = false;
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			var actors = GameObject.FindGameObjectsWithTag("Actor");
			if (controlledActor == null) {
				assumeControl(actors[0]);
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
				assumeControl(actors[(c + 1) % actors.Length]);
			}
		}

		if (Input.GetKeyDown (KeyCode.S)) {
			var mp = Input.mousePosition;
			Debug.Log(mp.x);
			Debug.Log(mp.y);
			var t = (Transform)(Instantiate(Player, new Vector3(mp.x/100 - 3.5f,mp.y/100 - 3.5f,0), Quaternion.identity));
			assumeControl(t.gameObject);
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
