using UnityEngine;
using System.Collections;

public class SynchronizedAnimate : MonoBehaviour {
	public float animationdelay = 0.2f;
	public float timer = 0.0f;
	public float tilesizeX = 300.0f;
	public float tilesizeY = 60.0f;
	public float offsetX = 0.0f;
	public float offsetY = 0.0f;
	public bool isEnabled = true;
	public bool oneshot = false;
	
	
	void Update ()
	{
		if (isEnabled) {
			timer += Time.deltaTime;
			if (timer >= animationdelay) {
				timer = 0.0f;
				//add one step
				offsetX += tilesizeX;
				if (offsetX >= 1.0f) {
					//we exceeded bounds, reset and move down
					offsetX = 0.0f;
					offsetY -= tilesizeY;
					if (offsetY <= 0.0f) {
						//we exceeded bounds, if we are oneshot
						if (oneshot) {
							isEnabled = false;
							offsetX = 1.0f-tilesizeX;
							offsetY = 0.0f;
						}
					}

				}
			}
		}
		renderer.material.SetTextureOffset ("_MainTex", new Vector2 (offsetX, offsetY));
		
		
	}

	public void SetEnabled(bool state){
		isEnabled = state;
	}

	public void SetOneShot (bool state)
	{
		oneshot = state;
	}
}
