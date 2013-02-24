using UnityEngine;
using System.Collections;
[RequireComponent (typeof(EnemyController))]
[RequireComponent (typeof(Animator))]
public class SeagullAI : MonoBehaviour {
	[SerializeField] float xSpeedScale = 10;
	[SerializeField] float ySpeedScale = 5;
	[SerializeField] Animation flap;
	[SerializeField] Animation soar;
	[SerializeField] Animation dive;
	EnemyController controller;
	Animator animator;
	// Use this for initialization
	void Start () {
		controller = GetComponent<EnemyController>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		float time = Time.time;
		float xSpeed = -1 * Mathf.Cos(0.5f * time);
		float ySpeed = -1 * Mathf.Cos(time) ;
		Debug.Log(xSpeed);
		if(xSpeed > 0 ){
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.lossyScale.y, transform.lossyScale.z);
				
		}
		else{
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.lossyScale.y, transform.lossyScale.z);
		}
		
		
		if(ySpeed > 0 ){
			//animator.setAnimation(soar, true);
		}
		else{
			//animator.setAnimation(dive, true);
		
		}
		controller.setXSpeed(xSpeed * xSpeedScale  );
		controller.setYSpeed(ySpeed * ySpeedScale );	
	}
}
