using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	
	[SerializeField] private Texture healthBarBackground;
	[SerializeField] private Texture healthBar;
	[SerializeField] private Texture healthIcon;
	[SerializeField] private Vector2 scaleImage = new Vector2(1,1);
	[SerializeField] private Transform player;
	private Status playerStatus;
	private WeaponImplementer weaponImplementer;
	// Use this for initialization
	void Start () {
		playerStatus = player.GetComponent<Status>();
		weaponImplementer = player.GetComponent<WeaponImplementer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		GUI.DrawTexture(getRect(0,0, 65 , 65), healthIcon);
		GUI.DrawTexture(getRect (70, 0, 175  , 65 ), healthBarBackground);
		GUI.DrawTexture( getRect(75,5, 165 * ((float) playerStatus.getHealth() / playerStatus.getMaxHealth()) , 55 ),healthBar);
		GUI.TextArea(getRect(5 , 70, 50,50) , playerStatus.getHealth() + " / " + playerStatus.getMaxHealth()); 
		GUI.DrawTexture(getRect(375, 0 , 50, 50) , weaponImplementer.getCurrentWeapon().weaponIcon);
	}
	
	
	Rect getRect(float left, float top, float width, float height){
		return new Rect(scaleImage.x * left, scaleImage.y * top, scaleImage.x * width , scaleImage.y * height);
	}
}