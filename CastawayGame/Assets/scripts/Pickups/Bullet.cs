using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public int speed = 30;
	public float deathTime = 2f;//how many seconds till destroyed
	float timer = 0f;
	public int damage = 1; // how much damage this bullet will do to a status
	//hit particles
	public Transform normalHit;
	public Transform damageHit;
	public Transform sandyHit;
	public Transform woodenHit;
	public GameObject HPText;	//for the generic text
	//add more when we have more

	
	
	
	void OnCollisionEnter(Collision collision) {
		// if the object has a status take health from the status and destroy the bullet
		Status status = collision.transform.GetComponent<Status>();
		if(status != null && !collision.transform.tag.Equals("Player")){
			status.substractHealth(damage);
			//we hit an enemy
			Instantiate(damageHit,transform.position,transform.rotation);
			//instantiate the damage text
			GameObject name = Instantiate (HPText, transform.position, Quaternion.identity) as GameObject;
			name.GetComponent<TextMesh> ().text = "-" + damage.ToString();
		}
		//check for the material of what we hit	
		string tag = collision.transform.tag;
		if(tag.Equals("Sand")){
			// we hit sand
			Instantiate(sandyHit,transform.position,transform.rotation);
		} else  if(tag.Equals("Wood")){
			//we hit wood
			Instantiate(woodenHit,transform.position,transform.rotation);
		} else if(status == null){
			//we hit something else
			Instantiate(normalHit,transform.position,transform.rotation);
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
