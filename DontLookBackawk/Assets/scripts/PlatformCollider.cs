using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformCollider : MonoBehaviour {
	
	public bool grounded = false;
	private List<GameObject> oWPlatformers = new List<GameObject>();

	void OnLevelWasLoaded(int level)
	{
		oWPlatformers = new List<GameObject>();
		oWPlatformers.AddRange(GameObject.FindGameObjectsWithTag("OWPlatform"));
	}

	void Update()
	{
		foreach(var p in oWPlatformers)
		{
			if(p.transform.position.y > transform.position.y - 0.5f)
			{
				p.GetComponent<Collider2D>().enabled = false;
			}
			else
			{
				p.GetComponent<Collider2D>().enabled = true;
			}
		}
	}


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

		if(col.gameObject.tag == "OWPlatform")
		{
			RaycastHit2D ray = Physics2D.Raycast(transform.position - Vector3.up * 0.6f, -Vector2.up, 0.1f);
			if (ray.collider != null && ray.collider.gameObject.tag == "OWPlatform") 
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
		else if(col.gameObject.tag == "OWPlatform")
		{
			grounded = false;
		}
	}
}


//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++[>+>+>+>+>+>+>+>+>+>+>+>+<<<<<<<<<<<<-]>++++++++.>+++++.>++++++++++++.>++++++++++++.>+++++++++++++++.>.>+++++++++++++++++++++++.>+++++++++++++++.>++++++++++++++++++.>++++++++++++.>++++.>.