using UnityEngine;
using System.Collections;

public class Animator : MonoBehaviour {
	[SerializeField] private Animation currentAnimation;
	[SerializeField] private bool loop;
	[SerializeField] private int currentX;
	[SerializeField] private int currentY;
	[SerializeField] private double tileX;
	[SerializeField] private double tileY;
	private float timeSinceLastFrame = 0;
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
				if(loop){
					currentX = currentAnimation.startX;
				}
				else{
					currentX = currentAnimation.endX;
				}
			}
			
			
			float x = currentX * (float)tileX;
			
			float y = 1 - currentY * (float)tileY;
			renderer.material.mainTextureOffset = new Vector2(x,y); 
		}
	}
	
	
	public void setAnimation(Animation newAnimation, bool loop){
		currentAnimation = newAnimation;
		this.loop = loop;
		
		if(loop && currentY == newAnimation.y   && currentX >= newAnimation.startX  && currentX <= newAnimation.endX){
		
		}
		currentX = newAnimation.startX;
		currentY = newAnimation.y;
	}
}
