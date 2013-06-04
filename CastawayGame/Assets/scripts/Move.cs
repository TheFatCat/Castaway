using UnityEngine;

using System.Collections;

public class Move  : CutSceneElement{
	public Transform actor;
	public Vector3 direction;
	public float speed;
	void Update(){
		this.Action();
	}
	
	public override void ActionLogic(){
		Debug.Log(transform.name + " moving");
		actor.position = Vector3.MoveTowards(actor.position, actor.position + direction,speed * Time.deltaTime);
	}
	
	
}




