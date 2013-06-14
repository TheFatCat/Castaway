using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Class for doing in game cutscenes
public class CutScene : MonoBehaviour {
	
	//this field is where all the cutscene segments are stored each segment will store the individual actions during the segment and the duration
	[SerializeField] public CutSceneSegment[] cutSceneSegments;
	
	// Use this for initialization
	void Start () {
		if(cutSceneSegments.Length > 0){
			//PlayerController.getPlayer().GetComponent<PlayerController>().SetFrozen(true);
			//EnemyController.freezeAllEnemies();
			
		}
		else{
			Destroy(gameObject);
		}
	}
	
	int segmentNumber = 0;
	// Update is called once per frame
	void Update () {
		if(cutSceneSegments[0] != null){
			cutSceneSegments[segmentNumber].RunElements();
			if(cutSceneSegments[segmentNumber].CheckTime()){
				segmentNumber ++;
				Debug.Log("next segment " + segmentNumber);
				if(segmentNumber > cutSceneSegments.Length -1){
					Destroy(gameObject);
				}
				
			}
		}
		
	}
	
	// the segment of the cutscene that holds multiple elements
	[System.Serializable]
	public class CutSceneSegment{
		public float duration;
		public Move[] moveElements;
		
		public GameObject dialogObject;
		DialogBox dialogue;
		bool instantiated;
		public CutsceneInstantiateDestroy[] instantiateDestroys;
		public void RunElements(){
			
			List<CutSceneElement> elements = new List<CutSceneElement>();
			elements.AddRange(moveElements);
			elements.AddRange(instantiateDestroys);
			
			
			if(dialogObject != null && !instantiated){
				instantiated = true;
				GameObject dialogInstance = MonoBehaviour.Instantiate(dialogObject) as GameObject ;
				Debug.Log(dialogInstance);
				dialogue = dialogInstance.GetComponent<DialogBox>();
			}
			
			foreach(CutSceneElement element in elements){
				element.ActionLogic();
			}
		}
		public bool CheckTime(){
			if(dialogue != null){
				return false;	
			}
			else{
				duration -= Time.deltaTime;
				if(duration < 0){
					return true;
				}
				else{
					return false;
				}
			}
		}
	}
	
	
	
	
	
	
	
}
