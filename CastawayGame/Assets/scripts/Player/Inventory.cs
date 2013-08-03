using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	[SerializeField] float scaleX = 1f;
	[SerializeField] float scaleY = 1f;
	[SerializeField] int coins;
	List<Item> items = new List<Item>();
	WeaponImplementer wp;
	List<Weapon> weapons;
	// these three booleans will be used to decide which section of the inventory the player is using
	bool onWeaponBox = true;
	bool onItemBox = false;
	bool onEqupiiedItemsBox = false;
	//three textures for the weapon box
	public Texture activeWeaponTexture;
	public Texture inactiveWeaponTexture;
	public Texture weaponTextureBackground;
	Rect weaponRect;
	// texture for the description box
	public Texture descriptionTexture;
	Rect descriptionRect;
	// three textures for the item box
	public Texture activeItemTexture;
	public Texture inactiveItemTexture;
	public Texture itemTexturebackground;
	Rect itemRect;
	// three textures for the equipped items box 
	public Texture activeEquippedItemTexture;
	public Texture inactiveEquippedItemTexture;
	public Texture equippedItemBackground;
	Rect equippedItemRect;
	
	public Rect objectSlot;
	
	public GUISkin skin;
	// Use this for initialization
	void Start () {
		//weapons = wp.getWeapons();

	}
	
	public void addCoins(int val ){
		if(val < 0){
			return;
		}
		
		coins += val;
	}
	
	
	public void subtractCoins(int val){
		if(val < 0){
			return;
		}
		
		coins -= val;
	}
	
	public int getCoins(){
		return coins;
	}
	
	
	
	//GUI
	
	[SerializeField] Texture window;
	private bool inventoryIsOpen;
	
	
	// Update is called once per frame
	void Update () {
		//starting box's for the GUI
		weaponRect = new Rect(getXPixels(20), getYPixels(50), getXPixels(510), getYPixels(175));
		//starting box for the description box
		descriptionRect = new Rect(getXPixels(540), getYPixels(50), getXPixels(240), getYPixels(550));
		//starting box for the item box
		itemRect = new Rect(getXPixels (20), getYPixels(230), getXPixels(510), getYPixels(235));
		//starting box for the equiped items box
		equippedItemRect = new Rect(getXPixels(20), getYPixels(470), getXPixels(510),getYPixels(120));

		if(Input.GetKeyDown(KeyCode.I)){
			inventoryIsOpen = !inventoryIsOpen;
			
			if(inventoryIsOpen){
				Time.timeScale = 0;
				GetComponent<PlayerController>().frozen = true;
				GetComponent<WeaponImplementer>().canShoot = false;
			}
			else{
				currentScaleX = 0; 
				currentScaleY = 0 ;
				Time.timeScale = 1;
				GetComponent<PlayerController>().frozen = false;
				GetComponent<WeaponImplementer>().canShoot = true;
			}
			
		}
	}
	
	
	
	private float currentScaleX = 0;
	private float currentScaleY = 0;
	void OnGUI(){
		if(inventoryIsOpen){
			//GUI.skin = skin;
			currentScaleX = Mathf.Lerp(currentScaleX , scaleX , 0.1f);
			currentScaleY = Mathf.Lerp(currentScaleY, scaleY , 0.1f);
			// item description background
				GUI.DrawTexture(descriptionRect, descriptionTexture);
			// the following if statements will put the active tuxtures forward and the inactive textures back.
			if(onWeaponBox == true ){
				GUI.DrawTexture(weaponRect, activeWeaponTexture);
				GUI.DrawTexture(itemRect, inactiveItemTexture);
				GUI.DrawTexture(equippedItemRect, inactiveEquippedItemTexture);
			}else if( onItemBox == true){
				GUI.DrawTexture(weaponRect, inactiveWeaponTexture);
				GUI.DrawTexture(itemRect, activeItemTexture);
				GUI.DrawTexture(equippedItemRect, inactiveEquippedItemTexture);	
			}else if(onEqupiiedItemsBox == true){
				GUI.DrawTexture(weaponRect, inactiveWeaponTexture);
				GUI.DrawTexture(itemRect, inactiveItemTexture);
				GUI.DrawTexture(equippedItemRect, activeEquippedItemTexture);
			}
			
		}
	}
	
	
	void mainWindow(int id){
	}
	
	void equippedMenu(int id){
	}
	void descriptionMenu(int id){
	}
	void weaponMenu(int id){
	}
	
	void itemMenu(int id){
	}
	
	
 	int getXPixels(float size){
		return(int)(size * currentScaleX * (Screen.width / 800f));
	}
	
	int getYPixels(float size){
		return(int)(size * currentScaleX * (Screen.height/ 600f));
	}
	
	
	[System.Serializable] 
	public class Item {
		public Texture icon;
		public string description;
		
	}
}
