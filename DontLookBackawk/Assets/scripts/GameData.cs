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
			.set ("offsetY", 4.3882f)
			.set ("left", "level3")
			.set ("right", "level5"),
		new LevelData ()
			.set ("name", "level5")
			.set ("offsetY", 1.42216f)
			.set ("left", "level4")
			.set ("right", "level6"),
		new LevelData ()
			.set ("name", "level6")
			.set ("left", "level5")
			.set ("right", "level7"),
		new LevelData ()
			.set ("name", "level7")
			.set ("left", "level6")
			.set ("right", "level8")

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
