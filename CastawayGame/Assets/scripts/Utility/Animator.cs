using UnityEngine;
using System.Collections;

public class Animator : MonoBehaviour {
	[SerializeField] private Animation currentAnimation; // what is the current animation
	[SerializeField] private bool loop; // should the animation loop
	[SerializeField] private int currentX; // keeps track of current frames
	[SerializeField] private int currentY; //ditto
	[SerializeField] private double offsetX; // starting offset of image
	[SerializeField] private double offsetY; 
	[SerializeField] private double tileX; // tiling for current set of animations
	[SerializeField] private double tileY;
	private float timeSinceLastFrame = 0; // for keeping time with frames
	
	private bool flashingFrame = false; // if is flashing frame don't do other animations
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!flashingFrame){
			timeSinceLastFrame += Time.deltaTime;
		}
		if(timeSinceLastFrame > currentAnimation.timePerFrame){
			timeSinceLastFrame = 0;
			currentX ++;
			if(currentX > currentAnimation.endX){
				// if looping go back to the first frame
				if(loop){
					currentX = currentAnimation.startX;
				}
				
				// if not looping stay on the last frame
				else{
					currentX = currentAnimation.endX;
				}
			}
			
			// calculate actual offset for material
			//not guaranteed to work for all offsets yet
			float x = (float)offsetX + currentX * (float)tileX;
			
			float y = (float) offsetY - currentY * (float)tileY - (float)  tileY ;
			renderer.material.mainTextureOffset = new Vector2(x,y); 
		}
	}
	
	
	//start flash frame from a Animation
	public void flashFrame(Animation frame){
		flashingFrame = true;
		float x = (float)offsetX + frame.startX * (float)tileX;
			
		float y = (float) offsetY - frame.y * (float)tileY - (float)  tileY ;
		renderer.material.mainTextureOffset = new Vector2(x,y); 
	}
	
	// end the flashframe and set the renderer back to the normal offset
	public void endFlashFrame(){
		flashingFrame = false;
		float x = (float)offsetX + currentX * (float)tileX;
			
		float y = (float) offsetY - currentY * (float)tileY - (float)  tileY ;
		renderer.material.mainTextureOffset = new Vector2(x,y); 
	}
	public void setAnimation(Animation newAnimation, bool loop){
		currentAnimation = newAnimation;
		this.loop = loop;
		
		//if(loop && currentY == newAnimation.y   && currentX >= newAnimation.startX  && currentX <= newAnimation.endX){
			currentX = newAnimation.startX;
			currentY = newAnimation.y;
		//}
		
		
		
	}
}
