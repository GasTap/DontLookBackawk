using UnityEngine;
using System.Collections;

public class PlatformCollider : MonoBehaviour {
	
	public bool grounded = false;

	void OnCollisionStay2D(Collision2D col)
	{
		if(col.gameObject.tag == "Platform")
		{
			RaycastHit2D ray = Physics2D.Raycast(transform.position - Vector3.up * 0.6f, -Vector2.up, 0.1f);
			if (ray.collider != null && ray.collider.gameObject.tag == "Platform") 
			{
				grounded = true;
			}
		}
	}

	void OnCollisionExit2D(Collision2D col)
	{
		if(col.gameObject.tag == "Platform")
		{
			grounded = false;
		}
	}
}


//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++[>+>+>+>+>+>+>+>+>+>+>+>+<<<<<<<<<<<<-]>++++++++.>+++++.>++++++++++++.>++++++++++++.>+++++++++++++++.>.>+++++++++++++++++++++++.>+++++++++++++++.>++++++++++++++++++.>++++++++++++.>++++.>.