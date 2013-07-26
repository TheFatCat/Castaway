using UnityEngine;
using System.Collections;


public class shadow : MonoBehaviour {
	//the player
	public Transform parent;
	public float maxDistance = 10f;
	public LayerMask mask;

	// Update is called once per frame
	void Update ()
		{
			//if there is something below the parent, put us there
			
			RaycastHit hit;
			if (Physics.Raycast (parent.position + (Vector3.up * 2.0f), -Vector3.up, out hit, maxDistance, mask)) {
						transform.position = hit.point;
			} else {//nothing below us
				transform.position = parent.position - (Vector3.up * 20f);
			}
	

	}
}
