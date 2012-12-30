using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
	void OnTriggerEnter(Collider collider){
		if(collider.transform.tag.Equals("Player")){
			transform.parent.collider.enabled = false;
				
		}
	}
	
	void OnTriggerExit(Collider collider){
		if(collider.transform.tag.Equals("Player")){
			transform.parent.collider.enabled = true;
				
		}
	}

	
}
