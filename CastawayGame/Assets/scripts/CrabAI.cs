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
	// Use this for initialization
	void Start () {
		controller = GetComponent<EnemyController>();
		animator = GetComponent<Animator>();
		
	}
	public float minTimeBetweenActions;
	public float maxTimeBetweenActions;
	private float actionTimeLength = 0;
	private float timeSinceChangedActions = 0;
	// Update is called once per frame
	void Update () {
		timeSinceChangedActions += Time.deltaTime;
		if(timeSinceChangedActions > actionTimeLength){
			timeSinceChangedActions = 0;
			actionTimeLength = Random.Range(minTimeBetweenActions, maxTimeBetweenActions);
			chooseRandomAction();
			
		}
	}
	
	
	
	void chooseRandomAction(){
		Debug.Log("changed action");
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
	void OnDrawGizmos(){
		
	}
}
