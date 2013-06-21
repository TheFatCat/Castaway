using UnityEngine;
using System.Collections;

public class TextRise : MonoBehaviour {

	public float distance = 1.0f;
	public float accel = 1.0f;


	void Update () {
		transform.position = new Vector3(transform.position.x,Mathf.Lerp(transform.position.y,transform.position.y + distance,accel * Time.deltaTime),transform.position.z);
		if (distance > 0.0f) {	
			distance -= accel * Time.deltaTime;
		}
	}
}
