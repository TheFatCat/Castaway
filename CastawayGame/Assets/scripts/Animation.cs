using UnityEngine;
using System.Collections;
[System.Serializable]
public class Animation  {
	
	public int startX; // starting values for animation
	public int endX;
	public int y; // y is expected to not change for each animation
	public float timePerFrame = 1f; // time between each frame
	
}
