using UnityEngine;
using System.Collections;
// this is a status object that all objects 

[RequireComponent (typeof(Animator))]
public class Status :MonoBehaviour {
	Animator animator;
	[SerializeField] Animation hitFrame; // animation frame when object is hit
	[SerializeField] private GameObject deathPrefab ;
	[SerializeField] private bool shouldDestroyOnDeath = true;
	private bool invincible = false; // make the object invincible from damage
	[SerializeField] int health = 0;
	[SerializeField] int maxHeath = 100;
	//[SerializeField] int mana = 0;
	//[SerializeField] Color takeDamageColor = Color.red; // when object takes damage what should the object flash in color
	//public MonoBehaviour controllerScript;
	private float invincibleTimer = 0; // for keeping track of how long the object should be invincible
	private float invincibleTime = 0;
	private float flashTimer = 0.5f; // for keeping track of how long the object should flash
	
	
	void Start(){
		animator = GetComponent<Animator>();
	}
	
	
	void Update(){
		if(flashTimer < 0.2f){
			flashTimer += Time.deltaTime;
			if(flashTimer > 0.2f){
				animator.endFlashFrame();
				//renderer.material.color = Color.white; // after a tenth of a second switch back to normal color
			}
		}
		if(invincible){
			
			invincibleTimer += Time.deltaTime;
			if(invincibleTimer > invincibleTime){
				
				
				invincible = false;
				
			}
			
		}
	}
	public void substractHealth(int damage){
		
		if(damage < 0){
			return;
		}
		if(! invincible){
			if(animator != null){
				animator.flashFrame(hitFrame);
			}
			//renderer.material.color = takeDamageColor;
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
	
	public void die(){
		Debug.Log(transform.name + " just died");	
		if(shouldDestroyOnDeath){
			Destroy(gameObject);
		}
		Instantiate(deathPrefab,transform.position, Quaternion.identity);
		
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
