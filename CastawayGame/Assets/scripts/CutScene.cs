using UnityEngine;
using System.Collections;


//Class for doing in game cutscenes
public class CutScene : MonoBehaviour {
	
	//this field is where all the cutscene segments are stored each segment will store the individual actions during the segment and the duration
	[SerializeField] public CutSceneSegment[] cutSceneSegments;
	
	// Use this for initialization
	void Start () {
		if(cutSceneSegments.Length > 0){
			//PlayerController.getPlayer().GetComponent<PlayerController>().SetFrozen(true);
			//EnemyController.freezeAllEnemies();
			cutSceneSegments[0].Start();
		}
		else{
			Destroy(gameObject);
		}
	}
	
	int segmentNumber = 0;
	// Update is called once per frame
	void Update () {
		if(cutSceneSegments[0] != null){
			if(cutSceneSegments[segmentNumber].CheckTime()){
				segmentNumber ++;
				if(segmentNumber > cutSceneSegments.Length -1){
					Destroy(gameObject);
				}
				else{
				cutSceneSegments[segmentNumber].Start();
				}
			}
		}
		
	}
	
	// the segment of the cutscene that holds multiple elements
	[System.Serializable]
	public class CutSceneSegment{
		public float duration;
		public CutSceneElement[] elements;
		public void Start(){
			foreach(CutSceneElement element in elements){
				element.StartElement();
			}
		}
		public bool CheckTime(){
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