using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public int speed = 30;
	public float deathTime = 2f;//how many seconds till destroyed
	float timer = 0f;
	public int damage = 1; // how much damage this bullet will do to a status
	public int critDamage = 2;
	//hit particles
	public Transform damageHit;
	public Transform damageCrit;	//critical Hit
	public float critChance = 0.0f;
	public Transform normalHit;
	public Transform sandyHit;
	public Transform woodenHit;
	public Transform metalHit;
	public float Ricochet = 0.0f;

	public GameObject HPText;	//for the generic text
	//add more when we have more

	
	
	
	void OnCollisionEnter(Collision collision) {
		// if the object has a status take health from the status and destroy the bullet
		Status status = collision.transform.GetComponent<Status>();
		if(status != null ){
			//roll and see if we did critical damage
			if(Random.value < critChance){

				//instantiate the damage text if not invincible
				if(!status.isInvincible()){
					GameObject name = Instantiate (HPText, transform.position, Quaternion.identity) as GameObject;
					name.GetComponent<TextMesh> ().text = "-" + critDamage.ToString();
				}
				//CRITICAL HIT!!!
				status.substractHealth (critDamage);
				Instantiate(damageCrit,transform.position,transform.rotation);

			} else{

				//instantiate the damage text if not invincible
				if(!status.isInvincible()){

					GameObject name = Instantiate (HPText, transform.position, Quaternion.identity) as GameObject;
					name.GetComponent<TextMesh> ().text = "-" + damage.ToString();
				}
				//we hit an enemy
				status.substractHealth(damage);
				Instantiate(damageHit,transform.position,transform.rotation);
						
			}

		}
		//check for the material of what we hit	
		string tag = collision.transform.tag;



		if (tag.Equals ("Metal")) {
			//hit metal, so make a thing
			Instantiate (metalHit, transform.position, transform.rotation);
			//reverse Direction
						Debug.Log (rigidbody.velocity);
						rigidbody.velocity = -Ricochet * rigidbody.velocity.normalized * speed;
						Debug.Log (rigidbody.velocity);
						transform.Rotate (new Vector3 (0, 0, 180));
						//transform.rotation  =  Quaternion.Euler(0,0,transform.rotation.z);
						//changeVelocity (rigidbody.velocity * -Ricochet);
		}else{
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
		
		
		
	}
	// Update is called once per frame
	void Update () {
		// if bullet reaches death time before colliding destroy it
		timer += Time.deltaTime;
		if(timer >= deathTime){
			Destroy(gameObject);
		}
	}

	public void addVelocity(Vector3 velocity){
		rigidbody.velocity += velocity;
	}

	public void changeVelocity(Vector3 velocity){
		rigidbody.velocity = velocity;
	}
}
