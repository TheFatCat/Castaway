using UnityEngine;
using System.Collections;

public class ItemHandler : MonoBehaviour {
	// pick only one to give a value
	public Weapon weaponAttributes;
	public ItemAttributes itemAttributes;
	public ItemType itemType;
	
	[System.Serializable]
	public enum ItemType{
		WEAPON, ITEM, TOOL
	}
}
