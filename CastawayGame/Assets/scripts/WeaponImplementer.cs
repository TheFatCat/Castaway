using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WeaponImplementer : MonoBehaviour{
	public bool canShoot = true;
	/*
	public Texture weaponIcon;
	public Texture uniqueSpriteSheet;
	public string description;
	public Rigidbody bullet;
	public Transform muzzleFlash;
	public double speed = 10;
	public double fireRate = 0.2;//seconds per shot
	*/
	[SerializeField] private List<Weapon> weapons = new List<Weapon>(); // essentially the players armory
	[SerializeField] private Weapon currentWeapon;
	private double timer = 0.0; // to keep track of rate of fire
	
	public void  fire ( Vector3 bulletLocation ,  Vector3 bulletDirection  ){
		
			if(currentWeapon.ammo > 0 && canShoot){
				currentWeapon.ammo --;
				bulletLocation.x *= -(transform.localScale.x/Mathf.Abs(transform.localScale.x));//change for direction
				Rigidbody bulletClone= Instantiate(currentWeapon.bullet,transform.position + bulletLocation, Quaternion.identity) as Rigidbody;//instantiate bullet
				Instantiate(currentWeapon.muzzleFlash,transform.position + bulletLocation, Quaternion.identity);//instantiate muzzle flash
			
				bulletClone.velocity = bulletDirection * (float)(currentWeapon.bulletSpeed);
			}
	}
	public void addWeapon(Weapon weapon){
		if(weapons.Contains(weapon)){
			Debug.Log("Already have that weapon");
			return;
		}
		
		weapons.Add(weapon);
		//allow us to shoot. redundant, but who cares
		PlayerController controller = GetComponent<PlayerController>();
		controller.SetCanShoot(true);
		//if this is the only weapon we have
		if(weapons.Count == 1){
			changeCurrentWeaponTo(weapon);

		}
		
	}
	
	public void removeWeapon(Weapon weapon){
		if(! weapons.Contains(weapon)){
			Debug.Log("the weapon you are trying to remove isnt in the array");
			return;
		}
		weapons.Remove(weapon);	
	}
	
	void  Update (){
		timer+= Time.deltaTime;	
		if((Input.GetButtonDown("Fire1") || (Input.GetButton("Fire1") && currentWeapon.automatic) )&& timer >= currentWeapon.timeBetweenShots ){
			timer = 0.0f;
			
			PlayerController controller = GetComponent<PlayerController>();
			controller.fire();
		}
	}
	
	public void changeCurrentWeaponTo(Weapon weapon){
		if(!weapons.Contains(weapon)){
			Debug.Log("cant change to that weapon because its not in the array");
			return;
		}
		renderer.material.mainTexture = weapon.uniqueSpriteSheet;
		currentWeapon = weapon;
		
	}
	
	
	public Weapon getCurrentWeapon(){
		return currentWeapon;
	}
	public void setAttributes(Weapon attributes){
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