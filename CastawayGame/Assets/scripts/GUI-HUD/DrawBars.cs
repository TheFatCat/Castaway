using UnityEngine;
using System.Collections;

public class DrawBars : MonoBehaviour {
	[SerializeField] Texture bar;
	int barWidth;
	void OnGUI(){
		
		float ratio = ((float)Screen.width) / Screen.height ;
		if( ratio > 1.3333f){
			barWidth = (int)((ratio - 1.3333f) * Screen.height * 0.5f);
			GUI.DrawTexture(new Rect(0,0, barWidth, Screen.height) , bar );
			GUI.DrawTexture(new Rect(Screen.width - barWidth,0, barWidth, Screen.height) , bar );
		}
	}
	
	
	public int getBarWidth(){
		return barWidth;
	}
}
