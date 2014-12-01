using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	private static LevelLoader levelLoader;

	public static LevelData currentLevelData;

	public static Vector2 cachedPlayerPosition = new Vector2(0,0);
	public static Vector2 cachedPlayerVelocity = new Vector2(0,0);

	void Start () {
		// TODO set player start pos in first level
		Debug.Log("initialising game");
		Object[] staticObjects = GameObject.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject o in staticObjects) {
			DontDestroyOnLoad(o);
			LevelLoader ll = o.GetComponent<LevelLoader>();
			Debug.Log(ll);
			if (ll) {
				levelLoader = ll;
			}
		}
		levelLoader.loadLevel("level7");
	}

	// true if level can be changed, false if died
	public static bool playerChangeLevel (int dir, Vector2 pos, Vector2 vel) {
		switch (dir) {
		case LevelData.RIGHT:
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

}
