using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class  CutSceneElement {
	[SerializeField]
	private float startTime = 0;
	[SerializeField]
	private float duration = 0;
	
	
	public void OnEnable(){
		Debug.Log("Enabled"+ToString());
		//DestroyImmediate(this);
		//hideFlags = HideFlags.HideAndDontSave;
	}
	public void setStartTime(float time){
		startTime  =time;
	}
	public float getStartTime(){
		return startTime;
	}
	
	public void setDuration(float duration){
		this.duration = duration;
	}
	public float getDuration(){
		return duration;
	}
	
	public virtual void ActionLogic(float deltaTime){
	}
  	public virtual void DrawGUI(){
	}
			
		
}

