using UnityEngine;
using System.Collections;

public class GameData {

	// Defining level data like this
	private static LevelData[] levelData = new LevelData[] {

		new LevelData ()
			.set ("name", "level1")
			.set ("right", "level2"),
		new LevelData ()
			.set ("name", "level2")
			.set ("left", "level1")
			.set ("top", "level3"),
		new LevelData ()
			.set ("name", "level3")
			.set ("bottom", "level2")
			.set ("right", "level4")
			.set ("offsetX", -9.246f),
		new LevelData ()
			.set ("name", "level4")
			.set ("offsetY", 4.039929f)
			.set ("left", "level3")

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
