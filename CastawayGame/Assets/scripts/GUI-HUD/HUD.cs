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
	[SerializeField] private GUIText healthText;
	[SerializeField] private Font healthFont;
	[SerializeField] private Font healthFlashFont;
	[SerializeField] private GUIText ammoText;



	private Vector2 scaleImage = new Vector2(1,1);
	private PlayerStatus playerStatus;
	private WeaponImplementer playerWeapon;
	private Inventory inventory;
	//private WeaponImplementer weaponImplementer;
	DrawBars drawBars ;
	public float flashTime = 0.1f;
	private float timer = 5000f;

	
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
		//weaponImplementer = PlayerController.getPlayer().GetComponent<WeaponImplementer>();	
		drawBars = GetComponent<DrawBars>();
		
	}
	
	
	public void hideHUD(){
		pullUp = true;
	} 
	public void unhideHUD(){
		pullUp = false;
	}

	public void hit(){
		timer = 0.0f;
	}


	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
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
		//Draw Health numbers
		//GUI.Label (getRect (235,-10,120,40), playerStatus.getHealth () + "", healthStyle);


		//Draw Health
		if(playerStatus.getHealth() >= 0){
			//draw health numbers
			healthText.pixelOffset = new Vector2 (drawBars.getBarWidth() + scaleImage.x * 235, (550 * scaleImage.y)+ 60);
			healthText.text = playerStatus.getHealth () + "";

			if(timer < flashTime){	//we're in the zone!
				GUI.DrawTexture( getRect(55,14, 175 * ((float) playerStatus.getHealth() / playerStatus.getMaxHealth()) , 28 ),healthFlash);
			}else{
				GUI.DrawTexture( getRect(55,14, 175 * ((float) playerStatus.getHealth() / playerStatus.getMaxHealth()) , 28 ),healthBar);
			}

		}
		//Draw Health Background
		GUI.DrawTexture(getRect (55,12, 175  , 32 ), healthBarBackground);

		//only draw ammo if we have a gun
		if(playerWeapon.getMaxAmmo() != 0){	//neeed to find a better way to check whether we have a weapon
			//Draw Bullets
			GUI.DrawTexture (getRect (736, 0, 64, 64), ammoIcon);
			//Draw Ammo
			if(playerWeapon.getAmmo() > 0){	
				GUI.DrawTexture( getRect(745,14, -175 * ((float) playerWeapon.getAmmo() / playerWeapon.getMaxAmmo()) , 28 ),ammoBar);
				//draw numbers
				//GUI.Label (getRect (445,-10,120,60), playerWeapon.getAmmo () + "", ammoStyle);

				ammoText.pixelOffset = new Vector2 (drawBars.getBarWidth() + scaleImage.x * 565, (550 * scaleImage.y)+ 60);
				ammoText.text = playerWeapon.getAmmo () + "";
			}
			//Draw Ammo Background
			GUI.DrawTexture(getRect (570,12, 175  , 32 ), ammoBarBackground);
			//Draw Weapon Icon
			GUI.DrawTexture(getRect(340, 0 , 50, 50) , playerWeapon.getCurrentWeapon().weaponIcon);
			GUI.DrawTexture(getRect(410, 0 , 50, 50) , playerWeapon.getCurrentWeapon().weaponIcon);
		}
		//GUI.TextArea(getRect(5 , 70, 50,50) , playerStatus.getHealth() + " / " + playerStatus.getMaxHealth(), guistyle); 

		//GUI.TextArea(getRect(745, 0, 50, 50), "" +inventory.getCoins());
		/*
		for(int i = 0; i < weaponImplementer.getWeapons().Size(); i ++){
		}*/

		     
	}
	
	
	Rect getRect(float left, float top, float width, float height){
		return new Rect(drawBars.getBarWidth() + scaleImage.x * left, scaleImage.y * top - pullupScale * (scaleImage.y * (top + height)), scaleImage.x * width , scaleImage.y * height);
	}
}
