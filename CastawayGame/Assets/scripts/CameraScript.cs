using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public Transform target;
<<<<<<< HEAD
	public float cameraDistance = 29.7299f;
	public float cameraHeight = 16.1712f;
	public float Ymin = 23.9f;
	public float Ymax = 100.0f;
	public float Xmin = -271f;
	public float Xmax = 117f;
	public float XminMoveDist = 15f;
	public float YminMoveDist = 12f;
	public float rumbleAmount = 0.0f;
	public float rumbleTimer = 0.0f;
=======
	public float cameraDistance = 10f; // distance in z from the target
	public float cameraHeight = 10f; // distance y from the target
	public float cameraSpeed = 1f; // speed the camera follows the player
>>>>>>> a lot of stuff
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(target != null){
<<<<<<< HEAD
			transform.position = new Vector3(Mathf.Clamp(target.position.x,Xmin,Xmax) ,Mathf.Clamp(target.position.y + cameraHeight,Ymin,Ymax), target.position.z - cameraDistance);
			//if rumbling
			if(rumbleTimer > 0.0f){
				transform.position += new Vector3(Random.Range(-rumbleAmount,rumbleAmount) * Time.deltaTime,Random.Range(-rumbleAmount,rumbleAmount) * Time.deltaTime,0.0f);	//move randomly
				transform.position = new Vector3(Mathf.Clamp(transform.position.x,Xmin,Xmax) ,Mathf.Clamp(transform.position.y,Ymin,Ymax),transform.position.z);//stay in bounds
				rumbleTimer -= Time.deltaTime;//subtract timer
			}
=======
			
			transform.position = Vector3.Lerp(transform.position , new Vector3(target.position.x , target.position.y + cameraHeight ,
				target.position.z - cameraDistance), cameraSpeed);
>>>>>>> a lot of stuff
		}
	}

	public void Rumble (float amount, float time){
		rumbleAmount = amount;
		rumbleTimer = time;
	}
}
