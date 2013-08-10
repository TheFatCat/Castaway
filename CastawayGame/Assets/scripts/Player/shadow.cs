using UnityEngine;
using System.Collections;


public class shadow : MonoBehaviour {
	//the player
	public Transform parent;
	public float maxDistance = 10f;
	public LayerMask mask;
	public Transform enterParticle;
	public Transform exitParticle;
	private bool engaged = false;

	// Update is called once per frame
	void Update ()
		{
			//if there is something below the parent, put us there
			

			RaycastHit hit;
			if (Physics.Raycast (parent.position + (Vector3.up * 2.0f), -Vector3.up, out hit, maxDistance, mask)) {
				transform.position = hit.point;	
				if (enterParticle != null && engaged == false) {
					
					Instantiate (enterParticle, transform.position, Quaternion.identity);
				}
				if (audio) {	//if we have audio
					if (!audio.isPlaying) {	//and its not playing
						audio.Play ();	//play the audio!
					}
				}
				engaged = true;
				
			} else {//nothing below us
						if (exitParticle != null && engaged == true) {
								
								Instantiate (exitParticle, transform.position, Quaternion.identity);
						}
						engaged = false;
					if (audio) {	//if there is an audio component
						if (audio.isPlaying) {	//and its still playing
							audio.Pause ();	//pause it
						}
					}
					transform.position = parent.position - (Vector3.up * 100f);
			}
	

	}
}
