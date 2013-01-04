using UnityEngine;
using System.Collections;
// this is a status object that all objects 
public class Status :MonoBehaviour {
	
	
	[SerializeField] int health = 0;
	[SerializeField] int mana = 0;
	//public MonoBehaviour controllerScript;
	public void substractHealth(int damage){
		health -= damage;
		if(health <= 0){
			die();
		}
	}
		
	public void die(){
		Debug.Log(transform.name + " just died");	
		Destroy(gameObject);
		//unimplemented	
	}
	
	
}	
