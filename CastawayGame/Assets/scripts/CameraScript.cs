using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public Transform target;
	public float cameraDistance = 10f;
	public float cameraSpeed = 1f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(target != null){
			
			transform.position = Vector3.Lerp(transform.position , new Vector3(target.position.x , target.position.y + 4 , target.position.z - cameraDistance), cameraSpeed);
		}
	}
}
