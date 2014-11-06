using UnityEngine;
using System.Collections;

public class GameData {

	// Defining level data like this
	private static LevelData[] levelData = new LevelData[] {

		new LevelData ()
			.set ("name", "level1")
			.set ("right", "level2")
			.set ("left", "level2")
			.set ("top", "level2")
			.set ("bottom", "level2"),
		new LevelData ()
			.set ("name", "level2")
			.set ("left", "level1")
			.set ("bottom", "level1")

	};

	public static LevelData getLevelData(string name) {
		foreach (LevelData ld in levelData) {
			if (ld.name == name) {
				return ld;
			}
		}
		Debug.LogError("COULDN'T FIND LEVEL CALLED: " + name);
		return levelData[0];
	}
}
