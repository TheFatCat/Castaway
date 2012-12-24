using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Inventory : MonoBehaviour {
	int weaponNum = 0;//current weapon player is using
	
	
	
	public ArrayList weaponAttributes = new ArrayList() ;// holds weaponAttributesAttributes; 
	public ArrayList itemAttributes = new ArrayList(); //holds item attributes
	private bool openWindow = false;// should see inventory window
	public GUISkin inventorySkin;//the custom skin we are using
	private RectOffset border; // border of the window 
	public float windowHeight ; // the main windows height as a percentage
	public Vector2 spacing  ; // spacing between edges
	private int freeSpaceX ; // free space for images inside the main window
	private int freeSpaceY ;
	public Vector2 iconSize ; // the size of the icons in the main window
	private Rect windowRect ; //the rect for the main window
	private Rect infoWindowRect; // the rect for the item info window
	private int selectorPosition = 0;
	private int selectorLevel = 0;
	private int textHeight ;
	// Use this for initialization
	void Start () {
		textHeight = 30;
		border = inventorySkin.window.border;
		
	}
	void changeWeapon(){
		
		if(weaponAttributes.Count >0  ){
			Debug.Log("changeing item");
			
			weaponNum ++;
			if(weaponNum >= weaponAttributes.Count){
				Debug.Log("sent back to first weapon");
				weaponNum = 0;
			}
			GetComponent<Weapon>().setAttributes((WeaponAttributes)weaponAttributes[weaponNum]);
			
		}
	
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Q)){
			changeWeapon();
		}
		if(Input.GetKeyDown(KeyCode.Return)){
			openWindow = !openWindow;
			Time.timeScale = Time.timeScale == 0 ? 1 : 0;
		}
		if(openWindow){
			if(Input.GetKeyDown(KeyCode.Z)){
				if(selectorLevel == 0 && weaponAttributes.Count > 0){
					
					weaponNum = selectorPosition;
					GetComponent<Weapon>().setAttributes((WeaponAttributes)weaponAttributes[weaponNum]);
					
				}
			}
			if(Input.GetKeyDown(KeyCode.RightArrow)){
				
				if(selectorPosition < getItemArray().Count - 1 ){
					selectorPosition ++;
				}
			}
			else if(Input.GetKeyDown(KeyCode.LeftArrow)){
				
				
				if(selectorPosition > 0 ){
					selectorPosition -- ;
				}
				
			}
			else if(Input.GetKeyDown(KeyCode.DownArrow)){
				if(selectorLevel < 1){
					selectorLevel ++;
					if(selectorPosition >= getItemArray().Count){
						selectorPosition = getItemArray().Count == 0? 0 : getItemArray().Count - 1;
					}
				}
			}
			else if(Input.GetKeyDown(KeyCode.UpArrow)){
				if(selectorLevel > 0){
					selectorLevel --;
					if(selectorPosition >= getItemArray().Count){
						selectorPosition = getItemArray().Count == 0? 0 : getItemArray().Count - 1;
						
					}
				}
			}
		}
	
	}
	
	// gets the right array for the current selector.
	ArrayList getItemArray(){
		if(selectorLevel == 0){
			return weaponAttributes;
		}
		else if(selectorLevel == 1){
			return itemAttributes;
		}
		else{
			return null;
		}
	}
	
    void OnGUI() {
		
		if(openWindow){
			GUI.skin = inventorySkin;
			freeSpaceX =(int) ((1- 2 * spacing.x) * Screen.width - (border.left + border.right));
			freeSpaceY =(int) (windowHeight * Screen.height - (border.top + border.bottom));
			windowRect = new Rect((float)Screen.width * spacing.x,(float)Screen.height * spacing.y , (float)Screen.width * (1- 2 * spacing.x), (float)Screen.height * windowHeight);
			infoWindowRect = new Rect((float) Screen.width * spacing.x ,(float) Screen.height * (2 * spacing.x + windowHeight), (float) Screen.width * (1- 2 * spacing.x), Screen.height * ( 1 - windowHeight - 3 * spacing.y) );
        	GUI.Window(0, windowRect, DoMyWindow, "Inventory" );
			GUI.Window(1,infoWindowRect, InfoWindow, "" );
		}
		
    }
    void DoMyWindow(int windowID) {
		GUI.Box(new Rect(border.left + selectorPosition * iconSize.x * freeSpaceX  ,border.top + selectorLevel * iconSize.y * freeSpaceY + textHeight + selectorLevel* textHeight, iconSize.x * freeSpaceX,iconSize.y * freeSpaceY ), "");
		GUI.TextField(new Rect(border.left, border.top, freeSpaceX ,textHeight ), "~weaponAttributes~");
		for(int i = 0 ; i < weaponAttributes.Count; i ++){
			
			//GUI.Box(new Rect(8f + 0.1f * i * freeSpaceX,8f, 0.1f * freeSpaceX,0.25f * freSpaceY ), ((WeaponAttributes)weaponAttributes[i]).weaponIcon);
			GUI.DrawTexture(new Rect(border.left + iconSize.x * i * freeSpaceX,border.top + textHeight, iconSize.x * freeSpaceX,iconSize.y * freeSpaceY ), ((WeaponAttributes)weaponAttributes[i]).weaponIcon, ScaleMode.ScaleToFit, true, 0);
		}
        GUI.TextField(new Rect(border.left, border.top + textHeight + iconSize.y*freeSpaceY, freeSpaceX ,textHeight ), "~itemAttributes~");
        for(int i = 0 ; i < itemAttributes.Count; i ++){
			
			//GUI.Box(new Rect(8f + 0.1f * i * freeSpaceX,8f, 0.1f * freeSpaceX,0.25f * freSpaceY ), ((WeaponAttributes)weaponAttributes[i]).weaponIcon);
			GUI.DrawTexture(new Rect(border.left + iconSize.x * i * freeSpaceX,border.top + textHeight*2 + iconSize.y * freeSpaceY, iconSize.x * freeSpaceX,iconSize.y * freeSpaceY ), ((Item)itemAttributes[i]).itemIcon, ScaleMode.ScaleToFit, true, 0);
		}
    }
	void InfoWindow(int windowID){
		string description = "";
		if(selectorLevel == 0){
			if(weaponAttributes.Count == 0){
				return;
			}
			description = ((WeaponAttributes) weaponAttributes[selectorPosition]).description  ;
		}
		else if (selectorLevel == 1){
			if(itemAttributes.Count == 0){
				return;
			}
			description = ((Item) itemAttributes[selectorPosition]).description  ;
		}
		GUI.TextArea( new Rect(border.left, border.top, freeSpaceX, Screen.height - windowHeight - 3 * spacing.y - border.top - border.bottom),description );
	}
	
	
}
