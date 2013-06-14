using UnityEngine;
using System.Collections;
[System.Serializable]
public class CutsceneInstantiateDestroy : CutSceneElement
{
	public float waitTime;
	public InstantiateDestroyType instantiateDestroyType;
	public GameObject gameObject;
	public Vector3 position;
	public Quaternion rotation;
	float timer = 0;
	public  void ActionLogic(){
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
	
	[System.Serializable]
	public enum InstantiateDestroyType{
		INSTANTIATE,
		DESTROY
	}
}

