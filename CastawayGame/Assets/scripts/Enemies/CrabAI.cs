using UnityEngine;
using System.Collections;
[RequireComponent (typeof(EnemyController))]
[RequireComponent (typeof(Animator))]
public class CrabAI : MonoBehaviour {
	public Animation idle;
	public Animation scuttle;
	public float speed;
	EnemyController controller ;
	Animator animator;
	public float minTimeBetweenActions;
	public float maxTimeBetweenActions;
	private float actionTimeLength = 0;
	private float timeSinceChangedActions = 0;
	public float maxDistanceFromStartingPoint; // only has to do with x
	private float startingXPosition;
	// Use this for initialization
	void Start () {
		controller = GetComponent<EnemyController>();
		animator = GetComponent<Animator>();
		startingXPosition = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		
		// make sure the crab doesnt go to far from the starting point
		if(transform.position.x > startingXPosition + maxDistanceFromStartingPoint){
			controller.setXSpeed(-1 * speed);
			animator.setAnimation(scuttle,true);
		}
		else if(transform.position.x < startingXPosition - maxDistanceFromStartingPoint){
			controller.setXSpeed(speed);
			animator.setAnimation(scuttle,true);
		}
		
		// after the amount of time do a new action randomly chosen
		timeSinceChangedActions += Time.deltaTime;
		if(timeSinceChangedActions > actionTimeLength){
			timeSinceChangedActions = 0;
			actionTimeLength = Random.Range(minTimeBetweenActions, maxTimeBetweenActions); // time between actions s also random
			chooseRandomAction();
			
		}
	}
	
	
	
	void chooseRandomAction(){
		//Debug.Log("changed action");
		// pick a random action idle moveleft or moveright
		switch(Random.Range(0,3)){
		case 0 :
			controller.setXSpeed(0);
			animator.setAnimation(idle,true);
			break;
		case 1:
			controller.setXSpeed(speed);
			animator.setAnimation(scuttle,true);
			break;
		case 2:
			controller.setXSpeed(-1 * speed);
			animator.setAnimation(scuttle,true);
			break;
		default:
			break;
		}
	}
	
	//draw the maximum distance the crab can move
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + new Vector3(-1 * maxDistanceFromStartingPoint , 0 ,0));
		
		Gizmos.DrawLine(transform.position, transform.position + new Vector3( maxDistanceFromStartingPoint , 0 ,0));

	}
}
