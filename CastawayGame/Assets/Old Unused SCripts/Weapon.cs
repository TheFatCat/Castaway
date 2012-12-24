using UnityEngine;
using System.Collections;
public class Weapon : MonoBehaviour{
	public Texture weaponIcon;
	public Texture uniqueSpriteSheet;
	public string description;
	public Rigidbody bullet;
	public Transform muzzleFlash;
	public double speed = 10;
	public double fireRate = 0.2;//seconds per shot
	
	private double timer = 0.0;
	
	public void  fire ( Vector3 bulletLocation ,  Vector3 bulletDirection  ){
		
			
			bulletLocation.x *= -(transform.localScale.x/Mathf.Abs(transform.localScale.x));//change for direction
			Rigidbody bulletClone= Instantiate(bullet,transform.position + bulletLocation, Quaternion.identity) as Rigidbody;//instantiate bullet
			Rigidbody muzzleFlashClone= Instantiate(muzzleFlash,transform.position + bulletLocation, Quaternion.identity)as Rigidbody;//instantiate muzzle flash
			
			bulletClone.velocity = bulletDirection * (float)speed;
		
	}
	
	void  Update (){
		timer+= Time.deltaTime;	
		if(Input.GetButtonDown("Fire1")&& timer >= fireRate && speed != 0){
			timer = 0.0f;
			PlayerController controller = GetComponent<PlayerController>();
			controller.fire();
		}
	}
	public void setAttributes(WeaponAttributes attributes){
		renderer.material.mainTexture = attributes.uniqueSpriteSheet;
		uniqueSpriteSheet = attributes.uniqueSpriteSheet;
		weaponIcon = attributes.weaponIcon;
		description = attributes.description;
		muzzleFlash = attributes.muzzleFlash;
		speed = attributes.speed;
		fireRate = attributes.fireRate;
	}
}