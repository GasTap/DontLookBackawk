using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public static bool hideDebugObjects = false;

	private static LevelLoader levelLoader;

	public static LevelData currentLevelData;

	public static List<GameObject> globalObjects;
	
	public static float xBound = 7f;
	public static float yBound = (7 * 3 / 4f);

	public Transform FadeOut;

	public List<GameObject> doNotKeep;

	// TODO need to reset stuff if they start again after finishing the game
	public void initializeGame (float delay) {
		StartCoroutine(_initializeGame(delay));
	}

	private IEnumerator _initializeGame (float delay) {
		var f = Instantiate(FadeOut);
		f.transform.position = new Vector3(0,0,-1);
		f.GetComponent<fadein>().duration = delay;

		Random.seed = 1234;
		
		yield return new WaitForSeconds(delay / 1000);
		
		Debug.Log("initialising game");
		Object[] staticObjects = GameObject.FindObjectsOfType(typeof(GameObject));
		globalObjects = new List<GameObject>();
		// TODO potentially don't have individual scenes for screens
		foreach (GameObject o in staticObjects) {
			if (o == f.gameObject || doNotKeep.Contains(o)) { continue; }
			globalObjects.Add(o);
			//DontDestroyOnLoad(o);
			LevelLoader ll = o.GetComponent<LevelLoader>();
			if (ll) {
				levelLoader = ll;
			}
		}
		levelLoader.loadLevel("level1");
	}

	void Start () {}

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

	public static void reloadLevel () {
		levelLoader.reloadLevel();
	}

}
