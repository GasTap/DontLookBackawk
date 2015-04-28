using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour {

	// TODO move this to global gamestate, or move global gamestate here
	private Vector2 lastExitPosition;
	private Vector2 lastPos;
	private Vector2 lastVelocity;
	private int lastExitDirection = LevelData.NONE;

	private void _loadLevel (string name, bool spawnAtPos) {		
		StartCoroutine(LoadLevelCoroutine(name, spawnAtPos));
	}

	public void loadLevel (string name) {
		_loadLevel(name, false);
	}

	public void loadLevel (GameObject exit, Vector2 nPos, Vector2 velocity, int exitDirection) {
		GlobalGameState.playerPreviousLevel = GlobalGameState.playerCurrentLevel;
		lastExitPosition = exit.transform.position;
		lastPos = nPos;
		lastVelocity = velocity;
		lastExitDirection = exitDirection;
		GlobalGameState.playerEntrancePosition = nPos;
		_loadLevel(exit.GetComponent<exit>().level, false);
	}

	public void reloadLevel () {
		_loadLevel(GlobalGameState.playerCurrentLevel, true);
	}

	private PlayerInputSystem pis;
	void Start () {
		pis = GameObject.Find("PlayerInput").GetComponent<PlayerInputSystem>();
	}

	Vector2 initialPlayerPos;
	void Update () {
		if (loadingLevel) { return; }
		var player = pis.controlledActor;
		if (player == null) { return; }
		bool changed = false;
		var pos = player.transform.position;
		initialPlayerPos = new Vector2(pos.x, pos.y);
		if (pos.x > GameController.xBound) {
			changed = GameController.playerChangeLevel(LevelData.RIGHT, new Vector2(-GameController.xBound, pos.y), player.GetComponent<Rigidbody2D>().velocity);
			initialPlayerPos.x = changed ? -GameController.xBound : GameController.xBound;
		} else if (pos.x < -GameController.xBound) {
			changed = GameController.playerChangeLevel(LevelData.LEFT, new Vector2(GameController.xBound, pos.y), player.GetComponent<Rigidbody2D>().velocity);
			initialPlayerPos.x = changed ? GameController.xBound : -GameController.xBound;
		} else if (pos.y > GameController.yBound) {
			changed = GameController.playerChangeLevel(LevelData.TOP, new Vector2(pos.x, -GameController.yBound), player.GetComponent<Rigidbody2D>().velocity);
			initialPlayerPos.y = changed ? -GameController.yBound : GameController.yBound;
		} else if (pos.y < -GameController.yBound) {
			changed = GameController.playerChangeLevel(LevelData.BOTTOM, new Vector2(pos.x, GameController.yBound), player.GetComponent<Rigidbody2D>().velocity);
			initialPlayerPos.y = changed ? GameController.yBound : -GameController.yBound;
		}
		//player.SendMessage("disablePhysics");
	}

	void OnLevelLoad (List<GameObject> destroyed, bool spawnAtPos) {
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

		lastPos.x = initialPlayerPos.x;
		lastPos.y = initialPlayerPos.y;

		if (lastExitDirection == LevelData.RIGHT) {
			if (ld.leftSpawn) { 
				if (!spawnAtPos) {
					lastPos.y += ld.leftSpawn.transform.position.y - lastExitPosition.y;
				} else {
					lastPos.x = ld.leftSpawn.transform.position.x;
					lastPos.y = ld.leftSpawn.transform.position.y;
				}
			}
		} else if (lastExitDirection == LevelData.LEFT) {
			if (ld.rightSpawn) {
				if (!spawnAtPos) {
					lastPos.y += ld.rightSpawn.transform.position.y - lastExitPosition.y;
				} else {
					lastPos.x = ld.leftSpawn.transform.position.x;
					lastPos.y = ld.leftSpawn.transform.position.y;
				}
			}
		} else if (lastExitDirection == LevelData.TOP) {
			if (ld.bottomSpawn) {
				if (!spawnAtPos) {
					lastPos.x += ld.bottomSpawn.transform.position.x - lastExitPosition.x;
				} else {
					lastPos.x = ld.leftSpawn.transform.position.x;
					lastPos.y = ld.leftSpawn.transform.position.y;
				}
			}
		} else if (lastExitDirection == LevelData.BOTTOM) {
			if (ld.topSpawn) {
				if (!spawnAtPos) {
					lastPos.x += ld.topSpawn.transform.position.x - lastExitPosition.x;
				} else {
					lastPos.x = ld.leftSpawn.transform.position.x;
					lastPos.y = ld.leftSpawn.transform.position.y;
				}
			}
		}

		GameController.currentLevelData = ld;

		if (GameController.hideDebugObjects) {
			hideDebugObjects();
		}

		var player = pis.controlledActor;
		if (player != null) {
			//player.SendMessage("enablePhysics");
			player.transform.position = lastPos;
			player.GetComponent<Rigidbody2D>().velocity = lastVelocity;
			player.SendMessage("OnLevelLoad", onStage);
		}
	}

	private void hideDebugObjects () {
		var stuff = new List<GameObject>();
		stuff.AddRange(GameObject.FindGameObjectsWithTag("Platform"));
		stuff.AddRange(GameObject.FindGameObjectsWithTag("OWPlatform"));
		stuff.AddRange(GameObject.FindGameObjectsWithTag("Spawn"));
		stuff.AddRange(GameObject.FindGameObjectsWithTag("Exit"));
		stuff.AddRange(GameObject.FindGameObjectsWithTag("Death"));
		
		foreach (GameObject i in stuff) {
			i.GetComponent<Renderer>().enabled = false;
		}
	}

	private bool loadingLevel = false;
	IEnumerator LoadLevelCoroutine(string levelToLoad, bool spawnAtPos) {

		if (loadingLevel) {
			return false;
		}
		loadingLevel = true;

		var oldGameObjects = (Transform[])GameObject.FindObjectsOfType<Transform>();

		yield return Application.LoadLevelAdditiveAsync(levelToLoad);

		Destroy(GameObject.Find("MainSceneOverlay"));

		GlobalGameState.playerCurrentLevel = levelToLoad;

		var player = pis.controlledActor;
		List<GameObject> destroyed = new List<GameObject>();
		for (int i = 0 ; i < oldGameObjects.Length; i++) {
			var t = oldGameObjects[i];
			if (t && t.gameObject != player && !GameController.globalObjects.Contains(t.gameObject)) {
				Debug.Log ("deleting " + t.gameObject.name);
				destroyed.Add(t.gameObject);
				Destroy (t.gameObject);
			}
		}

		OnLevelLoad(destroyed, spawnAtPos);
		loadingLevel = false;
	}
}
