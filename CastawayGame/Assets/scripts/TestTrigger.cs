using UnityEngine;
using System.Collections;

public class TestTrigger : MonoBehaviour {
	int enter = 0;
	int exit = 0;
	void OnTriggerEnter(Collider collider){
		if(collider.GetComponent<PlayerController>() == null){
			enter ++;
			Debug.Log("enter " + enter);
		}
	}
	
	void OnTriggerExit(Collider collider){
		exit ++;
		Debug.Log("exit " + exit);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
