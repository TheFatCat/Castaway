using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	public float hp = 1.0f;
	//public Transform drop;
	private bool selected = false;
	
	void OnMouseEnter(){
		selected = true;
	}
	
	void OnMouseExit(){
		selected = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(selected){
			if(Input.GetMouseButton(0)){	//clicked
				PerlinNoise perlin = PerlinNoise.getPerlin().GetComponent<PerlinNoise>();
				perlin.write ((int)transform.position.x,(int)transform.position.y,0.0f);
				Destroy(gameObject);
			}
		}
	}
}
