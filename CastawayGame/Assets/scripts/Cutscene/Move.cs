using UnityEngine;

using System.Collections;
using UnityEditor;
[System.Serializable]
public class Move  : CutSceneElement{
	public Transform actor;
	public Vector3 direction;
	public float speed;
	
	
	public  override void ActionLogic(float deltaTime){
		if(actor != null){
			actor.Translate(direction * speed * deltaTime);
		}
	}
	
	public override void DrawGUI(){
		setStartTime(EditorGUILayout.FloatField("StartTime",getStartTime())); 
		setDuration(EditorGUILayout.FloatField("Duration",getDuration()));
		actor = EditorGUILayout.ObjectField("Target", actor,(typeof(Transform))) as Transform;
		direction = EditorGUILayout.Vector3Field("Direction",direction);
		speed = EditorGUILayout.FloatField("Speed",speed);
		
	}
	
	
}




