using UnityEngine;

using System.Collections;

public class Move  : CutSceneElement{
	public Transform actor;
	public float speed;
	void Update(){
		this.Action();
	}
	
	public override void ActionLogic(){
		Debug.Log(transform.name + " moving");
		actor.position = Vector3.MoveTowards(actor.position, transform.position,speed * Time.deltaTime);
	}
	
	
}




