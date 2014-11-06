using UnityEngine;
using System.Collections;

public class LevelSpawner {

	public static LevelData loadLevel (string name, Vector2 nPos, Vector2 velocity) {
		LevelData old = GameController.currentLevelData;
		LevelData ld = GameData.getLevelData(name);
		if (old != null){
			nPos.x -= old.offsetX;
			nPos.y -= old.offsetY;
		}
		nPos.x += ld.offsetX;
		nPos.y += ld.offsetY;

		GameController.cachedPlayerPosition = nPos;
		GameController.cachedPlayerVelocity = velocity;

		Debug.Log("Loading level: " + ld.name);
		Application.LoadLevel(ld.name);

		return ld;
	}
}
