using UnityEngine;
using System.Collections;


public class shadow : MonoBehaviour {
	//the player
	public Transform parent;
	public float maxDistance = 10f;
	public LayerMask mask;
	public Transform exitParticle;
	private bool engaged = false;

	// Update is called once per frame
	void Update ()
		{
			//if there is something below the parent, put us there
			

			RaycastHit hit;
			if (Physics.Raycast (parent.position + (Vector3.up * 2.0f), -Vector3.up, out hit, maxDistance, mask)) {
				transform.position = hit.point;	
				if (exitParticle != null && engaged == false) {
					
					Instantiate (exitParticle, transform.position, Quaternion.identity);
				}

				engaged = true;
				
			} else {//nothing below us
						if (exitParticle != null && engaged == true) {
								
								Instantiate (exitParticle, transform.position, Quaternion.identity);
						}
						engaged = false;
					transform.position = parent.position - (Vector3.up * 20f);
			}
	

	}
}
