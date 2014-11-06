using UnityEngine;
using System.Collections;

public class LevelData {

	public static string NO_LEVEL = "";

	public string name = "main";
	public float offsetX = 0;
	public float offsetY = 0;
	public string left = NO_LEVEL;
	public string right = NO_LEVEL;
	public string top = NO_LEVEL;
	public string bottom = NO_LEVEL;

	public LevelData set (string prop, object val) {
		Debug.Log("setting " + prop + " to " + val);
		this.GetType().GetField(prop).SetValue(this, val);
		return this;
	}
}
