using UnityEngine;
using System.Collections;
#if UNITY_EDITOR 
using UnityEditor;
#endif
[System.Serializable]
public class CutsceneInstantiateDestroy : CutSceneElement
{
	public float waitTime;
	public GameObject gameObject;
	public Vector3 position;
	bool objectWasInstantiated = false;
	GameObject instantiatedObject ;
	public bool shouldDestroy = false;
	float timer = 0;
	public override void ActionLogic(float deltaTime){
		timer += deltaTime;
		if(timer < waitTime ||  shouldDestroy == false){
			if(! objectWasInstantiated){
				objectWasInstantiated = true;
				instantiatedObject = MonoBehaviour.Instantiate(gameObject,position,Quaternion.identity) as GameObject;
			}
		}
		else{
			if(instantiatedObject != null){
				MonoBehaviour.Destroy(instantiatedObject);
			}
		}
	}
	
	public override void DrawGUI(){
#if UNITY_EDITOR
		base.DrawGUI();
		gameObject =	EditorGUILayout.ObjectField(gameObject,(typeof(GameObject)),true) as GameObject;
		position = EditorGUILayout.Vector3Field("Position" , position);		
		shouldDestroy = EditorGUILayout.Toggle("Should Destroy",shouldDestroy);
		if(shouldDestroy){
			waitTime = EditorGUILayout.FloatField("Time Until Destroyed",waitTime);
		}
		setDuration(waitTime  +0.1f);
#endif
	}
	
}

