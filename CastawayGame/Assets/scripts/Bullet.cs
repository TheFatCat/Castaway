using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public int speed = 30;
	public float deathTime = 2f;//how many seconds till destroyed
	float timer = 0f;
	public int damage = 1;
	
	
	void OnCollisionEnter(Collision collision) {
		
		Status status = collision.transform.GetComponent<Status>();
		if(status != null){
			status.substractHealth(damage);
		}
			
		Destroy(gameObject);
		
		
		
	}
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer >= deathTime){
			Destroy(gameObject);
		}
	}
	
	public void changeVelocity(Vector3 velocity){
		rigidbody.velocity = velocity * speed;
	}
}
