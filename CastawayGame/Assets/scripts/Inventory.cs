using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	[SerializeField] float scaleX = 1f;
	[SerializeField] float scaleY = 1f;
	[SerializeField] int coins;
	List<Item> items = new List<Item>();
	bool onWeaponBox = true;
	bool onItemBox = false;
	bool onEqupiiedItemsBox = false;
	WeaponImplementer wp;
	List<Weapon> weapons;
	//three textures for the weapon box and the weapon box's starting size
	public Texture activeWeaponTexture;
	public Texture inacvticeWeaponTexture;
	public Texture weaponTextureBackground;
	private Rect weaponRect = new Rect(0, getYPixels(50), getXPixels(500), getYPixels(50));
	// texture for the description box
	public Texture descriptionTexture;
	private Rect descriptionRect = new Rect(getXPixels(50), getYPixels(500), getXPixels(300), getYPixels(550));
	// three textures for the item box
	public Texture activeItemTexture;
	public Texture inactiveItemTexture;
	public Texture itemTexturebackground;
	private Rect itemRect = new Rect(0, getYPixels(100), getXPixels(500), getYPixels(400));
	// three textures for the equipped items box 
	public Texture activeEquippedItemTexture;
	public Texture inactiveEquippedItemTexture;
	public Texture equippedItemBackground;
	private Rect equippedItemRect = new Rect(0, getYPixels(550), getXPixels(500),getYPixels(50));
	
	public GUISkin skin;
	// Use this for initialization
	void Start () {
		weapons = wp.getWeapons;
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
			GUI.skin = skin;
			currentScaleX = Mathf.Lerp(currentScaleX , scaleX , 0.1f);
			currentScaleY = Mathf.Lerp(currentScaleY, scaleY , 0.1f);
			if(onWeaponBox == true && onItemBox == false && onEqupiiedItemsBox == false){
				
			}else if(onWeaponBox == false && onItemBox == true && onEqupiiedItemsBox == false){
				
			}else if(onWeaponBox == false && onItemBox == false && onEqupiiedItemsBox == true){
				
			}
			GUI.DrawTexture(new Rect(0,getXPixels(50), getXPixels(500), getYPixels(50)), weaponTextureBackground);
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
	
	
	int getXPixels(int size){
		return (int)(size * currentScaleX);
	}
	
	int getYPixels(float size){
		return (int)(size * currentScaleX);
	}
	
	
	[System.Serializable] 
	public class Item {
		public Texture icon;
		public string description;
		
	}
}
