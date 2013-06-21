/// <summary>
/// Instantiate test.
/// This code is used in the game for instantiating objects for testing
/// something will be instantiated whenever you type space
/// </summary>


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
