using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	
	[SerializeField] private Texture healthBarBackground;
	[SerializeField] private Texture healthBar;
	[SerializeField] private Texture healthIcon;
	[SerializeField] private Vector2 scaleImage = new Vector2(1,1);
	private PlayerStatus playerStatus;
	private WeaponImplementer weaponImplementer;
	DrawBars drawBars ;
	
	//For pulling the hud up or down when we want to hide it
	float pullupScale = 0;
	bool pullUp = false;
	//***************************************
	
	
	
	// Use this for initialization
	void Start(){
		drawBars = GetComponent<DrawBars>();	
	}
	
	
	public void hideHUD(){
		pullUp = true;
	}
	public void unhideHUD(){
		pullUp = false;
	}
	// Update is called once per frame
	void Update () {
		
		
		
		if(pullUp){
			pullupScale = Mathf.Lerp(pullupScale, 1,0.1f);
		}
		else{
			pullupScale = Mathf.Lerp(pullupScale, 0,0.1f);
		}
		scaleImage.x = (Screen.width  - (2 * drawBars.getBarWidth()))/ 800f;
		scaleImage.y = Screen.height / 600f;
	}
	
	void OnGUI(){
		if(playerStatus == null){
			playerStatus = PlayerController.getPlayer().GetComponent<PlayerStatus>();
			weaponImplementer = PlayerController.getPlayer().GetComponent<WeaponImplementer>();	
		}
		GUI.DrawTexture(getRect(0,0, 65 , 65), healthIcon);
		
		GUI.DrawTexture( getRect(75,5, 158 * ((float) playerStatus.getHealth() / playerStatus.getMaxHealth()) , 26 ),healthBar);
		GUI.DrawTexture(getRect (70, 0, 162  , 34 ), healthBarBackground);
		GUI.TextArea(getRect(5 , 70, 50,50) , playerStatus.getHealth() + " / " + playerStatus.getMaxHealth()); 
		/*
		for(int i = 0; i < weaponImplementer.getWeapons().Size(); i ++){
		}*/
		GUI.DrawTexture(getRect(375, 0 , 50, 50) , weaponImplementer.getCurrentWeapon().weaponIcon);
		
	}
	
	
	Rect getRect(float left, float top, float width, float height){
		return new Rect(drawBars.getBarWidth() + scaleImage.x * left, scaleImage.y * top - pullupScale * (scaleImage.y * (top + height)), scaleImage.x * width , scaleImage.y * height);
	}
}
