using UnityEngine;
using System.Collections;
// this is a status object that all objects 

[RequireComponent (typeof(Animator))]
public class Status :MonoBehaviour {
	private Animator animator;
	[SerializeField] protected Animation hitFrame; // animation frame when object is hit
	[SerializeField] protected  GameObject deathPrefab ;
	[SerializeField] protected  GameObject hitPrefab ;
	[SerializeField] protected AudioClip hitSound;
	public int minDrops = 0;
	public int maxDrops = 0;
	public Vector3 offset = Vector3.zero;
	public GameObject[] drops;
	//public float[] dropRate;				//For when we want to change the frequency of drops
	[SerializeField] protected bool shouldDestroyOnDeath = true;
	protected bool invincible = false; // make the object invincible from damage

	[SerializeField] protected int health = 0;
	[SerializeField] protected int maxHeath = 100;
	//[SerializeField] int mana = 0;
	//[SerializeField] Color takeDamageColor = Color.red; // when object takes damage what should the object flash in color
	//public MonoBehaviour controllerScript;
	protected float invincibleTimer = 0; // for keeping track of how long the object should be invincible
	public float invincibleTime = 0;
	protected float flashTimer = 0.5f; // for keeping track of how long the object should flash
	public float flashTime = 0.25f;




	
	
	void Start(){
		animator = GetComponent<Animator>();
	}
	
	
	void Update(){
		//runs once every frame
		if(flashTimer < flashTime){	//for enemies that need to flash when hit
			flashTimer += Time.deltaTime;

			if(flashTimer > flashTime){
				if(animator != null){
					animator.endFlashFrame();
				}

				//renderer.material.color = Color.white; // after a tenth of a second switch back to normal color
			}
		}
		if(invincible){	//invincible stuff
			
			invincibleTimer += Time.deltaTime;
			if(invincibleTimer > invincibleTime){
				
				
				invincible = false;
				
			}
			
		}
	}
	public virtual void substractHealth(int damage){

		if(damage < 0){
			return;
		}
		if(! invincible){
			if(animator != null){
				animator.flashFrame(hitFrame);
			}
			//play the hit sound
			if (hitSound && audio) {
				audio.PlayOneShot (hitSound);
			}

			//instantiate the hit prefab
			if(hitPrefab != null){
				Instantiate(hitPrefab,transform.position, Quaternion.identity);
			}

			flashTimer = 0;
			health -= damage;
			if(health <= 0){
				die();
			}
		}
	}
	
	
	public int getHealth(){
		return health;
	}
	
	public int getMaxHealth(){
		return maxHeath;
	}
	public void addHealth(int addedHealth){
		if(addedHealth < 0){
			return;
		}
		health += addedHealth;
		
		health = Mathf.Clamp(health, 0, maxHeath);
	}
	
	
	public void increaseMaxHealth(int increase){
		if(increase < 0){
			return;
		}
		
		maxHeath += increase;
	}
	
	public virtual void die(){
		Debug.Log(transform.name + " just died");	
		if(shouldDestroyOnDeath){
			Destroy(gameObject);
		}
		Instantiate(deathPrefab,transform.position, Quaternion.identity);
		//instantiate all pickups spawned
		int toDrop = Random.Range (minDrops, maxDrops);
		for (int i = 0; i< toDrop; i++) {
			//choose a random drop, and instantiate it
			Instantiate (drops[(int) Random.Range (0, drops.Length - 1)], transform.position + offset, Quaternion.identity);
		}



	}
	public bool isInvincible(){
		return invincible;
	}
	public void setInvincibleFor(float seconds){
		invincible = true;
		invincibleTime = seconds;
		invincibleTimer = 0;
	}
	
	
}	
