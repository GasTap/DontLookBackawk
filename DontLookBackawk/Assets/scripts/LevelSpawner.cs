using UnityEngine;
using System.Collections;

public class LevelSpawner {

	public static LevelData loadLevel (string name) {
		LevelData ld = GameData.getLevelData(name);
		Debug.Log("Loading level: " + ld.name);
		Application.LoadLevel(ld.name);
		return ld;
	}
}
