using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	
	[SerializeField] private Texture healthBarBackground;
	[SerializeField] private Texture healthBar;
	[SerializeField] private Texture healthIcon;
	[SerializeField] private Vector2 scaleImage = new Vector2(1,1);
	private Status playerStatus;
	private WeaponImplementer weaponImplementer;
	DrawBars drawBars ;
	// Use this for initialization
	void Start(){
		drawBars = GetComponent<DrawBars>();	
	}
	
	// Update is called once per frame
	void Update () {
		scaleImage.x = (Screen.width  - (2 * drawBars.getBarWidth()))/ 800f;
		scaleImage.y = Screen.height / 600f;
	}
	
	void OnGUI(){
		if(playerStatus == null){
			playerStatus = PlayerController.getPlayer().GetComponent<Status>();
			weaponImplementer = PlayerController.getPlayer().GetComponent<WeaponImplementer>();	
		}
		GUI.DrawTexture(getRect(0,0, 65 , 65), healthIcon);
		
		GUI.DrawTexture( getRect(75,5, 158 * ((float) playerStatus.getHealth() / playerStatus.getMaxHealth()) , 26 ),healthBar);
		GUI.DrawTexture(getRect (70, 0, 162  , 34 ), healthBarBackground);
		GUI.TextArea(getRect(5 , 70, 50,50) , playerStatus.getHealth() + " / " + playerStatus.getMaxHealth()); 
		
		GUI.DrawTexture(getRect(375, 0 , 50, 50) , weaponImplementer.getCurrentWeapon().weaponIcon);
		
	}
	
	
	Rect getRect(float left, float top, float width, float height){
		return new Rect(drawBars.getBarWidth() + scaleImage.x * left, scaleImage.y * top, scaleImage.x * width , scaleImage.y * height);
	}
}
