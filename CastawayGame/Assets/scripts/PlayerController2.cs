using UnityEngine;
using System.Collections;

public class PlayerScriptTwo : MonoBehaviour {
	CollisionFlags collisionFlags;
	// Use this for initialization
	void Start () {
	
	}
	float fallSpeed = 0;
	// Update is called once per frame
	void Update () {
		collisionFlags = GetComponent<CharacterController>().Move(new Vector3(Input.GetAxisRaw("Horizontal"),0,0) * 5 *Time.deltaTime);
		
		if(! IsGrounded()){
			fallSpeed += 9.8f * Time.deltaTime;
			collisionFlags = GetComponent<CharacterController>().Move(new Vector3(0,-1 * fallSpeed,0) * 5 *Time.deltaTime);
				
		}
		else{
			fallSpeed = 0;
		}
	
	}
	
	public bool  IsGrounded (){
		return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
	}
}
