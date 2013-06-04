using UnityEngine;
using System.Collections;

public class CutSceneElement : MonoBehaviour{
		public bool started = false;
		public float duration ;
		public void StartElement(){
			started = true;	
				
		}
		
		public void Action(){
			if(started){
				ActionLogic();
				
				duration -= Time.deltaTime;
				if(duration < 0){
				Destroy(gameObject);
				}
			}
		
		}
	
	
		public virtual void ActionLogic(){
		}
			
		
	}

