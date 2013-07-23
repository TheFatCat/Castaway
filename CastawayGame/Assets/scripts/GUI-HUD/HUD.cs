using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	
	[SerializeField] private Texture healthBarBackground;
	[SerializeField] private Texture healthBar;
	[SerializeField] private Texture healthFlash;	//for when we lose health
	[SerializeField] private Texture healthIcon;
	[SerializeField] private Texture ammoBarBackground;
	[SerializeField] private Texture ammoBar;
	[SerializeField] private Texture ammoIcon;
	private Vector2 scaleImage = new Vector2(1,1);
	private PlayerStatus playerStatus;
	private WeaponImplementer playerWeapon;
	private Inventory inventory;
	private WeaponImplementer weaponImplementer;
	DrawBars drawBars ;
	
	//For pulling the hud up or down when we want to hide it
	float pullupScale = 0;
	bool pullUp = false;
	//***************************************
	
	
	
	// Use this for initialization
	void Start(){
		while(PlayerController.getPlayer() == null){	//<-- this is terrible, why would you ever write this?

		}
		//cache important variables
		inventory = PlayerController.getPlayer().GetComponent<Inventory>();
		playerStatus = PlayerController.getPlayer().GetComponent<PlayerStatus>();
		playerWeapon = PlayerController.getPlayer ().GetComponent<WeaponImplementer> ();
		weaponImplementer = PlayerController.getPlayer().GetComponent<WeaponImplementer>();	
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
		scaleImage.x = (float)(Screen.width  - (2 * drawBars.getBarWidth()))/ 800f;
		scaleImage.y = (float) Screen.height / 600f;
	}
	
	void OnGUI(){
		if(playerStatus == null){
			
			
		}
		//Draw Heart
		GUI.DrawTexture(getRect(0, 0, 64 , 64), healthIcon);
		//Draw Bullets
		GUI.DrawTexture (getRect (736, 0, 64, 64), ammoIcon);
		//Draw Health
		if(playerStatus.getHealth() > 0){
			GUI.DrawTexture( getRect(55,14, 175 * ((float) playerStatus.getHealth() / playerStatus.getMaxHealth()) , 28 ),healthBar);
		}
		//Draw Health Background
		GUI.DrawTexture(getRect (55,12, 175  , 32 ), healthBarBackground);
		//Draw Ammo
		if(playerWeapon.getAmmo() > 0){	
			GUI.DrawTexture( getRect(745,14, -175 * ((float) playerWeapon.getAmmo() / playerWeapon.getMaxAmmo()) , 28 ),ammoBar);
		}
		//Draw Ammo Background
		GUI.DrawTexture(getRect (570,12, 175  , 32 ), ammoBarBackground);

		GUI.TextArea(getRect(5 , 70, 50,50) , playerStatus.getHealth() + " / " + playerStatus.getMaxHealth()); 
		//GUI.TextArea(getRect(745, 0, 50, 50), "" +inventory.getCoins());
		/*
		for(int i = 0; i < weaponImplementer.getWeapons().Size(); i ++){
		}*/
		GUI.DrawTexture(getRect(375, 0 , 50, 50) , weaponImplementer.getCurrentWeapon().weaponIcon);
		     
	}
	
	
	Rect getRect(float left, float top, float width, float height){
		return new Rect(drawBars.getBarWidth() + scaleImage.x * left, scaleImage.y * top - pullupScale * (scaleImage.y * (top + height)), scaleImage.x * width , scaleImage.y * height);
	}
}
