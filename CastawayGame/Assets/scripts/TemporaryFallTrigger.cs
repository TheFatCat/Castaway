using UnityEngine;
using System.Collections;

public class TemporaryFallTrigger : MonoBehaviour {

	public float fallSpeed = 0.0f;
	private float curSpeed = 0.0f;
	public Transform fall;
	public Transform particleHost;
	public GameObject fallingCube;
	private bool beganCountdown = false;
	public float timer = 3;
	public float rumblePower = 40;
	private PlayerController controller;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(beganCountdown){
			timer -= Time.deltaTime;
			if(timer < 0){
				beganCountdown = false;
				controller.frozen = false;
				Destroy(fallingCube);	//destroy the platform
				curSpeed = fallSpeed;	//make particles move
				ParticleSystem ps = particleHost.GetComponent<ParticleSystem>();
				ps.Play(true);	//enable particles
				SynchronizedAnimate sa = fall.GetComponent<SynchronizedAnimate>();
				sa.SetEnabled(true);
			}
		}
		particleHost.position -= new Vector3(0,curSpeed * Time.deltaTime,0);
	
	}

	void OnTriggerEnter (Collider other){
		if (other.CompareTag ("Player")) {
			Camera.mainCamera.GetComponent<CameraScript>().Rumble(rumblePower, timer);
			beganCountdown = true;
			controller = other.GetComponent<PlayerController>();
			controller.frozen = true;
		}
	}
}
