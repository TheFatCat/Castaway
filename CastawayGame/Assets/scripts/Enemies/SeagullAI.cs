using UnityEngine;
using System.Collections;
[RequireComponent (typeof(EnemyController))]
[RequireComponent (typeof(Animator))]
public class SeagullAI : MonoBehaviour {
	[SerializeField] float swoopSpeed = 10;
	[SerializeField] float xSpeedScale = 10;
	[SerializeField] float swoopDistance = 100;
	[SerializeField] Animation flap;
	[SerializeField] Animation soar;
	[SerializeField] Animation dive;
	EnemyController controller;
	Animator animator;
	
	private bool swooping = false;
	private bool rising = false;
	Vector3 endPosition;
	Vector3 target;
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
		Transform player = PlayerController.getPlayer();
		//Debug.Log(controller.getXSpeed());
		if(!swooping){
			timeSinceTurned += Time.deltaTime;
			
			controller.setYSpeed( 3 * Mathf.Sin( 3 * Time.time));
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
			if(Vector3.Distance(transform.position,player.position) < swoopDistance ){
				swooping = true;
				target = player.position + new Vector3(0f,player.lossyScale.y * 5f,0f);
				endPosition = new Vector3(target.x + 1.25f * (target.x - transform.position.x),transform.position.y,transform.position.z); 
			}
		}
		else{
			float dx = target.x - transform.position.x;
			float dy = target.y - transform.position.y;
			float scale = swoopSpeed /Mathf.Sqrt(Mathf.Pow(dx,2) + Mathf.Pow(dy,2));
			if(!rising ){
				controller.setXSpeed(dx * scale);
				controller.setYSpeed(dy * scale);
				if(scale > 20){
					rising = true;
					target = endPosition;
				}
			}
			else{
				controller.setXSpeed(Mathf.Lerp(controller.getXSpeed(),dx * scale,0.1f));
				controller.setYSpeed(Mathf.Lerp(controller.getYSpeed(),dy * scale,0.01f));
				if(scale > 20){
					swooping = false;
					rising = false;
				}
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
