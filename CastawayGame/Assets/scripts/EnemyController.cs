using UnityEngine;
using System.Collections;
[RequireComponent (typeof(CharacterController))]
[RequireComponent (typeof(Status))]
public class EnemyController : MonoBehaviour {
	public float gravityAcceleration ;
	public int touchDamage = 0;
	Vector3 velocity;
	private CollisionFlags collisionFlags;
	CharacterController controller ;
	
	void Start(){
		controller = GetComponent<CharacterController>();
	}/*
	void OnTriggerEnter(Collider collider){
		//Debug.Log("hitting " + hit.transform.name);
		if(collider.transform.tag.Equals("Player")){
			Debug.Log("hit player");
			collider.GetComponent<Status>().substractHealth(touchDamage);	
		}
	}
	*/
	/*
	void OnCollisionEnter(Collision collision){
		if(collision.transform.tag.Equals("Player")){
			collision.transform.GetComponent<Status>().substractHealth(touchDamage);	
		}
	}
	*/
	
	
	void OnControllerColliderHit(ControllerColliderHit hit){
		if(hit.transform.tag.Equals("Player")){
			hit.transform.GetComponent<Status>().substractHealth(touchDamage);
		}
	}
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (transform.position.x, transform.position.y, 10f);
		collisionFlags = controller.Move(velocity * Time.deltaTime);
		if(IsGrounded()){
			setYSpeed(-.05f);
		}
		
		else{
			setYSpeed(getYSpeed() - gravityAcceleration * Time.deltaTime);
		}
	}
	public float getXSpeed(){
		return velocity.x;
	}
	
	public float getYSpeed(){
		return velocity.y;
	}
	
	public void setYSpeed(float speed){
		velocity.y = speed;
	}
	
	public void setXSpeed(float speed){
		velocity.x = speed;
	}
	
	public bool  IsGrounded (){
		return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
	}
}
