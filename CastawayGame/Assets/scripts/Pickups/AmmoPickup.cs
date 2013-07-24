using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
//this is the pickup script for picking up ammos
public class AmmoPickup : MonoBehaviour {

	[SerializeField] private GameObject ammoPoof; //the flash after ammo is picked up
	[SerializeField] private GameObject ammoName;	//the text prefab that appears when picket up
	[SerializeField] private int ammoValue; // the number of ammos reveived from this pickup




	void Start () {
		this.collider.isTrigger = true;	//this is really unnessessary....

	}


	//delete the ammo instantiate a ammo flash and add the ammos value to the player inventory
	void OnTriggerEnter(Collider collider){
		if (collider.transform == PlayerController.getPlayer ()) {
			if(PlayerController.getPlayer().GetComponent<WeaponImplementer>().getWeapons().Count !=0){	//make sure we have a weapon
				Instantiate (ammoPoof, transform.position, Quaternion.identity);
				GameObject name = Instantiate (ammoName, transform.position, Quaternion.identity) as GameObject;
				name.GetComponent<TextMesh> ().text = "+" + ammoValue.ToString ();
				PlayerController.getPlayer ().GetComponent<WeaponImplementer> ().addAmmo (ammoValue);
				Destroy (gameObject);
			}


		} 
	}
}
