using UnityEngine;
using System.Collections;

public class LevelData {

	public const int NONE = -1;
	public const int TOP = 0;
	public const int BOTTOM = 1;
	public const int LEFT = 2;
	public const int RIGHT = 3;

	public static GameObject NO_LEVEL = null;

	public string name = "main";

	public GameObject leftSpawn = NO_LEVEL;
	public GameObject leftExit = NO_LEVEL;

	public GameObject rightSpawn = NO_LEVEL;
	public GameObject rightExit = NO_LEVEL;

	public GameObject topSpawn = NO_LEVEL;
	public GameObject topExit = NO_LEVEL;

	public GameObject bottomSpawn = NO_LEVEL;
	public GameObject bottomExit = NO_LEVEL;
}
