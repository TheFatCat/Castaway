using UnityEngine;
using System.Collections;

public class InstantiateTest : MonoBehaviour {
	public GameObject objectForInstantiation;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			Instantiate (objectForInstantiation);
		}
	}
}
