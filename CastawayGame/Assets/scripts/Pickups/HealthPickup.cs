using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
//this is the pickup script for picking up healths
public class HealthPickup : MonoBehaviour {

	[SerializeField] private GameObject healthPoof; //the flash after health is picked up
	[SerializeField] private GameObject healthName;	//the text prefab that appears when picket up
	[SerializeField] private int healthValue; // the number of healths reveived from this pickup




	void Start () {
		this.collider.isTrigger = true;	//this is really unnessessary....

	}


	//delete the health instantiate a health flash and add the healths value to the player inventory
	void OnTriggerEnter(Collider collider){
		if (collider.transform == PlayerController.getPlayer ()) {
			Instantiate (healthPoof, transform.position, Quaternion.identity);
			GameObject name = Instantiate (healthName, transform.position, Quaternion.identity) as GameObject;
			name.GetComponent<TextMesh> ().text = "+" + healthValue.ToString ();
			PlayerController.getPlayer ().GetComponent<PlayerStatus> ().addHealth (healthValue);
			Destroy (gameObject);

		} 
	}
}
