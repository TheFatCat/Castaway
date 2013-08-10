using UnityEngine;
using System.Collections;
[RequireComponent (typeof(EnemyController))]
[RequireComponent (typeof(Animator))]
public class SeagullAI : MonoBehaviour {
	[SerializeField] float swoopDuration = 3;
	[SerializeField] float xSpeedScale = 10;
	[SerializeField] float ySpeedScale = 5;
	[SerializeField] float swoopDistance = 100;
	[SerializeField] Animation flap;
	[SerializeField] Animation soar;
	[SerializeField] Animation dive;
	EnemyController controller;
	Animator animator;
	
	private bool swooping = false;
	private float yZero = 0;
	private int xMultiplier = 1;
	private float xZero = 0;
	private float t = 0;
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
				Vector3 position = player.position + new Vector3(0f,player.lossyScale.y * 5f,0f);
				float dX = position.x - transform.position.x;
				float dY = position.y - transform.position.y;
				yZero = 4 * dY / swoopDuration; 
				xZero = dX / swoopDuration;
				xMultiplier = dX > 1 ? 1:-1;
			}
		}
		else{
			float deltaTime = Time.deltaTime;
			t += deltaTime;
			controller.setXSpeed(xMultiplier*Mathf.Abs(xZero - (xZero / (0.5f * swoopDuration)) * (t - 0.5f * swoopDuration)));
			controller.setYSpeed(-1 * (t - 0.5f * swoopDuration) *(yZero / (0.5f * swoopDuration)));
			if(t > swoopDuration){
				t = 0;
				swooping = false;
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
