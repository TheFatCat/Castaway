using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
	public int health = 30;
	private bool isGrounded = false;
	public LayerMask floorLayerMask;
	private float f_lastY;
	private Transform player;
	public int detectionDistance = 10;
	// Use this for initialization
	void OnBecameVisible(){
		enabled = true;
	}
	void OnBecameInvisible(){
		enabled = false;
	}
	
	void Start () {
		f_lastY = transform.position.y;
		player = GameObject.Find("Player").transform;
	
	}
	
	// Update is called once per frame
	void Update () {
		if(health <= 0	){
			die();
		}
		if(isGrounded && Vector3.Distance(transform.position, player.position) < detectionDistance){
			if(player.position.x > transform.position.x){
				rigidbody.velocity = new Vector3(3,3,0);
			}
			else{
				rigidbody.velocity = new Vector3(-3,3,0);
			}
		}
	
	}
	
	void die(){
		Destroy(gameObject);
	}
	
	public void LateUpdate() {
		//Checking to see if grounded by using a raycast
		
		
		Vector3 v3_right = new Vector3(transform.position.x + (collider.bounds.size.x*0.45f), transform.position.y, transform.position.z);
		Vector3 v3_left = new Vector3(transform.position.x - (collider.bounds.size.x*0.45f), transform.position.y, transform.position.z);

   	 	if (Physics.Raycast (transform.position, -Vector3.up, 0.5f, floorLayerMask.value)) {
       		isGrounded = true;
     	} else if (Physics.Raycast (v3_right, -Vector3.up, 0.5f, floorLayerMask.value)) {
   			if (!isGrounded) {
        		isGrounded = true;
        	}
    	} else if (Physics.Raycast (v3_left, -Vector3.up, 0.5f, floorLayerMask.value)) {
        	if (!isGrounded) {
        		isGrounded = true;
        	}
    	} else {
			if (isGrounded) {
	    		if (Mathf.Floor(transform.position.y) == f_lastY) {
	    			isGrounded = true;
	    		} else {
	    			isGrounded= false;
	    		}
	    	}
		}
    	f_lastY = Mathf.Floor(transform.position.y);
    	//Update Camera
		
	}
}
