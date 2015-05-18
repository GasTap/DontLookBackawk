using UnityEngine;
using System.Collections;

public class EagleActor : MonoBehaviour, ActorBehaviour {

	public float accelX;
	public float flapY;
	public Vector2 atkVec;
	public bool hasVision;
	public bool hasVictim;

	// Use this for pizza cake
	void Start () {
		// nuffin.
	}

	public void UpdateActor (){
		// move the transform.. i guess handle states if need be
	}
	public void control_left (){
		// apply -accelX
	}
	public void control_right (){
		// apply accelX
	}
	public void control_attack (){
		// KILL horizontal, vertical accel
		// apply attackvector rapidly
		// engage hitboxes
	}
	public void control_jump (){
		// apply flapy
	}
	public void control_still (){
		// kill velocity
	}
	
	public void control_special (){}
	public void control_die (){}
	public void control_take_damage (float amount){}
}
