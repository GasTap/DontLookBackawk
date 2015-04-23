using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour {

	// TODO move this to global gamestate, or move global gamestate here
	private Vector2 lastExitPosition;
	private Vector2 lastPos;
	private Vector2 lastVelocity;
	private int lastExitDirection = LevelData.NONE;

	public GameObject player;

	private void _loadLevel (string name) {		
		StartCoroutine(LoadLevelCoroutine(name));
	}

	public void loadLevel (string name) {
		_loadLevel(name);
	}

	public void loadLevel (GameObject exit, Vector2 nPos, Vector2 velocity, int exitDirection) {
		GlobalGameState.playerPreviousLevel = GlobalGameState.playerCurrentLevel;
		lastExitPosition = exit.transform.position;
		lastPos = nPos;
		lastVelocity = velocity;
		lastExitDirection = exitDirection;
		GlobalGameState.playerEntrancePosition = nPos;
		_loadLevel(exit.GetComponent<exit>().level);
	}

	public void reloadLevel () {
		_loadLevel(GlobalGameState.playerCurrentLevel);
	}

	void OnLevelLoad (List<GameObject> destroyed) {
		LevelData ld = new LevelData();
		
		var objectsOnStage = (Transform[])GameObject.FindObjectsOfType<Transform>();
		List<GameObject> onStage = new List<GameObject>();

		foreach (Transform t in objectsOnStage) {
			if (destroyed.Contains(t.gameObject)) {
				// TODO this is retarded
				continue;
			}
			var o = t.gameObject;
			onStage.Add(o);
			Debug.Log(o + " ON STAGE");
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

		player.transform.position = lastPos;
		player.GetComponent<Rigidbody2D>().velocity = lastVelocity;

		GameController.currentLevelData = ld;

		GameObject.FindGameObjectWithTag("Player").SendMessage("OnLevelLoad", onStage);
	}

	IEnumerator LoadLevelCoroutine(string levelToLoad) {

		Debug.Log("loading " + levelToLoad);

		var oldGameObjects = (Transform[])GameObject.FindObjectsOfType<Transform>();

		yield return Application.LoadLevelAdditiveAsync(levelToLoad);

		Destroy(GameObject.Find("MainSceneOverlay"));

		GlobalGameState.playerCurrentLevel = levelToLoad;

		List<GameObject> destroyed = new List<GameObject>();
		for (int i = 0 ; i < oldGameObjects.Length; i++) {
			var t = oldGameObjects[i];
			if (t && !GameController.globalObjects.Contains(t.gameObject)) {
				Debug.Log ("deleting " + t.gameObject.name);
				destroyed.Add(t.gameObject);
				Destroy (t.gameObject);
			}
		}

		foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>()) {
			Debug.Log("here " + go.name);
		}

		OnLevelLoad(destroyed);
	}
}
