using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
//this is the pickup script for picking up coins
public class CoinPickup : MonoBehaviour {
	
	[SerializeField] private GameObject coinFlash; //the flash after coin is picked up
	[SerializeField] private GameObject coinName;	//the text prefab that appears when picket up
	[SerializeField] private int coinValue; // the number of coins reveived from this pickup




	void Start () {
		this.collider.isTrigger = true;	//this is really unnessessary....

	}
	
	
	//delete the coin instantiate a coin flash and add the coins value to the player inventory
	void OnTriggerEnter(Collider collider){
		if (collider.transform == PlayerController.getPlayer ()) {
			Instantiate (coinFlash, transform.position, Quaternion.identity);
			GameObject name = Instantiate (coinName, transform.position, Quaternion.identity) as GameObject;
			name.GetComponent<TextMesh> ().text = "$" + coinValue.ToString ();
			PlayerController.getPlayer ().GetComponent<Inventory> ().addCoins (coinValue);
			Destroy (gameObject);

		} 
	}
}
