using UnityEngine;
using System.Collections;
[RequireComponent (typeof(EnemyController))]
public class CrabAI : MonoBehaviour {
	EnemyController controller ;
	// Use this for initialization
	void Start () {
		controller = GetComponent<EnemyController>();
	}
	
	// Update is called once per frame
	void Update () {
		controller.setXSpeed(-3f);
	}
}
