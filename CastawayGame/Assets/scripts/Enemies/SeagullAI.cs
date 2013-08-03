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
	
	private bool swooping = false;
	private float a = 0;
	private float b = 0;
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
			if(Vector3.Distance(transform.position,player.position) < 100 ){
				swooping = true;
				Vector3 position = player.position + new Vector3(0f,player.lossyScale.y * 5f,0f);
				float dX = position.x - transform.position.x;
				float dY = position.y - transform.position.y;
				t = 0f;
				a = dY / 9f;
				b = dX / 3f;
			}
		}
		else{
			float deltaTime = Time.deltaTime;
			t += deltaTime;
			controller.setXSpeed(b);
			controller.setYSpeed(2 * -1 * (t - 3) * a);
			if(t > 6){
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
