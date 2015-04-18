using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	private Vector2 lastExitPosition;
	private Vector2 lastPos;
	private Vector2 lastVelocity;
	private int lastExitDirection = LevelData.NONE;

	public GameObject player;

	private void _loadLevel (string name) {
		Application.LoadLevel(name);
	}

	public void loadLevel (string name) {
		_loadLevel(name);
	}

	public void loadLevel (GameObject exit, Vector2 nPos, Vector2 velocity, int exitDirection) {
		lastExitPosition = exit.transform.position;
		lastPos = nPos;
		lastVelocity = velocity;
		lastExitDirection = exitDirection;
		_loadLevel(exit.GetComponent<exit>().level);
	}

	void OnLevelWasLoaded () {
		LevelData ld = new LevelData();
		
		Object[] objectsOnStage = GameObject.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject o in objectsOnStage) {
			switch (o.name) {
			case "LeftExit":
				ld.leftExit = o;
				break;
			case "RightExit":
				ld.rightExit = o;
				break;
			case "TopExit":
				ld.topExit = o;
				break;
			case "BottomExit":
				ld.bottomExit = o;
				break;
				
			case "LeftSpawn":
				ld.leftSpawn = o;	
				break;
			case "RightSpawn":
				ld.rightSpawn = o;
				break;
			case "TopSpawn":
				ld.topSpawn = o;
				break;
			case "BottomSpawn":
				ld.bottomSpawn = o;
				break;
			}
		}

		if (lastExitDirection == LevelData.RIGHT) {
			if (ld.leftSpawn) { 
				lastPos.y += ld.leftSpawn.transform.position.y - lastExitPosition.y;
			}
		} else if (lastExitDirection == LevelData.LEFT) {
			if (ld.rightSpawn) {
				lastPos.y += ld.rightSpawn.transform.position.y - lastExitPosition.y;
			}
		} else if (lastExitDirection == LevelData.TOP) {
			if (ld.bottomSpawn) {
				lastPos.x += ld.bottomSpawn.transform.position.x - lastExitPosition.x;
			}
		} else if (lastExitDirection == LevelData.BOTTOM) {
			if (ld.topSpawn) {
				lastPos.x += ld.topSpawn.transform.position.x - lastExitPosition.x;
			}
		}

		Debug.Log("caching pos");
		player.transform.position = lastPos;
		player.GetComponent<Rigidbody2D>().velocity = lastVelocity;

		GameController.currentLevelData = ld;
	}
}
