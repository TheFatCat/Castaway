using UnityEngine;
using System.Collections;

public class PlayerStatus : Status {

	float invincibleFlashTimer = 0;
	
	void Start(){
		animator = GetComponent<Animator>();
	}
	
	
	
	void Update(){
		if(flashTimer < flashTime){
			flashTimer += Time.deltaTime;

			if(flashTimer > flashTime){
				if(animator != null){
					animator.endFlashFrame();
				}

				//renderer.material.color = Color.white; // after a tenth of a second switch back to normal color
			}
		}
		if(invincible){
			invincibleFlashTimer += Time.deltaTime;
			if(invincibleFlashTimer > 0.05){
				invincibleFlashTimer = 0;
				renderer.enabled = !renderer.enabled;
			}
			invincibleTimer += Time.deltaTime;
			if(invincibleTimer > invincibleTime){
				
				renderer.enabled = true;
				invincible = false;
				
			}
			
		}
	}
}
