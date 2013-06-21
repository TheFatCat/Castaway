using UnityEngine;
using System.Collections;

public class WeaponPickup : MonoBehaviour {
	public Transform pickupFlash;
	public GameObject pickupNamePrefab;
	public string title = "weapon";
	void Start(){
		collider.isTrigger = true;
	}
	[SerializeField] private Weapon weapon;
	private WeaponImplementer implementer;
	private bool canBePickedUp;
	void OnTriggerEnter(Collider collider){
		if(collider.GetComponent<PlayerController>() != null){
			//player has entered into the trigger so can pick up this weapon
			canBePickedUp = true;
			//get the players weapon implementer
			implementer = collider.GetComponent<WeaponImplementer>();
		}
	}
	
	void OnTriggerExit(Collider collider){
		if(collider.GetComponent<PlayerController>() != null){
			//player has exited and no longer can pick up this weapon
			canBePickedUp = false;
		}
	}
	
	
	void Update(){
		//player presses down to pick up weapon
		if(canBePickedUp && Input.GetAxisRaw("Vertical") < -0.5){
			// add the weapon to the players array of weapons
			implementer.addWeapon(weapon);
			//instantiate the pickup prefab
			Instantiate(pickupFlash,transform.position,transform.rotation);


			GameObject name = Instantiate (pickupNamePrefab, transform.position, Quaternion.identity) as GameObject;
			name.GetComponent<TextMesh> ().text = "" + title;

			//name.text = "hello";

			//Destroy the leftover object
			Destroy(gameObject);
			
			//pickup effects for later ie sound animation
		}
		
	}
}
