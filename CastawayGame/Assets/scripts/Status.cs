using UnityEngine;
using System.Collections;
// this is a status object that all objects 
public class Status :MonoBehaviour {
	
	private bool invincible = false; // make the object invincible from damage
	[SerializeField] int health = 0;
	[SerializeField] int maxHeath = 100;
	//[SerializeField] int mana = 0;
	[SerializeField] Color takeDamageColor = Color.red; // when object takes damage what should the object flash in color
	//public MonoBehaviour controllerScript;
	private float invincibleTimer = 0; // for keeping track of how long the object should be invincible
	private float invincibleTime = 0;
	private float flashTimer = 0.5f; // for keeping track of how long the object should flash
	void Update(){
		if(flashTimer < 0.1){
			flashTimer += Time.deltaTime;
			if(flashTimer > 0.1f){
				renderer.material.color = Color.white; // after a tenth of a second switch back to normal color
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
			renderer.material.color = takeDamageColor;
			flashTimer = 0;
			health -= damage;
			if(health <= 0){
				die();
			}
		}
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
		Destroy(gameObject);
		//unimplemented	
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
