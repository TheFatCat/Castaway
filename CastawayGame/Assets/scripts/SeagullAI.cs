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
		controller.setXSpeed(xSpeedScale);
	}
	private float timeSinceTurned = 0;
	private float turnWait = 5;
	// Update is called once per frame
	void Update () {
		//Debug.Log(controller.getXSpeed());
		timeSinceTurned += Time.deltaTime;
		
		controller.setYSpeed(Mathf.Sin(Time.time));
		if(timeSinceTurned > turnWait){
			timeSinceTurned = 0;
			turnWait = Random.Range(5,20);
			if(controller.getXSpeed() > 0){
				controller.setXSpeed(-1 * xSpeedScale);
			}
			else{
				controller.setXSpeed( xSpeedScale);

			}
		}
		
		
		if(controller.getXSpeed() > 0 ){
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.lossyScale.y, transform.lossyScale.z);
				
		}
		else{
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.lossyScale.y, transform.lossyScale.z);
		}
		
		/*
		if(ySpeed > 0 ){
			//animator.setAnimation(soar, true);
		}
		else{
			//animator.setAnimation(dive, true);
		
		}
		
		controller.setXSpeed(xSpeed * xSpeedScale  );
		controller.setYSpeed(ySpeed * ySpeedScale );
		*/	
	}
}
