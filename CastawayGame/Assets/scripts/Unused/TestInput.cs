using UnityEngine;
using System.Collections;

public class TestInput : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetButtonDown("Horizontal")){
			if(Input.GetAxis("Horizontal") > 0){
				Debug.Log("Right");
			}
			else{
				Debug.Log("Left");
			}
		}
	}
}

