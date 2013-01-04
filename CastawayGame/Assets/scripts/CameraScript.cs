using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public Transform target;
	public float cameraDistance = 29.7299f;
	public float cameraHeight = 16.1712f;
	public float cameraSpeed = 1f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(target != null){
			
			transform.position = new Vector3(target.position.x , target.position.y + cameraHeight , target.position.z - cameraDistance); 
		}
	}
}
