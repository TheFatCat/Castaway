using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class CutsceneWaitForInput : CutSceneElement {
	[SerializeField]
	string message = "";
	GameObject guiText = null;
	public CutsceneWaitForInput(){
		noDuration = true;
		stopTimer = true;
	}
	public override void ActionLogic(float deltaTime){
		noDuration = true;
		if(guiText == null){
			Debug.Log("GUITEXT INSTAntiating");
			guiText = new GameObject();
			GUIText text = guiText.AddComponent((typeof(GUIText))) as GUIText;
			text.text = message;
			text.pixelOffset = new Vector2(Screen.width / 2 , Screen.height / 2);
		}
		if(Input.anyKeyDown){
			noDuration = false;
			stopTimer = false;
			MonoBehaviour.Destroy(guiText);
		}
	}
	
	public override void DrawGUI ()
	{
#if UNITY_EDITOR
		base.DrawGUI ();
		message = EditorGUILayout.TextField("Message",message);
#endif
	}
	
}
