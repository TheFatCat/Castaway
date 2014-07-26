using UnityEngine;
using System.Collections;

public class HackPlayerController : MonoBehaviour {
	
	//variables and stuff
	public float velocity = 1.0f;
	//private CharacterController cc = this.GetComponent<CharacterController>();
	private Vector3 moveDir = Vector3.zero;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		
		moveDir = new Vector3(h,v,0) * velocity * Time.deltaTime;
		
		transform.position = transform.position + moveDir;
		
	}
}
