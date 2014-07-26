using UnityEngine;
using System.Collections;

public class normaldebugger : MonoBehaviour {
	private Mesh mesh;
	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
		
		foreach(Vector4 tangent in mesh.tangents){
			Debug.Log (mesh.vertexCount);
			Debug.Log (mesh.tangents.Length);
			Debug.Log(tangent);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
}
