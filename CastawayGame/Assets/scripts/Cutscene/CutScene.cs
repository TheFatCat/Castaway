using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Class for doing in game cutscenes
public class CutScene : MonoBehaviour {
	
	//this field is where all the cutscene segments are stored each segment will store the individual actions during the segment and the duration
	private bool start = false;
	private float timer = 0;
	[SerializeField]
	public List<Move> inactiveMoves ;
	[SerializeField]
	public List<CutsceneInstantiateDestroy> inactiveInstantiateDestroys;
	[SerializeField]
	private List<CutSceneElement> inactiveCutsceneElements ; 
	[SerializeField]
	private List<CutSceneElement> activeCutSceneElements;
	[SerializeField]
	bool startedEditor = false;
	
	// Use this for initialization
	void Start(){
		Debug.Log("cutscene size " + inactiveCutsceneElements.Count);
		Debug.Log("move size " + inactiveMoves.Count);
		//startedEditor = true;
		StartCutscene();
		/*
		Debug.Log("Now");
		Debug.Log("cutscene size " + inactiveCutsceneElements.Capacity);
		Debug.Log("move size " + inactiveMoves.Capacity);
		//if(inactiveMoves.Count != inactiveCutsceneElements.Count){
		//inactiveCutsceneElements.Clear();
		//inactiveMoves.Clear();
		inactiveMoves.TrimExcess();
		inactiveCutsceneElements.TrimExcess();
		foreach(CutSceneElement element in inactiveCutsceneElements){
			Debug.Log("value " + element);
		}
		foreach(CutSceneElement element in inactiveMoves){
			Debug.Log("value " + element);
		}*/
		//}
	}
	/*
	public void InitForEditor(){
		if(!startedEditor){
			startedEditor = true;
			Debug.Log("init for editor");
			inactiveCutsceneElements = new List<CutSceneElement>();
			activeCutSceneElements = new List<CutSceneElement>();
		}
	} */
	
	public void OnEnable(){
		Debug.Log("Enabled Cutscene");
		if(inactiveCutsceneElements == null){
			Debug.Log("new Arraylist");
			inactiveCutsceneElements = new List<CutSceneElement>();
			activeCutSceneElements = new List<CutSceneElement>();
		}
		if(inactiveMoves == null){
			inactiveMoves = new List<Move>();
			inactiveInstantiateDestroys = new List<CutsceneInstantiateDestroy>();
		}
	}
	
	public List<CutSceneElement> getInactiveElements(){
		return inactiveCutsceneElements;
	}
	
	public void StartCutscene(){
		inactiveCutsceneElements.Clear();
		
		inactiveCutsceneElements.AddRange(inactiveMoves.ToArray());				
		inactiveCutsceneElements.AddRange(inactiveInstantiateDestroys.ToArray());
		

		start = true;
	}
	public void addElement(CutSceneElement element){
		if(element.GetType() ==  (typeof( Move))){
			inactiveMoves.Add((Move) element);
		}
		else if(element.GetType() ==  (typeof( CutsceneInstantiateDestroy))){
			inactiveInstantiateDestroys.Add (new CutsceneInstantiateDestroy());
		}
		inactiveCutsceneElements.Add(element);	
		
	}
	
	public void removeElement(CutSceneElement element){
		if(element.GetType() ==  (typeof( Move))){
			inactiveMoves.Remove((Move) element);
		}
		inactiveCutsceneElements.Remove(element);	
	}
	
	
	void Update () {
		//Debug.Log("Update");
		if(start){
		//	Debug.Log("Has Started");
			timer += Time.deltaTime;
			for(int i = 0; i < inactiveCutsceneElements.Count; i ++){
				CutSceneElement element = inactiveCutsceneElements.ToArray()[i];
				//Debug.Log("inactive " + element.ToString());
				if(element.getStartTime() <= timer){
					i --;
					activeCutSceneElements.Add(element);
					inactiveCutsceneElements.Remove(element);
				}
			}
			for(int i = 0; i < activeCutSceneElements.Count; i ++){
				CutSceneElement element = activeCutSceneElements.ToArray()[i];
				if(element.getStartTime() + element.getDuration() <= timer){
					i --;
					activeCutSceneElements.Remove(element);
					//Destroy(element);
				}
				else{
					element.ActionLogic(Time.deltaTime);
				}
			}
		}
	}
	
	// the segment of the cutscene that holds multiple elements
	/*
	[System.Serializable]
	public class CutSceneSegment : Object{
		public float duration = 5;
		public List<CutSceneElement> elements = new List<CutSceneElement>();
		
		public GameObject dialogObject;
		DialogBox dialogue;
		bool instantiated;
		public CutsceneInstantiateDestroy[] instantiateDestroys;
		public void RunElements(){
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
	
	*/
	
	
	
	
	
}
