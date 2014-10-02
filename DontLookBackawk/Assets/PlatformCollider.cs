using UnityEngine;
using System.Collections;

public class PlatformCollider : MonoBehaviour {
	
	public bool grounded = false;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit2D ray = Physics2D.Raycast(transform.position - Vector3.up * 0.6f, -Vector2.up, 0.1f);
		if (ray.collider != null)
		{
			if (ray.collider.gameObject.tag == "Platform")
			{
				grounded = true;
			} 
		} else
		{
			grounded = false;
		}
	}
}
