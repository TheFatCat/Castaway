using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	
	public enum ButtonType {Start,Credits,Quit};
	
	public ButtonType myType;
	
	void OnMouseDown() {
			switch(myType){
				case(ButtonType.Start):
					Application.LoadLevel("FinalScene");
					break;
				case(ButtonType.Credits):
					Debug.Log("credits...");
					break;
				case(ButtonType.Quit):
					Application.Quit();
					break;
			
			}
		
	}
}
