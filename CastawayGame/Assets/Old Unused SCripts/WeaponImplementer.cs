using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Weapon : MonoBehaviour{
	/*
	public Texture weaponIcon;
	public Texture uniqueSpriteSheet;
	public string description;
	public Rigidbody bullet;
	public Transform muzzleFlash;
	public double speed = 10;
	public double fireRate = 0.2;//seconds per shot
	*/
	private List<WeaponAttributes> weapons = new List<WeaponAttributes>();
	private WeaponAttributes currentWeapon;
	private double timer = 0.0;
	
	public void  fire ( Vector3 bulletLocation ,  Vector3 bulletDirection  ){
		
			
			bulletLocation.x *= -(transform.localScale.x/Mathf.Abs(transform.localScale.x));//change for direction
			Rigidbody bulletClone= Instantiate(currentWeapon.bullet,transform.position + bulletLocation, Quaternion.identity) as Rigidbody;//instantiate bullet
			Instantiate(currentWeapon.muzzleFlash,transform.position + bulletLocation, Quaternion.identity);//instantiate muzzle flash
			
			bulletClone.velocity = bulletDirection * (float)(currentWeapon.speed);
		
	}
	public void addWeapon(WeaponAttributes weapon){
		if(weapons.Contains(weapon)){
			Debug.Log("Already have that weapon");
			return;
		}
		weapons.Add(weapon);
	}
	
	public void removeWeapon(WeaponAttributes weapon){
		if(! weapons.Contains(weapon)){
			Debug.Log("the weapon you are trying to remove isnt in the array");
			return;
		}
		weapons.Remove(weapon);	
	}
	
	void  Update (){
		timer+= Time.deltaTime;	
		if(Input.GetButtonDown("Fire1")&& timer >= currentWeapon.fireRate ){
			timer = 0.0f;
			PlayerController controller = GetComponent<PlayerController>();
			controller.fire();
		}
	}
	
	public void changeCurrentWeaponTo(WeaponAttributes weapon){
	
	}
	public void setAttributes(WeaponAttributes attributes){
		/*
		renderer.material.mainTexture = attributes.uniqueSpriteSheet;
		uniqueSpriteSheet = attributes.uniqueSpriteSheet;
		weaponIcon = attributes.weaponIcon;
		description = attributes.description;
		muzzleFlash = attributes.muzzleFlash;
		speed = attributes.speed;
		fireRate = attributes.fireRate;
		*/
	}
}