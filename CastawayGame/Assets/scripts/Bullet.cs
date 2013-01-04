using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public int speed = 30;
	public float deathTime = 2f;//how many seconds till destroyed
	float timer = 0f;
	public int damage = 1; // how much damage this bullet will do to a status
	
	
	
	void OnCollisionEnter(Collision collision) {
		// if the object has a status take health from the status and destroy the bullet
		Status status = collision.transform.GetComponent<Status>();
		if(status != null){
			status.substractHealth(damage);
		}
			
		Destroy(gameObject);
		
		
		
	}
	// Update is called once per frame
	void Update () {
		// if bullet reaches death time before colliding destroy it
		timer += Time.deltaTime;
		if(timer >= deathTime){
			Destroy(gameObject);
		}
	}
	
	public void changeVelocity(Vector3 velocity){
		rigidbody.velocity = velocity * speed;
	}
}
