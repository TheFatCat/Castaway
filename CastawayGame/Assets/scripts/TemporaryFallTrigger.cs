using UnityEngine;
using System.Collections;

public class TemporaryFallTrigger : MonoBehaviour {

	public float fallSpeed = 0.0f;
	private float curSpeed = 0.0f;
	public Transform fall;
	public Transform particleHost;
	public GameObject fallingCube;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		particleHost.position -= new Vector3(0,curSpeed * Time.deltaTime,0);
	
	}

	void OnTriggerEnter (Collider other){
		if (other.CompareTag ("Player")) {
			Destroy(fallingCube);	//destroy the platform
			curSpeed = fallSpeed;	//make particles move
			ParticleSystem ps = particleHost.GetComponent<ParticleSystem>();
			ps.Play(true);	//enable particles
			SynchronizedAnimate sa = fall.GetComponent<SynchronizedAnimate>();
			sa.SetEnabled(true);

		}
	}
}
