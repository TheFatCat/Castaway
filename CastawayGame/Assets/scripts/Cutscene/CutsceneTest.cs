using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CutsceneTest : MonoBehaviour {
	[SerializeField]
	private List<CutSceneElement> cutscenes;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float deltaTime = Time.deltaTime;
		Debug.Log(deltaTime);
		transform.Translate(new Vector3(1,0,0) * deltaTime);
	}
}
