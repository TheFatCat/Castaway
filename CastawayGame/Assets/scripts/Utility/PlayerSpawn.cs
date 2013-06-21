using UnityEngine;
using System.Collections;

public class PlayerSpawn : MonoBehaviour {
	static Transform spawn;
	// Use this for initialization
	void Start () {
		spawn = transform;
	}
	
	
	
	public static Transform getPlayerSpawn(){
		return spawn;
	}
}
