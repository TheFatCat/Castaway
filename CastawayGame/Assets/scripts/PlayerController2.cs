using UnityEngine;
using System.Collections;

public class PlayerController2 : MonoBehaviour {
	CollisionFlags collisionFlags;
	CharacterController controller;
	float currentMaxSpeed;
	public float maxGroundSpeed = 10;
	public float maxCrouchSpeed = 7;
	public float maxAirSpeed = 8;
	public float jumpSpeed = 5;
	public float groundAcceleration = 1;
	public float crouchAcceleration = 1;
	public float airAcceleration = 1;
	public float gravityAcceleration = 9.8f;
	float currentYSpeed = 0;
	float currentXSpeed = 0 ;
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		
		
		float horizontalInput = Input.GetAxisRaw("Horizontal");
		float verticalInput = Input.GetAxisRaw("Vertical");
		
		//for jumping and falling
		collisionFlags = controller.Move(new Vector3(0,-1 * currentYSpeed,0)  *Time.deltaTime);
		Debug.Log("is grounded " + IsGrounded());
		
		//in the air
		if(! IsGrounded()){
			currentXSpeed = Mathf.Lerp(currentXSpeed, horizontalInput * maxAirSpeed, Time.time * airAcceleration);
			//accelerate downward
			currentYSpeed += gravityAcceleration * Time.deltaTime;
			//if you hold jump while in the air you will jump higher
			if(Input.GetButton("Jump") && currentYSpeed < 0 ){
				
				currentYSpeed -= (gravityAcceleration / 2f) * Time.deltaTime;
			}	
		}

		else{
			//fall speed should never be exacly 0 or is grounded doesnt work
			currentYSpeed = 0.1f;
			currentXSpeed = Mathf.Lerp(currentXSpeed, horizontalInput * maxAirSpeed, Time.time * groundAcceleration );

			if(Input.GetButton("Jump")){
				Debug.Log("Jumped");
				currentYSpeed -= jumpSpeed;
			}
			
		}
		
		//move from controller input
		collisionFlags = controller.Move(new Vector3(currentXSpeed,0,0)  *Time.deltaTime);
		

	
	}
	
	public bool  IsGrounded (){
		return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
	}
}
