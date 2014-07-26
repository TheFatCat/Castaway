using UnityEngine;
using System.Collections;

public class PlayerController2D : MonoBehaviour {
	
	private Vector3 moveDirection = Vector3.zero;
	private CharacterController cc;
	
	public float speed = 1.0f;
	public float accel = 5.0f; 
	public float gravity = 20.0f;
	public float jumpSpeed = 20.0f;
	public bool grounded = false;
	// Use this for initialization
	void Start () {
		//cache the character controller for later use
		cc = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		//cache inputs
		float h = Input.GetAxisRaw("Horizontal");
		//float v = Input.GetAxisRaw("Vertical");
		
		if(h<0){
			transform.localScale = new Vector3(-1.0f,1.0f,1.0f);
			//transform.localScale.Set(-1.0f,1.0f,1.0f);
		}else if(h>0){
			transform.localScale = new Vector3(1.0f,1.0f,1.0f);
		}
		
		//check for ground touching
		if ((cc.collisionFlags & CollisionFlags.CollidedBelow) > 0){
			grounded = true;
		}else{
			grounded = false;
			if((cc.collisionFlags & CollisionFlags.Above) > 0 && moveDirection.y > 0.0f){	//we hit our head
				//kill vert velocity
				moveDirection.y = 0.0f;
			
			}
		}
		
		//calculate interpolated velocity
		moveDirection = Vector3.Lerp(moveDirection,new Vector3(h * speed,moveDirection.y,0.0f),accel * Time.deltaTime);
		moveDirection.y -= gravity * Time.deltaTime;
		
		//if on ground or hit head, kill vert. velocity
		//|| cc.collisionFlags == CollisionFlags.Above
		if(grounded ){
			moveDirection.y = -1.0f;
		}
		
		if(Input.GetButtonDown("Jump") && grounded){
			moveDirection.y += jumpSpeed;
		}
		
		cc.Move(moveDirection * Time.deltaTime);	//move
		transform.position = new Vector3(transform.position.x,transform.position.y,0.0f);
		
	}
}
