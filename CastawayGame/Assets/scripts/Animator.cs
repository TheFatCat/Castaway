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
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceLastFrame += Time.deltaTime;
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
	
	
	public void setAnimation(Animation newAnimation, bool loop){
		currentAnimation = newAnimation;
		this.loop = loop;
		
		//if(loop && currentY == newAnimation.y   && currentX >= newAnimation.startX  && currentX <= newAnimation.endX){
			currentX = newAnimation.startX;
			currentY = newAnimation.y;
		//}
		
		
		
	}
}
