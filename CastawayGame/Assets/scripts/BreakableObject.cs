using UnityEngine;
using System.Collections;
[RequireComponent (typeof(Status))]
public class BreakableObject : MonoBehaviour {
	 private Status status ;
	[SerializeField] private BoxCollider[] boxCollidersDisable;
	[SerializeField] private BoxCollider[] boxCollidersEnable;
		
		
	void Start(){
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
