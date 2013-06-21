using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {
	public int maxEnemies = 10; //maximum number of enemies this spawn point can have at a time
	Transform[] enemies ;//stores all tracked enemies
	public GameObject[] prefabs; //stores all the possible enemies that could be spawned
	public float respawnTime = 10;// time betweeen respawn
	// Use this for initialization
	void Start () {
		enemies = new Transform[maxEnemies];
	}
	float timer = 0;
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer > respawnTime){
			timer = 0;
			for(int i = 0; i < enemies.Length; i ++){
				//if we have a free space	
				if( enemies[i] == null){
					enemies[i] = Instantiate(prefabs[Random.Range(0,prefabs.Length - 1)], transform.position, transform.rotation) as Transform;
					break;
				}
			}
		}
	}
}
