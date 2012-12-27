using UnityEngine;
using System.Collections;
//this struct holds the different kinds of 
[System.Serializable] //so we can see in the inspector
public class Weapon {
	[SerializeField]public  Texture uniqueSpriteSheet;//
	[SerializeField]public  Texture weaponIcon;//for gui
	[SerializeField]public  string description;//for gui
	[SerializeField]public  Rigidbody bullet ;//bullet gun will fire
	[SerializeField]public  Transform muzzleFlash;
	[SerializeField]public  double bulletSpeed ;//speed of bullet when fired
	[SerializeField]public  double timeBetweenShots ; // wait between shots so not rate but wait
	[SerializeField]public int ammo = 0;
	[SerializeField]public int maxAmmo = 0;
	
	
}
