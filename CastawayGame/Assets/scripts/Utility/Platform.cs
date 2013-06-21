using UnityEngine;
using System.Collections;


// this class allows platformlike colliders that dont collide in certain directions
public class Platform : MonoBehaviour {
	
	// there will be triggers on the platform where the player can go through the platform
	void OnTriggerEnter(Collider collider){
		if(collider.transform.tag.Equals("Player")){
			collider.gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
				
		}
	}
	// reset the collider after the player leaves the trigger
	void OnTriggerExit(Collider collider){
		if(collider.transform.tag.Equals("Player")){
			collider.gameObject.layer = LayerMask.NameToLayer("Player");
				
		}
	}

	
}
