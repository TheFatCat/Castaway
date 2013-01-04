using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public Transform target;
<<<<<<< HEAD
	public float cameraDistance = 10f;
	public float cameraHeight = 10f;
	public float cameraSpeed = 1f;
=======
	public float cameraDistance = 29.729f;
	public float cameraHeight = 16.17124f;
>>>>>>> fixed the camera..?
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(target != null){
<<<<<<< HEAD
			
			transform.position = Vector3.Lerp(transform.position , new Vector3(target.position.x , target.position.y + cameraHeight , target.position.z - cameraDistance), cameraSpeed);
=======
			transform.position = new Vector3(target.position.x , target.position.y + cameraHeight , target.position.z - cameraDistance);
>>>>>>> fixed the camera..?
		}
	}
}
