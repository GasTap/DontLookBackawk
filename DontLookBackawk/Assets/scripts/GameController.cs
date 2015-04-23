using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	private static LevelLoader levelLoader;

	public static LevelData currentLevelData;

	public static List<GameObject> globalObjects;

	void Start () {
		Debug.Log("initialising game");
		Object[] staticObjects = GameObject.FindObjectsOfType(typeof(GameObject));
		globalObjects = new List<GameObject>();
		// TODO potentially don't have individual scenes for screens
		foreach (GameObject o in staticObjects) {
			globalObjects.Add(o);
			//DontDestroyOnLoad(o);
			LevelLoader ll = o.GetComponent<LevelLoader>();
			if (ll) {
				levelLoader = ll;
			}
		}
		levelLoader.loadLevel("level1");
	}

	// true if level can be changed, false if died
	public static bool playerChangeLevel (int dir, Vector2 pos, Vector2 vel) {
		switch (dir) {
		case LevelData.RIGHT:
			Debug.Log (currentLevelData.rightExit);
			if (currentLevelData.rightExit != LevelData.NO_LEVEL) {
				levelLoader.loadLevel(currentLevelData.rightExit, pos, vel, LevelData.RIGHT);
				return true;
			}
			break;
		case LevelData.LEFT:
			if (currentLevelData.leftExit != LevelData.NO_LEVEL) {
				levelLoader.loadLevel(currentLevelData.leftExit, pos, vel, LevelData.LEFT);
				return true;
			}
			break;
		case LevelData.TOP:
			if (currentLevelData.topExit != LevelData.NO_LEVEL) {
				levelLoader.loadLevel(currentLevelData.topExit, pos, vel, LevelData.TOP);
				return true;
			}
			break;
		case LevelData.BOTTOM:
			if (currentLevelData.bottomExit != LevelData.NO_LEVEL) {
				levelLoader.loadLevel(currentLevelData.bottomExit, pos, vel, LevelData.BOTTOM);
				return true;
			}
			break;
		}
		return false;
	}

	public static void reloadLevel () {
		levelLoader.reloadLevel();
	}

}
