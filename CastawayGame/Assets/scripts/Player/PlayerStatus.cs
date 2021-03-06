using UnityEngine;
using System.Collections;

public class PlayerStatus : Status {

	//private PlayerSpriteAnimate PlayerAnimator;
	private PlayerController controller;
	//public float invincibleTime = 1.0f;
	public HUD hud;
	public GameObject HPText;
	public float invincibleIncrement = 0.05f;
	private float invincibleFlashTimer = 0.0f;
	public float deadTime = 5.0f;	//for keeping track of how long the player is dead
	private float deadTimer = 0.0f;
	public bool isDead = false;	//if player is dead but not respawned

	void Start(){

		controller  = GetComponent<PlayerController>();
	}
	
	
	
	void Update ()
	{
		//animator takes care of flashing by itself 

		if (invincible && !isDead) {	//very good,hmm, yes
			invincibleFlashTimer += Time.deltaTime;
			if (invincibleFlashTimer > invincibleIncrement) {
				invincibleFlashTimer = 0;
				renderer.enabled = !renderer.enabled;
			}
			invincibleTimer += Time.deltaTime;
			if (invincibleTimer > invincibleTime) {
				
				renderer.enabled = true;
				invincible = false;
				invincibleFlashTimer = 0;
				
			}
			
		}

		if (isDead) {	//we are dead but not respawned
			deadTimer += Time.deltaTime;

			if(deadTimer >= deadTime){	//time's up!
				health = (int)(.8 * maxHeath);
				transform.position = PlayerSpawn.getPlayerSpawn().position;	//respawn
				renderer.enabled = true;	// you can see me again!
				isDead = false;
				controller.SetFrozen(false);
				deadTimer = 0.0f;
			}
		}
	}

	//overrides the parent script (ERMAGHERD THE STUFF WE LEARN IN APCS IS USEFUL)
	public override void substractHealth(int damage){
		if(damage < 0){
			return;
		}
		//bounce back? for now just stop moving
		controller.SetVelocity (Vector3.zero);

		if(! invincible && health >= 0){
			//lets get hit
			controller.Hit();
			//invincible = true;

			//play the hit sound
			if (hitSound.Length > 0 && audio) {
				Debug.Log ("hitSound");
				//PLAY A SOUND
				int index = (int) (Random.value * hitSound.Length);
				if (hitSound[index]) {
					audio.PlayOneShot (hitSound[index]);
				}

			}


			//HUD hud = GetComponent<HUD> ();
			hud.hit ();

			//instantiate the hit prefab
			if(hitPrefab != null){
				Instantiate(hitPrefab,transform.position, Quaternion.identity);

			}
			//instantiate text
			GameObject name = Instantiate (HPText, transform.position, Quaternion.identity) as GameObject;
			name.GetComponent<TextMesh> ().text = "-" + damage.ToString();

			health -= damage;
			if(health <= 0){
				die();
			}
		}
	}

	public override void die(){
		Debug.Log(transform.name + " just died");
		isDead = true;
		Instantiate(deathPrefab,transform.position, transform.rotation);
		renderer.enabled = false;	//you can't see me
		//disable player input
		controller.SetFrozen(true);
	}
}
