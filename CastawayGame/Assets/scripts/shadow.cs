using UnityEngine;
using System.Collections;


public class shadow : MonoBehaviour {
	//the player
	public Transform parent;
	public float maxDistance = 10f;
	public int maxAlpha = 150;
	public int minAlpha = 50;
	public float alpha= 0f;

	// Update is called once per frame
	void Update ()
		{
			//if there is something below the parent, put us there
			int mask = ~(1 << 10);
			RaycastHit hit;
			if (Physics.Raycast (parent.position, -Vector3.up, out hit, 100f, mask)) {
						transform.position = hit.point;
			} else {//nothing below us
				transform.position = parent.position - (Vector3.up * 20f);
			}
	

	}
}
