using UnityEngine;
using System.Collections;

public class PlayerPusher : MonoBehaviour {

	public Vector3 pushAccel = Vector3.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider collider){
		if (collider.transform == PlayerController.getPlayer ()) {	//player entered us
			PlayerController.getPlayer ().GetComponent<PlayerController> ().AddVelocity (pushAccel * Time.deltaTime);

		}

	}
}
