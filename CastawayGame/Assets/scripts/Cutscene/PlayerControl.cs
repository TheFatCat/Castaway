using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class PlayerControl : CutSceneElement
{
	
	private int i = 0;
	int moveDirection = 0;
	string[] Options = new string[]{"Horizontal","Vertical","Jump","Shoot"}; 
	public override void ActionLogic (float deltaTime)
	{
		PlayerController player = PlayerController.getPlayer().GetComponent<PlayerController>();
		if(i == 0){
			player.SetInput(moveDirection,0,false);
		}
		else if(i == 1){
			player.SetInput(0,moveDirection,false);
		}
		else if( i == 2){
			player.SetInput(0,0,true);
		}
	}
	
	
	public override void DrawGUI ()
	{
#if UNITY_EDITOR
		base.DrawGUI ();
		i = EditorGUILayout.Popup(i,Options);
		if(i < 2){
			moveDirection = EditorGUILayout.IntField("MoveDirection",moveDirection);
			moveDirection = Mathf.Clamp(moveDirection,-1,1);
			setDuration(EditorGUILayout.FloatField("Duration",getDuration()));
		}
#endif
	}
}

