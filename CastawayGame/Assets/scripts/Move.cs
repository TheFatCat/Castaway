using UnityEngine;

using System.Collections;
[System.Serializable]
public class Move  : CutSceneElement{
	public Transform actor;
	public Vector3 direction;
	public float speed;
	
	
	public void ActionLogic(){
		
		actor.position = Vector3.MoveTowards(actor.position, actor.position + direction,speed * Time.deltaTime);
	}
	
	
}




