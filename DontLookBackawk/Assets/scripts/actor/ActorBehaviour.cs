using UnityEngine;
using System.Collections;

public interface ActorBehaviour {
	void UpdateActor ();
	void control_left ();
	void control_right ();
	void control_still ();
	void control_jump ();
	void control_attack ();
	void control_special ();
	void control_die ();
	void control_take_damage (float amount);
}
