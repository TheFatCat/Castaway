using UnityEngine;

using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class Move  : CutSceneElement{
	public Transform actor;
	public Vector3 direction;
	public float speed;
	
	
	public  override void ActionLogic(float deltaTime){
		//Debug.Log(deltaTime);
		if(actor != null){
			actor.Translate(direction * speed * deltaTime);
		}
	}
	
	public override void DrawGUI(){
		#if UNITY_EDITOR
		base.DrawGUI();
		setDuration(EditorGUILayout.FloatField("Duration",getDuration()));
		actor = EditorGUILayout.ObjectField("Target", actor,(typeof(Transform))) as Transform;
		direction = EditorGUILayout.Vector3Field("Direction",direction);
		speed = EditorGUILayout.FloatField("Speed",speed);
		#endif
	}
	
	
}




