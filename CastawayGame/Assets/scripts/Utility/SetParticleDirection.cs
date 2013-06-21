using UnityEngine;
using System.Collections;

public class SetParticleDirection : MonoBehaviour {

	public ParticleSystem ps;

	void Start () {
		ps = this.GetComponent<ParticleSystem>();
		ps.startRotation = Mathf.Deg2Rad * transform.parent.eulerAngles.z + Random.Range(-0.3f,0.3f);
	}
	

	void Update () {
		//nothing to see here
	}
}
