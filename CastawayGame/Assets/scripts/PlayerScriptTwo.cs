using UnityEngine;
using System.Collections;

public class PlayerScriptTwo : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		 GetComponent<CharacterController>().Move(new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0) * Time.deltaTime);
	}
}
