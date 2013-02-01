using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	[SerializeField] float scaleX = 1f;
	[SerializeField] float scaleY = 1f;
	[SerializeField] int coins;
	List<Item> items = new List<Item>();
	
	public GUISkin skin;
	// Use this for initialization
	void Start () {
		
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
			GUI.Window(1,new Rect(0,0, getXPixels(800), getYPixels(600)), mainWindow, "");
			//GUI.DrawTexture( new Rect(0,0, getXPixels(800) ,  getYPixels(600) ) , window);
		}
	}
	
	
	void mainWindow(int id){
		GUI.Window(1,new Rect(0,0, getXPixels(800), getYPixels(600)), weaponMenu, "");
		GUI.Window(1,new Rect(0,0, getXPixels(800), getYPixels(600)), itemMenu, "");
		GUI.Window(1,new Rect(0,0, getXPixels(800), getYPixels(600)), equippedMenu, "");
		GUI.Window(1,new Rect(0,0, getXPixels(800), getYPixels(600)), descriptionMenu, "");
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
