using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
	void OnTriggerEnter(Collider collider){
		if(collider.transform.tag.Equals("Player")){
			collider.gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
				
		}
	}
	
	void OnTriggerExit(Collider collider){
		if(collider.transform.tag.Equals("Player")){
			collider.gameObject.layer = LayerMask.NameToLayer("Player");
				
		}
	}

	
}
