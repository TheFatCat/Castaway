using UnityEngine;
using System.Collections;

public class AudioOnPlayerEnter : MonoBehaviour {
	public Transform audioplayer;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider collider){
		if (collider.transform.audio && collider.tag == "Player") {
				audioplayer.GetComponent<AudioSource>().audio.Play();
				
		}
	}
}
