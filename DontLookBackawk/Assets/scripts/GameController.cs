using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	private static LevelData currentLevelData;

	void Start () {
		Debug.Log("initialising game");
		Object[] staticObjects = GameObject.FindObjectsOfType(typeof(MonoBehaviour));
		foreach (Object o in staticObjects) {
			DontDestroyOnLoad(o);
		}
		currentLevelData = LevelSpawner.loadLevel("level1");
	}

	public const int TOP = 0;
	public const int BOTTOM = 1;
	public const int LEFT = 2;
	public const int RIGHT = 3;

	// true if level can be changed, false if died
	public static bool playerChangeLevel (int dir) {

		switch (dir) {
		case RIGHT:
			if (currentLevelData.right != LevelData.NO_LEVEL) {
				currentLevelData = LevelSpawner.loadLevel(currentLevelData.right);
			}
			return true;
		case LEFT:
			if (currentLevelData.left != LevelData.NO_LEVEL) {
				currentLevelData = LevelSpawner.loadLevel(currentLevelData.left);
			}
			return true;
		case TOP:
			if (currentLevelData.top != LevelData.NO_LEVEL) {
				currentLevelData = LevelSpawner.loadLevel(currentLevelData.top);
			}
			return true;
		case BOTTOM:
			if (currentLevelData.bottom != LevelData.NO_LEVEL) {
				currentLevelData = LevelSpawner.loadLevel(currentLevelData.bottom);
			}
			return true;
		}
		return false;
	}

}
