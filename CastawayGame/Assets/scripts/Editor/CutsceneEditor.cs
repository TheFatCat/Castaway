using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
[CustomEditor (typeof(CutScene))]
public class CutsceneEditor : Editor{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	int curentlySelected =0; 
	public override void OnInspectorGUI(){
		string[] elementTypes = new string[]{"Move" , "InstantiateDestroy"}; 
		
		CutScene cutScene = (CutScene) target;
		cutScene.StartCutscene();
		//cutScene.InitForEditor(); 
		//EditorGUILayout.IntPopup(
		curentlySelected = EditorGUILayout.Popup(curentlySelected,elementTypes);
		
		foreach(CutSceneElement element in cutScene.getInactiveElements()){
			//if(element != null)
			element.DrawGUI();
			EditorGUILayout.LabelField("##############################");

			
		}
		Rect buttonRect = EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField("Add new element");
		if(GUI.Button(buttonRect, "Add Element")){
			//cutScene.addElement(new Move());
			switch(curentlySelected){
			case 0:
				cutScene.addElement(new Move());
				break;
			case 1:
				cutScene.addElement(new CutsceneInstantiateDestroy());
				break;
			default:
				break;
			}
		}
		EditorGUILayout.EndVertical();
		
		buttonRect = EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField("clearelements");

		if(GUI.Button(buttonRect, "Remove Element")){
			CutSceneElement element = cutScene.getInactiveElements().ToArray()[cutScene.getInactiveElements().Count -1];
			//cutScene.getInactiveElements().Remove(element);
			//DestroyImmediate(element);
			cutScene.inactiveMoves.Clear();
		}
		EditorGUILayout.EndVertical();
		if(GUI.changed){
			EditorUtility.SetDirty (target);
		}
	}
	
	
}
