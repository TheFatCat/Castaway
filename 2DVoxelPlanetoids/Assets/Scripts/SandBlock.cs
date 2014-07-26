using UnityEngine;
using System.Collections;

public class SandBlock : MonoBehaviour {
	
	private Vector3 v = Vector3.zero;
	public float speed = 2.0f;
	private bool stopped = false;
	private bool inTransit = true;
	private PerlinVoxelMesh pvm;
	
	public void SetPVM(PerlinVoxelMesh q){
		pvm = q;
		inTransit = false;
	}
	
	// Update is called once per frame
	void Update ()
		{
				if (!stopped) {
						if (!inTransit) {
								inTransit = true;
								v = pvm.checkSandPosition (transform.position.x, transform.position.y, transform.position.z);
								if (v.Equals(transform.position)) {
										stopped = true;
										if(pvm.addBlock(transform.position.x - 0.5f,transform.position.y + 0.5f,3)){
											Debug.Log ("added");
											Destroy(gameObject);
										}
										//couldn't add, better move up
										v = transform.position + Vector3.up;
										stopped = false;
								}
						} else {
								transform.position = Vector3.Lerp (transform.position, v, speed * Time.deltaTime);	//lerp to position
								if(Vector3.Distance(transform.position,v) <= 0.2f){
									inTransit = false;
								}
						}
				}
	}
}
