using UnityEngine;
using System.Collections;
//this struct holds the different kinds of 
[System.Serializable] //so we can see in the inspector
public class WeaponAttributes{
	public Texture uniqueSpriteSheet;//
	public Texture weaponIcon;//for gui
	public string description;//for gui
	//bullet is removed because ammo isn't dependent on weapon
	public Transform muzzleFlash;
	public double speed ;//speed of bullet when fired
	public double fireRate ; // wait between shots so not rate but wait
}
