using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformCollider : MonoBehaviour {
	
	public bool grounded = false;
	private List<GameObject> oWPlatformers = new List<GameObject>();

	public string[] standables = {"Platform", "OWPlatform", "Egg"};

	void OnLevelLoad(List<GameObject> onStage)
	{
		oWPlatformers = new List<GameObject>();
		foreach (var go in onStage) {
			if (go.tag == "OWPlatform" && go.activeInHierarchy) {
				Debug.Log("adding " + go.name);
				oWPlatformers.Add(go);
			}
		}
	}

	PlayerInputSystem pis;
	void Start() {
		pis = GameObject.Find("PlayerInput").GetComponent<PlayerInputSystem>();
		var l = new List<GameObject>();
		l.AddRange(GameObject.FindObjectsOfType<GameObject>());
		OnLevelLoad(l);
	}

	void Update()
	{
		// TODO this means only the player can use one way platforms
		if (pis.controlledActor != this.gameObject) {
			return;
		}

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


	void OnCollisionStay2D(Collision2D col) {
		foreach (var tag in standables) {
			if(col.gameObject.tag == tag) {
				RaycastHit2D ray = Physics2D.Raycast(transform.position - Vector3.up * 0.6f, -Vector2.up, 0.1f);
				if (ray.collider != null && ray.collider.gameObject.tag == tag) {
					grounded = true;
				}
			}
		}

		if (col.gameObject.tag == "Death") {
			this.SendMessage("die");
		}
	}

	void OnCollisionExit2D(Collision2D col) {
		foreach (var tag in standables) {
			if(col.gameObject.tag == tag){
				grounded = false;
			}
		}
	}
}


//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++[>+>+>+>+>+>+>+>+>+>+>+>+<<<<<<<<<<<<-]>++++++++.>+++++.>++++++++++++.>++++++++++++.>+++++++++++++++.>.>+++++++++++++++++++++++.>+++++++++++++++.>++++++++++++++++++.>++++++++++++.>++++.>.