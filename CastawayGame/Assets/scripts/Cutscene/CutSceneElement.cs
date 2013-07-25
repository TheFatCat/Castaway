using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor ;
#endif

//This class is is the base class for all subclasses to be used with the cutscene class
//The purpose of cutscene elements is to run certain logic at a specific time in a cutscene
[System.Serializable]
public class  CutSceneElement {
	[SerializeField]
	private float startTime = 0;//when the element starts relative to when the cutscene started
	[SerializeField]
	protected float duration = 0;// how long the element should last after it starts
	[SerializeField]
	public bool noDuration = false;// for objects that have no duration and only have an even when they start or stop time
	[SerializeField]
	public bool stopTimer = false;// if the element should stop the timer on the cutscene like a dialogue box or wait for player input
	/* I dont remember why this is here
	public void OnEnable(){
		//DestroyImmediate(this);
		//hideFlags = HideFlags.HideAndDontSave;
	}
	*/
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
	//this is to overridden in sub classes to do whatever logic should be done when element has started
	public virtual void ActionLogic(float deltaTime){
	}
	//this method is for drawing th gui in edit mode to edit each specific cutscene element
  	public virtual void DrawGUI(){
#if UNITY_EDITOR
				setStartTime(EditorGUILayout.FloatField("StartTime",getStartTime())); 
#endif
	}
			
		
}

