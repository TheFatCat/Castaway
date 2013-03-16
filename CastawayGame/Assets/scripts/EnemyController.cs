using UnityEngine;
using System.Collections;
[RequireComponent (typeof(CharacterController))]
[RequireComponent (typeof(Status))]
public class EnemyController : MonoBehaviour {
	public float gravityAcceleration ;
	public int touchDamage = 0; // damage done each time enemy touches player
	[SerializeField]Vector3 velocity;
	private CollisionFlags collisionFlags;
	CharacterController controller ;
	private float zPosition = 0;
	
	static bool frozen;
	public static void freezeAllEnemies(){
		frozen = true;
	}
	
	public static void unfreezeAllEnemies(){
		frozen = false;
	}
	void Start(){
		zPosition = transform.position.z;
		controller = GetComponent<CharacterController>();
	}
	
	// check to see if we hit the player and if so do damage
	void OnControllerColliderHit(ControllerColliderHit hit){
		if(hit.transform.tag.Equals("Player")){
			hit.transform.GetComponent<PlayerController>().takeDamage(touchDamage);
			
		}
	}
	// Update is called once per frame
	void Update () {
		if(! frozen){
			// make sure the crab stays on the right zposition
			transform.position = new Vector3 (transform.position.x, transform.position.y, zPosition);
			
			// move the crab
			collisionFlags = controller.Move(velocity * Time.deltaTime);
			if(IsGrounded()){
				setYSpeed(-.05f); // need a very small y velocity to keep the crab grounded every frame
			}
			
			else{ // accelerate downward
				setYSpeed(getYSpeed() - gravityAcceleration * Time.deltaTime);
			}
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
