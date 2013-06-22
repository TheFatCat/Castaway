using UnityEngine;
using System.Collections;

public class FootprintsController : MonoBehaviour {

	public Transform player;
	public Vector3 offset;
	private CharacterController controller;
	private ParticleSystem ps;
	public float trigger = 0.0f;			//distance trigger
	public float triggerdistance = 1.0f;	//how far between prints
	public float raylength = 3.0f;
	public LayerMask mask = -1;

	// Use this for initialization
	void Start () {
		controller = player.GetComponent<CharacterController>();
		ps = GetComponent<ParticleSystem>();
		trigger = transform.position.x + triggerdistance;
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
					if (transform.position.x >= trigger) {//if we went farther to the right than the trigger
						ps.Emit (1);	//emit a particle
						trigger += triggerdistance;	//advance the trigger
					} else if(transform.position.x <= trigger - (triggerdistance * 2.0f)){	//if we went farther to the left of trigger
						ps.Emit (1);	//emit a particle
						trigger -= triggerdistance;	//advance the trigger
					}
				}
			
			}

		} else {
			trigger = transform.position.x;
		}
	
	}
}
