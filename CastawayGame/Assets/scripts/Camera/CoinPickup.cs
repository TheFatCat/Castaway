using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
//this is the pickup script for picking up coins
public class CoinPickup : MonoBehaviour {
	
	[SerializeField] private GameObject coinFlash; //the flash after coin is picked up
	[SerializeField] private GameObject coinName;	//the text prefab that appears when picket up
	[SerializeField] private int coinValue; // the number of coins reveived from this pickup
	public float startSpeed = 1.0f;



	void Start () {
		this.collider.isTrigger = true;	//this is really unnessessary....
		if (this.GetComponent<Rigidbody> ()) {
			this.GetComponent<Rigidbody> ().velocity = new Vector3 ((Random.value - 0.5f) * 2.0f * startSpeed, startSpeed + (Random.value * startSpeed), 0.0f);
		}
	}
	
	
	//delete the coin instantiate a coin flash and add the coins value to the player inventory
	void OnTriggerEnter(Collider collider){
		if (collider.transform == PlayerController.getPlayer ()) {
			Instantiate (coinFlash, transform.position, Quaternion.identity);
			GameObject name = Instantiate (coinName, transform.position, Quaternion.identity) as GameObject;
			name.GetComponent<TextMesh> ().text = "$" + coinValue.ToString ();
			PlayerController.getPlayer ().GetComponent<Inventory> ().addCoins (coinValue);
			Destroy (gameObject);
		} else if(!collider.isTrigger  ){	//we touched something else
			Debug.Log ("touched" + collider.name);
			if (this.GetComponent<Rigidbody> ()) {	//if we have as rigidbody
				Rigidbody rb = this.GetComponent<Rigidbody> ();
				rb.isKinematic = true;	//stop  movin



			}

		}
		
	}
}
