using UnityEngine;
using System.Collections;

public class cameraScreentoworldtest : MonoBehaviour {
	//private Vector3 v;
	//public Transform perlin;
	private PerlinVoxelMesh pvm;
	// Use this for initialization
	void Start () {
		//pvm = perlin.GetComponent<PerlinVoxelMesh>();
		Debug.Log ("started");
		StartCoroutine(Do ());
		Debug.Log ("Doing something else");
	}
	
	IEnumerator Do() {
        print("Do now");
        while(true){
         print("bleh");
         yield return null;
        }
    }

	
    void Update() {
    	
    	//v = transform.position + new Vector3(((Input.mousePosition.x/Screen.width) - 0.5f) * 2.0f * Camera.main.orthographicSize * Screen.width/Screen.height,((Input.mousePosition.y/Screen.height) - 0.5f) * 2.0f * Camera.main.orthographicSize,0.0f);
    	
        //Debug.DrawLine(transform.position,v + new Vector3(0.0f,0.0f,40f), Color.blue);
        
    }
}
