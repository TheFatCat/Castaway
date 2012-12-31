using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CharacterController))]
[RequireComponent(typeof (PlayerSpriteAnimate))]
[RequireComponent(typeof (WeaponImplementer))]
[RequireComponent(typeof (Status))]
public class PlayerController : MonoBehaviour {
//script to take user input and transform into movement
private float zPosition = 0.0f;
public double speed = 1.0;
public double minMoveDistance = 0.005;
public double acceleration = 0.5;
public double inAirAccel = 0.3;
public double jumpSpeed = 10;
public double jumpModifier = 10;	
public double crouchSpeed = 5.0;
public double gravity = 10.0;
public bool  grounded = false;
bool  crouching = false;
bool  jumping = false;
bool  canThrow = true;
bool  throwing = false;
bool  canShoot = true; 
bool  shooting = false;
public double shotLength = 0.7;
private double shootTimer= 0.0;
private Vector3 muzzleLocation = Vector3.zero;	//where the bullets will be spawned
private Vector3 muzzleDirection = Vector3.left;	//what direction bullets will fly
public Transform overlay;
CharacterController controller;	
//collision flags from controller
private CollisionFlags collisionFlags; 


public Vector3 moveDirection = Vector3.zero;

void  Start (){
	zPosition = transform.position.z;
	controller = GetComponent<CharacterController>();
}

int  getDirectionY (){ //returns a value, 1 if facing up, -1 if facing down, 0 if facing left or right
	double y = Input.GetAxisRaw("Vertical");	
	if(y > 0.25){
		return 1;
	}
	else if(y < -0.25){
	 	return -1;
	}
	else{	
		return 0 ; 
	}
}

int  getDirectionX (){//returns a value, 1 if facing right, -1 if facing left
	if(transform.localScale.x > 0){
		return 1;
	}
	else{
		return -1;	
	}
}

// Update is called once per frame
void  Update ()
	{
		//hold our z value to center player
		transform.position  = new Vector3(transform.position.x, transform.position.y, zPosition);
		//get inputs
		double h = Input.GetAxisRaw ("Horizontal");
		double v = Input.GetAxisRaw ("Vertical");
	
		PlayerSpriteAnimate overAnimator = overlay.GetComponent<PlayerSpriteAnimate> ();
		PlayerSpriteAnimate animator = GetComponent<PlayerSpriteAnimate> ();
	
		grounded = IsGrounded();
		if (grounded) {	//on ground controlls---------------

			//smooth out the horizontal vector
			if (crouching) {
			
				//slower move speed
				moveDirection.x = Mathf.Lerp ((float)moveDirection.x, (float)(h * crouchSpeed), (float)acceleration);
				//shorter controller
				if(controller.height != 4.36f){
					controller.center = new Vector3 (0, 0, 1.78f);
					controller.height = 4.36f;
				}
				//crouching
				muzzleLocation = new Vector3 (-3.2f, -0.55f, 0);
				muzzleDirection = new Vector3 ((transform.localScale.x / Mathf.Abs (transform.localScale.x)), 0, 0);
			
			} else {
				moveDirection.x = Mathf.Lerp ((float)moveDirection.x, (float)(h * speed), (float)acceleration);
				//regular controller
				if(controller.height != 6.92f){
					controller.center = new Vector3(0,0,0);
					controller.height = 6.92f;
				}
				if (v > 0.5f) {//looking up
					muzzleLocation = new Vector3 (-0.101f, 3.63f);
					muzzleDirection = Vector3.up;
				
				} else {//looking left
					muzzleLocation = new Vector3 (-3, 1.7082f, 0);
					muzzleDirection = new Vector3 ((transform.localScale.x / Mathf.Abs (transform.localScale.x)), 0, 0);
				}
			}
			if (h > 0.5f) {//moving right
				transform.localScale = new Vector3 (0.6f, 1, 0.73f);
			} else if (h < -0.5f) {//moving left
				transform.localScale = new Vector3 (-0.6f, 1, 0.73f);
			}
		
		
		
		
		
			//animation section....
			if (!shooting && !throwing) {
				//check orientation
				if (h != 0) {//is moving
		
					//are we looking up?
					if (v > 0.5f) {//looking up
						crouching = false;
						animator.SetAnimation (PlayerSpriteAnimate.animationType.MoveUp);
						animator.SetFallback (PlayerSpriteAnimate.animationType.IdleUp);
					} else if (v < -0.5f) {//crouching
						if (crouching == false) {
							//temporarily stop movement
							moveDirection.x = 0;
						}
						animator.SetAnimation (PlayerSpriteAnimate.animationType.CrouchMove);
						crouching = true;
					} else {	//looking forward
						crouching = false;
						animator.SetAnimation (PlayerSpriteAnimate.animationType.MoveLeft);
						animator.SetFallback (PlayerSpriteAnimate.animationType.IdleLeft);
					}
			
				} else {//is idle
					//are we looking up?
					if (v > 0.5f) {//looking up
						crouching = false;
						animator.SetAnimation (PlayerSpriteAnimate.animationType.IdleUp);
						animator.SetFallback (PlayerSpriteAnimate.animationType.IdleUp);
					} else if (v < -0.5f) {//crouching
						if (crouching == false) {
							//temporarily stop movement
							moveDirection.x = 0;
						}
						animator.SetAnimation (PlayerSpriteAnimate.animationType.CrouchLeft);
						crouching = true;
					} else {	//looking forward
						crouching = false;
						animator.SetAnimation (PlayerSpriteAnimate.animationType.IdleLeft);
						animator.SetFallback (PlayerSpriteAnimate.animationType.IdleLeft);
					}
				}
			}
		
			//we are on the ground, so check if we can jump
			if (Input.GetButtonDown ("Jump")) {
				jumping = true;
				shooting = false;
				shootTimer = 0.0f;
				//we are jumping
				moveDirection.y = (float)jumpSpeed;
				grounded = false;
				//nimation
			
				if (v > 0.5f) {//up
					animator.SetAnimation (PlayerSpriteAnimate.animationType.JumpUp);
				} else if (v < -0.5f) {//down
					animator.SetAnimation (PlayerSpriteAnimate.animationType.JumpDown);
				} else {//left
					animator.SetAnimation (PlayerSpriteAnimate.animationType.JumpLeft);
				}
			
				
			}
			//if we just landed
			if (jumping && grounded) {
				jumping = false;
				shooting = false;
				throwing = false;
				shootTimer = 0.0f;
				moveDirection.y = 0;
			}
			//if we just fell
			if (!jumping && !grounded) {
				jumping = true;
			}
		
		
		} else {//in air controls-----------------------------
			//smooth out the horizontal vector
			moveDirection.x = Mathf.Lerp ((float)moveDirection.x, (float)(h * speed), (float)inAirAccel);
			moveDirection.y -= (float)(gravity * Time.deltaTime);
			if(Input.GetButton("Jump") && moveDirection.y >0 ){
				moveDirection.y += (float)(jumpModifier * Time.deltaTime);	
			}
		
			if (v > 0.5f) {//up
				muzzleDirection = Vector3.up;
				muzzleLocation = new Vector3 (-0.101f, 3.63f, 0);
			} else if (v < -0.5f) {//down
				muzzleDirection = -Vector3.up;
				muzzleLocation = new Vector3 (0.404f, -2.037f, 0);
			} else {//left
				muzzleLocation = new Vector3 (-3, 1.7082f, 0);
				muzzleDirection = new Vector3 ((transform.localScale.x / Mathf.Abs (transform.localScale.x)), 0, 0);
			}
		
			//animation section....
			if (!shooting && !throwing) {
				//check orientation
				if (h > 0.5f) {//moving right
					transform.localScale = new Vector3 (0.6f, 1, 0.73f);
				} else if (h < -0.5f) {//moving left
					transform.localScale = new Vector3 (-0.6f, 1, 0.73f);
				}
				//nimation
				int frame = 0;
				if (v > 0.5f) {//up
					frame = animator.GetFrame ();
					animator.SetAnimation (PlayerSpriteAnimate.animationType.JumpUp);
					animator.SetFrame (frame);
				} else if (v < -0.5f) {//down
					frame = animator.GetFrame ();
					animator.SetAnimation (PlayerSpriteAnimate.animationType.JumpDown);
					animator.SetFrame (frame);
				} else {//left
					frame = animator.GetFrame ();
					animator.SetAnimation (PlayerSpriteAnimate.animationType.JumpLeft);
					animator.SetFrame (frame);
				}
			}

		}
	
	
		//test for secondary fire
		if (Input.GetButtonDown ("Fire2") && canThrow) {
			throwing = true;
			shooting = false;
			shootTimer = 0.0f;//quicker than shoot
		}
	
		if (throwing) {	//we are throwing
			if (grounded) {//on the ground
				if (v < -0.5f) {//crouching
					animator.SetAnimation (PlayerSpriteAnimate.animationType.ThrowCrouch);
					moveDirection.x = 0;	//cant move while crouching and throwing
				} else {//left
					if (h != 0) {//moving
						animator.SetAnimation (PlayerSpriteAnimate.animationType.ThrowMove);
					} else {
						animator.SetAnimation (PlayerSpriteAnimate.animationType.ThrowIdle);
					}
				}
		
			} else {	//in the air
				animator.SetAnimation (PlayerSpriteAnimate.animationType.ThrowFall);
			}
		
			if (shootTimer == 0.0f) {//cut to start of animation
				animator.SetFrameMin ();
			}
			shootTimer += Time.deltaTime;
			if (shootTimer >= shotLength - 0.12f) {
				throwing = false;
				shootTimer = 0.0f;
			}
		}
	
		if (!shooting && overlay.renderer.enabled) {	//we just finished shooting while running
		
			animator.SetAnimation (PlayerSpriteAnimate.animationType.MoveLeft);
			overlay.renderer.enabled = false;
			
		}
		if (shooting) {
			//we want to fire
			if (grounded) {//we are on the ground
				if (h != 0) {//we are moving =  SPECIAL CASE
					if (v > 0.5f) {//we are looking up
						overlay.renderer.enabled = true;
						animator.SetAnimation (PlayerSpriteAnimate.animationType.ShootMoveUp);
						crouching = false;
					
					} else if (v < -0.5f) {//crouching
						//don't allow to crouch fire and move
						moveDirection.x = 0;
						animator.SetAnimation (PlayerSpriteAnimate.animationType.ShootCrouch);
						animator.SetFallback (PlayerSpriteAnimate.animationType.CrouchLeft);
						overlay.renderer.enabled = false;
						crouching = true;
					} else {//left
						overlay.renderer.enabled = true;
						animator.SetAnimation (PlayerSpriteAnimate.animationType.ShootMoveLeft);
						crouching = false;
					}
				} else {//we are idle
					overlay.renderer.enabled = false;	
					if (v > 0.5f) {//we are looking up
						animator.SetAnimation (PlayerSpriteAnimate.animationType.ShootUp);
						animator.SetFallback (PlayerSpriteAnimate.animationType.CrouchLeft);
						crouching = false;
					} else if (v < -0.5f) {
						animator.SetAnimation (PlayerSpriteAnimate.animationType.ShootCrouch);
						animator.SetFallback (PlayerSpriteAnimate.animationType.CrouchLeft);
						crouching = true;
					} else {//left
						animator.SetAnimation (PlayerSpriteAnimate.animationType.ShootLeft);
						animator.SetFallback (PlayerSpriteAnimate.animationType.CrouchLeft);
						crouching = false;
					}
				}
			
			} else {//we are in the air
				crouching = false;
				overlay.renderer.enabled = false;
				if (v > 0.5f) {//we are looking up
					animator.SetAnimation (PlayerSpriteAnimate.animationType.ShootFallUp);
				} else if (v < -0.5f) {//down
					animator.SetAnimation (PlayerSpriteAnimate.animationType.ShootFallDown);
				} else {//left
					animator.SetAnimation (PlayerSpriteAnimate.animationType.ShootFallLeft);
				}
			}
			if (shootTimer == 0.0f) {//cut to start of animation
				animator.SetFrameMin ();
			}
			shootTimer += Time.deltaTime;
			if (shootTimer >= shotLength) {
				shooting = false;
				shootTimer = 0.0f;
			}
		}

	
		controller.Move(moveDirection * Time.deltaTime);
	
		
	
}

public void  fire (){
		if (canShoot) {
			WeaponImplementer weapon = GetComponent<WeaponImplementer> ();
			shooting = true;
			throwing = false;
			shootTimer = 0.0f;
			weapon.fire (muzzleLocation, muzzleDirection);
		}
}

//allow us to shoot
public void  SetCanShoot ( bool yesno  ){
	 canShoot = yesno;
}

//allow us to throw
public void  SetCanThrow ( bool yesno  ){
	 canThrow = yesno;
}

public bool  IsGrounded (){
	return controller.isGrounded;
}
}
