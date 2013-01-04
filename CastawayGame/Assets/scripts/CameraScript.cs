using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public Transform target;

	public float cameraDistance = 29.7299f; // distance from z
	public float cameraHeight = 16.1712f;// distance from y
	public float Ymin = 23.9f;
	public float Ymax = 100.0f;
	public float Xmin = -271f;
	public float Xmax = 117f;
	public float XminMoveDist = 15f;
	public float YminMoveDist = 12f;
	public float rumbleAmount = 0.0f;
	public float rumbleTimer = 5.0f;

	


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(target != null){

			

			
			transform.position = new Vector3(Mathf.Clamp(target.position.x,Xmin,Xmax) ,Mathf.Clamp(target.position.y + cameraHeight,Ymin,Ymax), target.position.z - cameraDistance);
			
			
			//if rumbling
			if(rumbleTimer > 0.0f){
				transform.position += new Vector3(Random.Range(-rumbleAmount,rumbleAmount) * Time.deltaTime,Random.Range(-rumbleAmount,rumbleAmount) * Time.deltaTime,0.0f);	//move randomly
				transform.position = new Vector3(Mathf.Clamp(transform.position.x,Xmin,Xmax) ,Mathf.Clamp(transform.position.y,Ymin,Ymax),transform.position.z);//stay in bounds
				rumbleTimer -= Time.deltaTime;//subtract timer
			}

		}
	}

	public void Rumble (float amount, float time){
		rumbleAmount = amount;
		rumbleTimer = time;
	}
}
