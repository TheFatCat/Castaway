using UnityEngine;
using System.Collections;

public class PlayerStatus : Status {

	//private PlayerSpriteAnimate PlayerAnimator;
	private PlayerController controller;
	//public float invincibleTime = 1.0f;
	public float invincibleIncrement = 0.05f;
	private float invincibleFlashTimer = 0.0f;
	public float deadTime = 5.0f;	//for keeping track of how long the player is dead
	private float deadTimer = 0.0f;

	void Start(){

		controller  = GetComponent<PlayerController>();
	}
	
	
	
	void Update(){
		//animator takes care of flashing by itself 

		if(invincible){	//very good,hmm, yes
			invincibleFlashTimer += Time.deltaTime;
			if(invincibleFlashTimer > invincibleIncrement){
				invincibleFlashTimer = 0;
				renderer.enabled = !renderer.enabled;
			}
			invincibleTimer += Time.deltaTime;
			if(invincibleTimer > invincibleTime){
				
				renderer.enabled = true;
				invincible = false;
				invincibleFlashTimer = -invincibleTime;
				
			}
			
		}
	}

	//overrides the parent script (ERMAGHERD THE STUFF WE LEARN IN APCS IS USEFUL)
	public override void substractHealth(int damage){
		if(damage < 0){
			return;
		}
		if(! invincible){
			//lets get hit
			controller.Hit();

			//instantiate the hit prefab
			if(hitPrefab != null){
				Instantiate(hitPrefab,transform.position, Quaternion.identity);
			}
			

			health -= damage;
			if(health <= 0){
				die();
			}
		}
	}

	public override void die(){
		Debug.Log(transform.name + " just died");	
		if(shouldDestroyOnDeath){
			Destroy(gameObject);
		}
		Instantiate(deathPrefab,transform.position, Quaternion.identity);
		//respawn the player after 2 seconds
		
		if(transform == PlayerController.getPlayer()){
			health = (int)(.8 * maxHeath);
			transform.position = PlayerSpawn.getPlayerSpawn().position;
		}
	}
}
