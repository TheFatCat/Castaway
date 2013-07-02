using UnityEngine;
using System.Collections;

public class FootprintsController : MonoBehaviour {

	public Transform player;
	public Vector3 offset;
	private CharacterController controller;
	private ParticleSystem ps;
	public AudioClip[] sandFootsteps;
	public float soundDistance = 1.0f;		//distance between sounds
	public float soundTrigger = 0.0f;
	private float trigger = 0.0f;			//distance trigger
	public float triggerdistance = 1.0f;	//how far between prints
	public float raylength = 3.0f;
	public LayerMask mask = -1;



	// Use this for initialization
	void Start () {
		controller = player.GetComponent<CharacterController>();
		ps = GetComponent<ParticleSystem>();
		trigger = transform.position.x + triggerdistance;	//start off the trigger a little to the right
		soundTrigger = transform.position.x + soundDistance;	//start off the sound trigger a little to the right
	}
	
	// Update is called once per frame
	void Update ()
	{

		transform.position = player.position;
		transform.position += offset;
		//ps.Emit(1);	//emit a particle
		if (controller.isGrounded) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position, Vector3.up * -raylength, out hit, mask.value)) {
				//ps.Emit(1);	//emit a particle
				if (hit.transform.tag.Equals ("Sand")) {
					if (transform.position.x >= trigger + triggerdistance) {//if we went farther to the right than the trigger
						ps.Emit (1);	//emit a particle
						trigger += triggerdistance;	//advance the trigger
						
					} else if(transform.position.x <= trigger - (triggerdistance * 2.0f)){	//if we went farther to the left of trigger
						ps.Emit (1);	//emit a particle
						trigger -= triggerdistance;	//advance the trigger
					}
					//sound 
					/*
					if (transform.position.x >= soundTrigger) {	//far enough right to make a sound
						int index = (int) (Random.value * sandFootsteps.Length);
						if (sandFootsteps[index]) {
							audio.PlayOneShot (sandFootsteps[index]);
						}
						Debug.Log ("went right");

						soundTrigger += soundDistance;	//advance the sound Trigger
					} else if (transform.position.x <= soundTrigger - soundDistance) { // far enough left to make a sound
						int index = (int) (Random.value * sandFootsteps.Length);
						if (sandFootsteps[index]) {
							audio.PlayOneShot (sandFootsteps[index]);
						}

						Debug.Log ("went left");

						soundTrigger -= soundDistance;	//advance the sound Trigger
					}
					*/

				}
			
			}

		} else {
			trigger = transform.position.x;
		}
	
	}
}
