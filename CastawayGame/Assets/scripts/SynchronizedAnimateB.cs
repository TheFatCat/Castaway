using UnityEngine;
using System.Collections;

public class SynchronizedAnimateB : MonoBehaviour {

	public int startX;
	public int startY;
	public int endX;
	public int endY;
	
	public float offsetX; // starting offset of the texture
	public float offsetY; 
	public float tileX; // tile size of each frame
	public float tileY;
	
	
	public int xDimension; // the maximum frames for both x and y
	public int yDimension;
	public float timePerFrame;
	float timer  = 0f;
	
	
	int currentX;
	int currentY;
	public bool playing = false;
	public bool loop = false;
	
	void Start(){
		currentX = startX;
		currentY = startY;
	}
	// Update is called once per frame
	void Update () {
		if(playing){
			timer += Time.deltaTime;
			if(timer > timePerFrame){
				timer = 0;
				currentX ++;
				if (currentY  == endY && currentX  > endX){
					if(loop){
						reset();
					}
					else{
						goToEnd();
					}
					
				}
				
				else if( (currentX + 1) > xDimension){
					currentX = 0;
					currentY ++;
				}
				float x = (float)offsetX + currentX * (float)tileX;
			
				float y = (float) offsetY - currentY * (float)tileY - (float)  tileY ;
				renderer.material.mainTextureOffset = new Vector2(x,y) ;
			}
		}
	}
	
	
	public void start(){
		playing = true;
	}
	
	public void stop(){
		playing = false;
	}
				
	public void reset(){
		currentX = startX;
		currentY = startY;
		timer = 0;
	}
	
	public void goToEnd(){
		currentX = endX;
		currentY = endY;
		timer = 0;
	}
}
