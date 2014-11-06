using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static LevelData currentLevelData;

	public static Vector2 cachedPlayerPosition = new Vector2(0,0);
	public static Vector2 cachedPlayerVelocity = new Vector2(0,0);

	void Start () {
		// TODO set player start pos in first level
		Debug.Log("initialising game");
		Object[] staticObjects = GameObject.FindObjectsOfType(typeof(MonoBehaviour));
		foreach (Object o in staticObjects) {
			DontDestroyOnLoad(o);
		}
		currentLevelData = LevelSpawner.loadLevel("level1", cachedPlayerPosition, cachedPlayerVelocity);
	}

	public const int TOP = 0;
	public const int BOTTOM = 1;
	public const int LEFT = 2;
	public const int RIGHT = 3;

	// true if level can be changed, false if died
	public static bool playerChangeLevel (int dir, Vector2 pos, Vector2 vel) {
		switch (dir) {
		case RIGHT:
			if (currentLevelData.right != LevelData.NO_LEVEL) {
				currentLevelData = LevelSpawner.loadLevel(currentLevelData.right, pos, vel);
				return true;
			}
			break;
		case LEFT:
			if (currentLevelData.left != LevelData.NO_LEVEL) {
				currentLevelData = LevelSpawner.loadLevel(currentLevelData.left, pos, vel);
				return true;
			}
			break;
		case TOP:
			if (currentLevelData.top != LevelData.NO_LEVEL) {
				currentLevelData = LevelSpawner.loadLevel(currentLevelData.top, pos, vel);
				return true;
			}
			break;
		case BOTTOM:
			if (currentLevelData.bottom != LevelData.NO_LEVEL) {
				currentLevelData = LevelSpawner.loadLevel(currentLevelData.bottom, pos, vel);
				return true;
			}
			break;
		}
		return false;
	}

}
