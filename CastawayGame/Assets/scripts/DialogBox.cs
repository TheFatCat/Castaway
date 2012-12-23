using UnityEngine;
using System.Collections;

public class DialogBox : MonoBehaviour {
 	string text = "place your text here buddy";
	int textLengthShown = 0;
	public Vector2 windowSizePercent;
	Rect windowRect;
	public float wait = 0.2f ;
	float time = 0f;
	int dialogNum =0;
	public DialogSegment[] dialogs;
	public float faceHeight = 0.1f;
		 
	// Use this for initialization
	void Start () {
		text = dialogs[0].dialog;
	}
	
	// Update is called once per frame
	void Update () {
		if(textLengthShown < text.Length){
			if(Input.anyKeyDown){
				textLengthShown = text.Length;
			}
			else{
				time += Time.deltaTime;
				if(time >= wait){
					time = 0f;
					textLengthShown ++;
				}
			}
		}
		else if(Input.anyKeyDown){
			
			textLengthShown = 0;
			dialogNum ++;
			if(dialogNum >= dialogs.Length){
				Destroy(this);
			}
			else{
				text = dialogs[dialogNum].dialog;
			}
		}
	}
	
	void OnGUI(){
		windowRect = new Rect(Screen.width * ((1 -windowSizePercent.x)/2 ),Screen.height * (1- windowSizePercent.y),
			Screen.width * windowSizePercent.x, Screen.height * windowSizePercent.y - 10);

		GUI.Window(0, windowRect,dialogWindow, "");
	}
	
	void dialogWindow(int id){
		switch(dialogs[dialogNum].position){
			case DialogPosition.LEFT :
				GUI.DrawTexture(new Rect(0,0,Screen.width * faceHeight, Screen.height * faceHeight), dialogs[dialogNum].face,ScaleMode.StretchToFill);
				break;
			case DialogPosition.MIDDLE:
				GUI.DrawTexture(new Rect( Screen.width / 2 - (faceHeight * Screen.width) /2 ,0,Screen.width * faceHeight, Screen.height * faceHeight), dialogs[dialogNum].face,ScaleMode.StretchToFill);
				break;
			case DialogPosition.RIGHT:
				GUI.DrawTexture(new Rect(Screen.width *(windowSizePercent.x - faceHeight),0,Screen.width * faceHeight, Screen.height * faceHeight), dialogs[dialogNum].face,ScaleMode.StretchToFill);
				break;
		}
		Rect textRect = new Rect(0,Screen.height * faceHeight, Screen.width * windowSizePercent.x , Screen.height * (windowSizePercent.y-faceHeight) - 10);
		GUI.TextArea(textRect, text.Substring(0,textLengthShown));
	}
	
	[System.Serializable] public class DialogSegment{
		public Texture face;
		public string dialog;
		public DialogPosition position;
	}
	
	public enum DialogPosition{
		LEFT, RIGHT, MIDDLE
	}
}
