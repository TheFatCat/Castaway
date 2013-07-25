using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class CutsceneVisibility : CutSceneElement
{
	[SerializeField]
	private bool visibility;
	[SerializeField]
	Renderer renderer;
	
	public CutsceneVisibility(){
		noDuration = true;
	}
	public override void ActionLogic(float deltaTime){
		if(renderer.enabled != visibility){
			Debug.Log("renderer enabled " + visibility);
			renderer.enabled = visibility;
		}
	}
	
	public override void DrawGUI(){
#if UNITY_EDITOR
		base.DrawGUI();
		renderer = EditorGUILayout.ObjectField("Game Object",renderer,(typeof(Renderer)),true) as Renderer;
		visibility = EditorGUILayout.Toggle("Set Visibility True/False",visibility);
#endif
	}
}

