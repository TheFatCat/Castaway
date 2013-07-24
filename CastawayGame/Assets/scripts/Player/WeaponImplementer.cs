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
	public Weapon currentWeapon = null;
	public Weapon currentWeapon2 = null;
	private double timer = 0.0; // to keep track of rate of fire
	
	
	public List<Weapon> getWeapons(){
		return weapons;
	}

	public int getAmmo(){

		return currentWeapon.ammo;
	}

	public int getMaxAmmo(){

		return currentWeapon.maxAmmo;

	}

	public void fire2 (Vector3 bulletLocation,Vector3 bulletDirection, Vector3 playerVelocity){
		if (currentWeapon2.shootSound) {
			audio.PlayOneShot (currentWeapon2.shootSound, currentWeapon2.shootVolume);
		}
		bulletLocation.x *= -(transform.localScale.x/Mathf.Abs(transform.localScale.x));//change for direction
		Debug.Log ("threw something");
		Rigidbody bullet2Clone = Instantiate(currentWeapon2.bullet,transform.position + bulletLocation, Quaternion.identity) as Rigidbody;//instantiate bullet
		if(transform.localScale.x < 0 ){	
			bullet2Clone.transform.localScale = new Vector3 (-bullet2Clone.transform.localScale.x,bullet2Clone.transform.localScale.y,bullet2Clone.transform.localScale.z); 
		}
		bullet2Clone.transform.parent = transform;
		//bullet2Clone.velocity = playerVelocity;

	}


	public void  fire (Vector3 bulletLocation, Vector3 bulletDirection, Vector3 playerVelocity){
		
			if(currentWeapon.ammo > 0 && canShoot){	//shoot
				//audio
				if (currentWeapon.shootSound) {

					audio.PlayOneShot (currentWeapon.shootSound, currentWeapon.shootVolume);
				}
				currentWeapon.ammo --;
				bulletLocation.x *= -(transform.localScale.x/Mathf.Abs(transform.localScale.x));//change for direction
				bulletLocation.z -= 0.3f;	//make sure bullets are in front of player
				Rigidbody bulletClone= Instantiate(currentWeapon.bullet,transform.position + bulletLocation, Quaternion.identity) as Rigidbody;//instantiate bullet

				//instantiate muzzle flash based on direction
				if(bulletDirection.x == 0){		//looking up or down
					if(bulletDirection.y > 0.5){//looking up
						Rigidbody muzzleFlash = Instantiate(currentWeapon.muzzleFlashUp,transform.position + bulletLocation, Quaternion.identity) as Rigidbody;
						muzzleFlash.velocity = playerVelocity;
					} else {					//looking down
						Rigidbody muzzleFlash = Instantiate(currentWeapon.muzzleFlashDown,transform.position + bulletLocation, Quaternion.identity) as Rigidbody;
						muzzleFlash.velocity = playerVelocity;
					}
				} else {						//looking left or right
					if(bulletDirection.x > 0.5){//looking right
						Rigidbody muzzleFlash = Instantiate(currentWeapon.muzzleFlashRight,transform.position + bulletLocation, Quaternion.identity) as Rigidbody;
						muzzleFlash.velocity = playerVelocity;
					} else {					//looking left
						Rigidbody muzzleFlash = Instantiate(currentWeapon.muzzleFlashLeft,transform.position + bulletLocation, Quaternion.identity) as Rigidbody;
						muzzleFlash.velocity = playerVelocity;
					}
				}
				
				
				bulletClone.velocity = (bulletDirection * (float)(currentWeapon.bulletSpeed));
				Vector3 bulletRotation = new Vector3(-bulletDirection.y,bulletDirection.x,0);	//rotate vector by 90 degrees (quaternions are stupid)
				bulletClone.rotation = Quaternion.LookRotation(Vector3.forward,bulletRotation);	//set the rotation of the new vector
				
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