using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	
	[SerializeField] int coins;
	List<Item> items = new List<Item>();
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
				Time.timeScale = 1;
				GetComponent<PlayerController>().frozen = false;
				GetComponent<WeaponImplementer>().canShoot = true;
			}
			
		}
	}
	
	void OnGUI(){
		if(inventoryIsOpen){
			
			
			//GUI.DrawTexture( new Rect(0,0,640, 480) , window);
		}
	}
	
	
	
	
	
	
	
	[System.Serializable] 
	public class Item {
		public Texture icon;
		public string description;
		
	}
}
