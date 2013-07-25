using UnityEngine;
using System.Collections;

public class DroppablePickup : MonoBehaviour {

	public float startSpeed = 0.0f;

	void Start () {
		if (this.GetComponent<Rigidbody> ()) {
			//random velocity up
			this.GetComponent<Rigidbody> ().velocity = new Vector3 ((Random.value - 0.5f) * 2.0f * startSpeed, startSpeed + (Random.value * startSpeed), 0.0f);
		}
	}
	
	void OnTriggerEnter(Collider collider){
		if(!collider.isTrigger  && collider.tag != "Bullet" && collider.tag != "Enemy" && !collider.isTrigger){	//we touched something else
			Debug.Log ("touched" + collider.name);
			if (this.GetComponent<Rigidbody> ()) {	//if we have as rigidbody
				Rigidbody rb = this.GetComponent<Rigidbody> ();
				rb.isKinematic = true;	//stop  movin



			}

		}

	}
}
