using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CharacterController))]
[RequireComponent(typeof (PlayerSpriteAnimate))]
[RequireComponent(typeof (WeaponImplementer))]
[RequireComponent(typeof (PlayerStatus))]
public class PlayerController : MonoBehaviour {
//script to take user input and transform into movement
private static  Transform player ;	
public bool frozen = false;
public LayerMask mask = -1;
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
public bool  canThrow = true;
bool  throwing = false;
public bool hit = false;
public float hitTime = 0.2f;
private float hitTimer = 0.0f;
public bool  canShoot = true; 
bool  shooting = false;
public double shotLength = 0.7;
private double shootTimer= 0.0;
private Vector3 secondaryMuzzleLocation = Vector3.zero;	//where thrown objects will be spawned
private Vector3 secondaryMuzzleDirection = Vector3.zero;	//where thrown objects will fly
private Vector3 muzzleLocation = Vector3.zero;	//where the bullets will be spawned
private Vector3 muzzleDirection = Vector3.left;	//what direction bullets will fly
public Transform overlay;
CharacterController controller;	
//collision flags from controller
private CollisionFlags collisionFlags; 
private Status status;
	//for input
	private double h = 0.0f;
	private double v = 0.0f;
	//for external control
	private bool jump = false;
private Vector3 moveDirection = Vector3.zero;
//audio
public AudioClip[] crouchFootsteps;
public AudioClip[] sandFootsteps;
public AudioClip[] woodFootsteps;
public AudioClip[] metalFootsteps;
//et cetera
public AudioClip crouchSound;
public AudioClip getUpSound;
public AudioClip jumpSound;
public AudioClip landSound;


private float audioTimer = 0.0f;
public float audioDelay = 1.0f;


void OnControllerColliderHit(ControllerColliderHit hit){
	if(hit.transform.tag.Equals("Enemy")){
		takeDamage(hit.transform.GetComponent<EnemyController>().touchDamage);
	}
}

public void SetInput(double fH, double fV, bool fjump){	//set the inputs to fake things.  For shooting and throwing, simply call the appropriate functions
		h = fH;
		v = fV;
		jump = fjump;
}

	
public void takeDamage(int damage){
	if(! status.isInvincible()){
		status.substractHealth(damage);	
		status.setInvincibleFor(0.75f);	
	}	
}
void  Start (){
	player = transform;	
	status = GetComponent<Status>();	
	zPosition = transform.position.z;
	controller = GetComponent<CharacterController>();
}

public static Transform getPlayer(){
		return player;

}
	
public int  getDirectionY (){ //returns a value, 1 if facing up, -1 if facing down, 0 if facing left or right
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

public int  getDirectionX (){//returns a value, 1 if facing right, -1 if facing left
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
		Debug.DrawLine (transform.position, transform.position + muzzleLocation, Color.red);
		
		//hold our z value to center player
		transform.position = new Vector3 (transform.position.x, transform.position.y, zPosition);
		//get inputs
		if(!frozen){	//if we're not frozen, or not being controlled
			h = Input.GetAxisRaw ("Horizontal");
			v = Input.GetAxisRaw ("Vertical");
		}

		//PlayerSpriteAnimate overAnimator = overlay.GetComponent<PlayerSpriteAnimate> ();
		PlayerSpriteAnimate animator = GetComponent<PlayerSpriteAnimate> ();
	
		grounded = IsGrounded ();
		if (grounded) {	//on ground controlls---------------
			//don't move down
			moveDirection.y = -10.0f;

			//smooth out the horizontal vector
			if (crouching) {
			
				//slower move speed
				moveDirection.x = Mathf.Lerp ((float)moveDirection.x, (float)(h * crouchSpeed), (float)acceleration);
				//shorter controller
				if (controller.height != 4.36f) {
					CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
					capsuleCollider.center = new Vector3(0,0,1.78f);
					capsuleCollider.height= 4.36f;
					controller.center = new Vector3 (0, 0, 1.78f);
					controller.height = 4.36f;
				}
				//crouching
				muzzleLocation = new Vector3 (-3.2f, -0.55f, 0);
				secondaryMuzzleLocation = new Vector3 (-0.8f, -0.5f, -0.5f);
				muzzleDirection = new Vector3 ((transform.localScale.x / Mathf.Abs (transform.localScale.x)), 0, 0);
			
			} else {
				moveDirection.x = Mathf.Lerp ((float)moveDirection.x, (float)(h * speed), (float)acceleration);
				//regular controller
				if (controller.height != 6.92f) {
					CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
					capsuleCollider.center = new Vector3(0,0,0);
					capsuleCollider.height= 6.92f;
					controller.center = new Vector3 (0, 0, 0);
					controller.height = 6.92f;
				}
				if (v > 0.5f) {//looking up
					muzzleLocation = new Vector3 (-0.101f, 3.63f);
					secondaryMuzzleLocation = new Vector3 (-0.5f, 1.7f, -0.5f);
					muzzleDirection = Vector3.up;
				
				} else {//looking left
					muzzleLocation = new Vector3 (-3, 1.7082f, 0);
					secondaryMuzzleLocation = new Vector3 (-0.5f, 1.7f, -0.5f);
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
				bool wasCrouching = crouching;
				//check orientation
				if (h != 0) {//is moving
		
					//are we looking up?
					if (v > 0.5f && !Physics.Raycast(transform.position,new Vector3(0.2f,1f,0f),5, mask.value) && !Physics.Raycast(transform.position,new Vector3(-0.2f,1f,0f),5, mask.value)) {//looking up and nothing above us
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
					} else if (!Physics.Raycast(transform.position,new Vector3(0.2f,1f,0f),5,mask.value)  && !Physics.Raycast(transform.position,new Vector3(-0.2f,1f,0f),5, mask.value)){	//looking forward and nothing above us
						crouching = false;
						animator.SetAnimation (PlayerSpriteAnimate.animationType.MoveLeft);
						animator.SetFallback (PlayerSpriteAnimate.animationType.IdleLeft);
					}
			
				} else {//is idle
					//are we looking up?
					if (v > 0.5f && !Physics.Raycast(transform.position,new Vector3(0.2f,1,0),5, mask.value)  && !Physics.Raycast(transform.position,new Vector3(-0.2f,1f,0f),5, mask.value)) {//looking up and nothing above us
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
					} else if (!Physics.Raycast(transform.position, new Vector3(0.2f,1,0),5, mask.value)  && !Physics.Raycast(transform.position,new Vector3(-0.2f,1f,0f),5, mask.value)){	//looking forward and nothing above us
						crouching = false;
						animator.SetAnimation (PlayerSpriteAnimate.animationType.IdleLeft);
						animator.SetFallback (PlayerSpriteAnimate.animationType.IdleLeft);
					}
				}
				if(crouching != wasCrouching){
					//something changed, mate
					if (crouching == true) {
						//we started crouching
						if (crouchSound) {
							audio.PlayOneShot (crouchSound);
						}
					} else {
						//we are standing up
						if (getUpSound) {
							audio.PlayOneShot (getUpSound);
						}
					}

				}
			}
		
			//we are on the ground, so check if we can jump
			if (((Input.GetButtonDown ("Jump") && !frozen) || jump) && !Physics.Raycast(transform.position,new Vector3(0.2f,1,0),5, mask.value)  && !Physics.Raycast(transform.position,new Vector3(-0.2f,1f,0f),5, mask.value)) {
				//reset jump input
				jump = false;

				//play sound
				if (jumpSound) {
					audio.PlayOneShot (jumpSound);
				}

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
				//audio
				if (landSound) {
					audio.PlayOneShot (landSound);
				}

				jumping = false;
				shooting = false;
				throwing = false;
				shootTimer = 0.0f;
				moveDirection.y = -0.05f;
			}
			//if we just fell
			if (!jumping && !grounded) {
				jumping = true;
			}


			//AUDIO>>>>>>>>>

			if(h !=0){
				audioTimer += Time.deltaTime;

				if(crouching){

				}else{//not crouching
					if (audioTimer >= audioDelay){	//time to play a sound!
						audioTimer = 0.0f;
						RaycastHit Rhit;
						if (Physics.Raycast (transform.position, -Vector3.up, out Rhit, 3.7f, mask.value)) {	//shoot a ray down
							if (Rhit.transform.tag.Equals ("Sand")) {	//we hit sand
								//PLAY A SOUND
								int index = (int) (Random.value * sandFootsteps.Length);
								if (sandFootsteps[index]) {
									audio.PlayOneShot (sandFootsteps[index]);
								}
							} else if (Rhit.transform.tag.Equals ("Wood")) {	//we hit wood


							}// get the idea?


						}

					}
				}
			}




		//in air
		} else {//in air controls-----------------------------
			//smooth out the horizontal vector
			moveDirection.x = Mathf.Lerp ((float)moveDirection.x, (float)(h * speed), (float)inAirAccel);
			moveDirection.y -= (float)(gravity * Time.deltaTime);
			if (Input.GetButton ("Jump") && moveDirection.y > 0) {
				moveDirection.y += (float)(jumpModifier * Time.deltaTime);	
			}
			secondaryMuzzleLocation = new Vector3 (-0.5f, 1.7f, -0.5f);
		
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
	
		//if we bumped our head
		if(Physics.Raycast(transform.position, Vector3.up,5, mask.value) && moveDirection.y > 0){
			//we hit something
			SetVelocity(new Vector3(moveDirection.x,0,(float)(-1.0f * gravity)));


		}
		/*
		//test for secondary fire
		if (Input.GetButtonDown ("Fire2") && canThrow) {
			Toss ();
			throwing = true;
			shooting = false;
			shootTimer = 0.0f;//quicker than shoot
		}
		*/
	
		if (throwing) {	//we are throwing
			if (grounded) {//on the ground
				if (crouching) {//crouching
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
					if (v > 0.5f && !Physics.Raycast(transform.position, Vector3.up,5, mask.value)) {//we are looking up and nothing above us
						overlay.renderer.enabled = true;
						animator.SetAnimation (PlayerSpriteAnimate.animationType.ShootMoveUp);
						crouching = false;
					
					} else if (v < -0.5f || crouching) {//crouching
						//don't allow to crouch fire and move
						moveDirection.x = 0;
						animator.SetAnimation (PlayerSpriteAnimate.animationType.ShootCrouch);
						animator.SetFallback (PlayerSpriteAnimate.animationType.CrouchLeft);
						overlay.renderer.enabled = false;
						crouching = true;
					} else if(!Physics.Raycast(transform.position, Vector3.up,5, mask.value)){//left and nothing above us
						overlay.renderer.enabled = true;
						animator.SetAnimation (PlayerSpriteAnimate.animationType.ShootMoveLeft);
						crouching = false;
					}
				} else {//we are idle
					overlay.renderer.enabled = false;	
					if (v > 0.5f && !Physics.Raycast(transform.position, Vector3.up,5, mask.value)) {//we are looking up and nothing above us
						animator.SetAnimation (PlayerSpriteAnimate.animationType.ShootUp);
						animator.SetFallback (PlayerSpriteAnimate.animationType.CrouchLeft);
						crouching = false;
					} else if (v < -0.5f) {//crouching
						animator.SetAnimation (PlayerSpriteAnimate.animationType.ShootCrouch);
						animator.SetFallback (PlayerSpriteAnimate.animationType.CrouchLeft);
						crouching = true;
					} else if(!Physics.Raycast(transform.position, Vector3.up,5, mask.value)){//left and nothing above us
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



		if (hit) {
			//increment the hit timer
			hitTimer += Time.deltaTime;
			//we got hit, baby
			if (!IsGrounded ()) {
				//jumping
				animator.SetAnimation (PlayerSpriteAnimate.animationType.HitJump);
			} else if (getDirectionY () == 0 && !crouching) {
				//facing left
				animator.SetAnimation (PlayerSpriteAnimate.animationType.HitLeft);
			} else if (getDirectionY () == 1 && !crouching) {
				//facing up
				animator.SetAnimation (PlayerSpriteAnimate.animationType.HitUp);
			} else {
				//crouching
				animator.SetAnimation (PlayerSpriteAnimate.animationType.HitCrouch);
			}
			if(hitTimer > hitTime){
				hitTimer = 0.0f;
				hit = false;
			}
		}
	
	
		controller.Move(moveDirection * Time.deltaTime);
	
		
	
}

public void AddVelocity (Vector3 velocity) //for knockback, etc
{
	moveDirection += velocity;
}

public void SetVelocity (Vector3 velocity)	//completely overrides current movement
{
	moveDirection = velocity;
}

public void  fire (){
		if (canShoot) {
			WeaponImplementer weapon = GetComponent<WeaponImplementer> ();
			shooting = true;
			throwing = false;
			shootTimer = 0.0f;
			if (grounded) {
				weapon.fire (muzzleLocation, muzzleDirection, new Vector3 (moveDirection.x, 0.0f, 0.0f));
			} else {
				weapon.fire (muzzleLocation, muzzleDirection, moveDirection);
			}
		}
}

public void Toss (){	//throw
	if (canThrow) {
		WeaponImplementer weapon = GetComponent<WeaponImplementer> ();
		throwing = true;
		shooting = false;
		shootTimer = 0.0f;//quicker than shoot
			Debug.Log ("Threw");
		if (grounded) {
			weapon.fire2 (secondaryMuzzleLocation, muzzleDirection, new Vector3 (moveDirection.x, 0.0f, 0.0f));
		} else {
			weapon.fire2 (secondaryMuzzleLocation, muzzleDirection, moveDirection);
		}
	}
}

public void Hit (){
		Debug.Log("HIT");
		hitTimer = 0.0f;
		hit = true;
}

//allow us to shoot
public void  SetCanShoot ( bool yesno  ){
	 canShoot = yesno;
}

//allow us to throw
public void  SetCanThrow ( bool yesno  ){
	 canThrow = yesno;
}

//freeze us
public void SetFrozen(bool freeze){
	frozen = freeze;
	this.SetVelocity(Vector3.zero);	
}

public bool  IsGrounded (){
	return controller.isGrounded;
}


}
