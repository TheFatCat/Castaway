using UnityEngine;
using System.Collections;

public class CutSceneElement : MonoBehaviour{
		public bool started = false;
		
		public void StartElement(){
			started = true;	
				
		}
		
		public void Action(){
			if(started){
				ActionLogic();
			}
		
		}
	
	
		public virtual void ActionLogic(){
		}
			
		
	}

