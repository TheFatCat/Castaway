using UnityEngine;
using System.Collections;
using UnityEditor;
[System.Serializable]
public class CutsceneInstantiateDestroy : Move
{
	public float waitTime;
	public InstantiateDestroyType instantiateDestroyType;
	public GameObject gameObject;
	public Vector3 position;
	public Quaternion rotation;
	float timer = 0;
	public override void ActionLogic(float deltaTime){
		timer += Time.deltaTime;
		if(timer > waitTime){
			switch(instantiateDestroyType){
			case InstantiateDestroyType.INSTANTIATE :
				MonoBehaviour.Instantiate(gameObject,position,rotation);
				break;
			case InstantiateDestroyType.DESTROY:
				MonoBehaviour.Destroy(gameObject);
				break;
			default:
			break;
			}
		}
	}
	
	public override void DrawGUI(){
		EditorGUILayout.TextArea("this is just a simple test to \n see if instantiate gui works");
	}
	[System.Serializable]
	public enum InstantiateDestroyType{
		INSTANTIATE,
		DESTROY
	}
}

