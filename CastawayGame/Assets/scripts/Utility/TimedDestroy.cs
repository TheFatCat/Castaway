using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {
	public float lifetime = 0.2f;
	private float timer =0f;
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer >= lifetime){
			Destroy(gameObject);
		}
	}
}
