using UnityEngine;
using System.Collections;

// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.


public class PlayerSpriteAnimate : MonoBehaviour {
//a script to handle all animations for the player

public enum animationMode { Wait = 0, Cut = 1, Overlay = 2 };

public enum animationType {IdleLeft = 0, IdleUp = 1, MoveLeft = 2, MoveUp = 3, MoveLegs = 4, CrouchStart = 5, CrouchLeft = 6, CrouchMove = 7, JumpLeft = 8, JumpUp = 9, JumpDown = 10, Turn = 11, ShootLeft = 12, ShootUp = 13, ShootMoveLeft = 14, ShootMoveUp = 15, ShootCrouch = 16, ShootFallLeft = 17, ShootFallUp = 18, ShootFallDown = 19, ThrowIdle = 20, ThrowMove = 21, ThrowCrouch = 22, ThrowFall = 23, Die = 24, HitLeft = 25, HitUp = 26, HitJump = 27, HitCrouch = 28 };

Transform overlay;

public double tilingX = 1.0;
public double tilingY = 1.0;
public double animationDelay = 0.2;
private double timer = 0.0;
public double offsetX = 0.0;
public double offsetY = 0.0;
//only horizontal looping supported
public int Xmin = 0;
public int Xmax = 1;
public int Yframe = 1;

//animationMode CurMode= animationMode.Wait;
public animationType CurType= animationType.IdleLeft;
//FIXME_VAR_TYPE NewType= animationType.IdleLeft;
public animationType FallbackType= animationType.IdleLeft;
public wrap CurWrap= wrap.Loop;
//bool  done =  false;

void Start (){
		SetAnimation(FallbackType);	//set to the current animation
	}

void  Update (){
	timer += Time.deltaTime;	//add to timer
	if(timer >= animationDelay){	//wait for timer to expire to do anything
		timer = 0.0f;
		//advance one frame
		offsetX += tilingX;
		//done = false;
		//check if out of loop
		if(offsetX < Xmin * tilingX){//too early
			offsetX = Xmin * tilingX;	
			
		}else if(offsetY != Yframe * tilingY){//vertical out of sync
			offsetY = Yframe * tilingY;
			
		}else if(offsetX >= Xmax * tilingX){//we are on the first undesired frame
			//if we are looping, go back to first frame
			if(CurWrap == wrap.Loop){
				SetFrameMin();
				
			//if we are oneshot hold, stay on last frame
			} else if(CurWrap == wrap.OneShotHold){
				offsetX -= tilingX;	//go back one frame 
				
			}else{	//if not we are oneshot, then go back to fallback
				SetAnimation(FallbackType);
				
				SetFrameMin();
			}
		}
		
		
		
	
	}
	//only need to update offset when timer expires
		offsetY = Yframe * tilingY;
		renderer.material.mainTextureOffset = new Vector2((float)offsetX, (float)(1.0f - offsetY));	//inverse y offset makes things easier
}

public int  GetFrame (){
	
	return (int)((offsetX / tilingX) - Xmin);
	
}

public bool  FrameInBounds (){
	if(offsetX >= tilingX * Xmin && offsetX <= Xmax * tilingX){
		return true;
	}else{
		return false;
	}
}

public void  SetFrame ( int frame  ){
	frame += Xmin;
	if(frame * tilingX >= Xmin && frame * tilingX <= Xmax){
		offsetX = frame * tilingX;
	}
}

public void  SetFrameMin (){
	offsetX = Xmin * tilingX;
}


public void  SetFallback ( animationType type  ){
	//set fallback type
	FallbackType = type;
}
public void  SetAnimation ( animationType type  ){
	//set the new animation to the one specified
	if(CurType != type){
		
		switch (type){
			case animationType.IdleLeft:
				//set frame coordinates
				Xmin = 12;
				Xmax = 17;
				Yframe = 1;
				//set speed
				animationDelay = 0.18f;
				//set wrapping
				CurWrap = wrap.Loop;
				//always cut
					SetFrameMin();	
				
				//set the current animation
				CurType = type;
				break;
			case animationType.IdleUp:
				//set frame coordinates
				Xmin = 12;
				Xmax = 17;
				Yframe = 2;
				//set speed
				animationDelay = 0.18f;
				//set wrapping
				CurWrap = wrap.Loop;
				//always cut
					SetFrameMin();	
				//set the current animation
				CurType = type;
				break;
			case animationType.MoveLeft:
				
				//set frame coordinates
				Xmin = 0;
				Xmax = 12;
				Yframe = 1;
				//set speed
				animationDelay = 0.05f;
				timer = animationDelay;	//cut straight to animation
				//set wrapping
				CurWrap = wrap.Loop;
				if(!FrameInBounds()){	//if out of bounds, set to min. otherwise, keep frame
					SetFrameMin();	
				}//set the current animation
				CurType = type;
				break;
			case animationType.MoveLegs:
				//set frame coordinates
				Xmin = 0;
				Xmax = 12;
				Yframe = 3;
				//set speed
				animationDelay = 0.05f;
				//set wrapping
				CurWrap = wrap.Loop;
				if(!FrameInBounds()){	//if out of bounds, set to min. otherwise, keep frame
					SetFrameMin();	
				}//set the current animation
				CurType = type;
				break;
			case animationType.MoveUp:
				//set frame coordinates
				Xmin = 0;
				Xmax = 12;
				Yframe = 2;
				//set speed
				animationDelay = 0.05f;
				timer = animationDelay;	//cut straight to animation
				//set wrapping
				CurWrap = wrap.Loop;
				if(!FrameInBounds()){	//if out of bounds, set to min. otherwise, keep frame
					SetFrameMin();	
				}//set the current animation
				CurType = type;
				break;
			case animationType.CrouchStart:
			
				//set frame coordinates
				Xmin = 14;
				Xmax = 17;
				Yframe = 9;
				//set speed
				animationDelay = 0.1f;
				//always cut-----------------
				SetFrameMin();
				//set wrapping
				CurWrap = wrap.OneShot;
				//set the current animation
				CurType = type;
				break;
			case animationType.CrouchLeft:
				if(CurType == animationType.CrouchMove || CurType == animationType.HitCrouch ||(CurType == animationType.CrouchStart && offsetX >= Xmax * tilingX)){//if came from crouch left or crouch start 
					//set frame coordinates
					Xmin = 12;
					Xmax = 16;
					Yframe = 3;
					//set speed
					animationDelay = 0.18f;
					//set wrapping
					CurWrap = wrap.Loop;
					//always cut
						SetFrameMin();	
					
					//set the current animation
					CurType = type;
				}else{																										//we didn't
					SetAnimation(animationType.CrouchStart);//start the crouch
					SetFallback(animationType.CrouchLeft);
					
				}
				break;
			case animationType.CrouchMove:
				if(CurType == animationType.CrouchLeft || (CurType == animationType.CrouchStart && offsetX >= Xmax * tilingX)){//if came from crouch left or crouch start
					//set frame coordinates
					Xmin = 8;
					Xmax = 14;
					Yframe = 9;
					//set speed
					animationDelay = 0.1f;
					//set wrapping
					CurWrap = wrap.Loop;
					//always cut
						SetFrameMin();	
					
					//set the current animation
					CurType = type;
				}else{
					SetAnimation(animationType.CrouchStart);//start the crouch
					SetFallback(animationType.CrouchMove);
					
				}
				break;
			case animationType.JumpLeft:
				//set frame coordinates
				Xmin = 0;
				Xmax = 8;
				Yframe = 8;
				//set speed
				animationDelay = 0.1f;
				//set wrapping
				CurWrap = wrap.OneShotHold;
				//always cut-----------------
				SetFrameMin();
				//set the current animation
				CurType = type;
				break;
			case animationType.JumpUp:
				//set frame coordinates
				Xmin = 8;
				Xmax = 16;
				Yframe = 8;
				//set speed
				animationDelay = 0.1f;
				//set wrapping
				CurWrap = wrap.OneShotHold;
				//always cut-----------------
				SetFrameMin();
				//set the current animation
				CurType = type;
				break;
			case animationType.JumpDown:
				//set frame coordinates
				Xmin = 0;
				Xmax = 8;
				Yframe = 9;
				//set speed
				animationDelay = 0.1f;
				//set wrapping
				CurWrap = wrap.OneShotHold;
				//always cut-----------------
				SetFrameMin();
				//set the current animation
				CurType = type;
				break;
			case animationType.Turn:
				//set frame coordinates
				Xmin = 16;
				Xmax = 17;
				Yframe = 3;
				//set speed
				animationDelay = 0.2f;
				//set wrapping
				CurWrap = wrap.OneShot;
				//always cut-----------------
				SetFrameMin();
				//set the current animation
				CurType = type;
				break;
			case animationType.ShootLeft:
				//set frame coordinates
				Xmin = 0;
				Xmax = 8;
				Yframe = 4;
				//set speed
				animationDelay = 0.06f;
				//set wrapping
				CurWrap = wrap.OneShot;
				//set the current animation
				CurType = animationType.IdleUp;
				break;
			case animationType.ShootUp:
				//set frame coordinates
				Xmin = 8;
				Xmax = 16;
				Yframe = 4;
				//set speed
				animationDelay = 0.06f;
				//set wrapping
				CurWrap = wrap.OneShot;
				//set the current animation
				CurType = animationType.IdleLeft;
				break;
			case animationType.ShootMoveLeft:					//special
				//set frame coordinates
				Xmin = 0;
				Xmax = 8;
				Yframe = 5;
				//set speed
				//set speed
				animationDelay = 0.06f;
				//set wrapping
				CurWrap = wrap.OneShot;
				//set the current animation
				CurType = animationType.IdleUp;
				break;
			case animationType.ShootMoveUp:							//special
				//set frame coordinates
				Xmin = 8;
				Xmax = 16;
				Yframe = 5;
				//set speed
				animationDelay = 0.06f;
				//set wrapping
				CurWrap = wrap.OneShot;
				//set the current animation
				CurType = animationType.IdleUp;
				break;
			case animationType.ShootCrouch:
				//set frame coordinates
				Xmin = 0;
				Xmax = 8;
				Yframe = 6;
				//set speed
				animationDelay = 0.06f;
				//set wrapping
				CurWrap = wrap.OneShot;
				//set the current animation
				CurType = animationType.CrouchLeft;
				break;
			case animationType.ShootFallLeft:
				//set frame coordinates
				Xmin = 8;
				Xmax = 16;
				Yframe = 6;
				//set speed
				animationDelay = 0.06f;
				//set wrapping
				CurWrap = wrap.OneShotHold;
				//set the current animation
				CurType = animationType.IdleLeft;
				break;
			case animationType.ShootFallUp:
				//set frame coordinates
				Xmin = 0;
				Xmax = 8;
				Yframe = 7;
				//set speed
				animationDelay = 0.06f;
				//set wrapping
				CurWrap = wrap.OneShotHold;
				//set the current animation
				CurType = animationType.IdleLeft;
				break;
			case animationType.ShootFallDown:
				//set frame coordinates
				Xmin = 8;
				Xmax = 16;
				Yframe = 7;
				//set speed
				animationDelay = 0.06f;
				//set wrapping
				CurWrap = wrap.OneShotHold;
				//set the current animation
				CurType = animationType.JumpDown;
				break;
			case animationType.ThrowIdle:
				//set frame coordinates
				Xmin = 0;
				Xmax = 4;
				Yframe = 10;
				//set speed
				animationDelay = 0.1f;
				//set wrapping
				CurWrap = wrap.OneShot;
				
				//set the current animation
				CurType = type;
				break;
			case animationType.ThrowMove:					//special
				//set frame coordinates
				Xmin = 4;
				Xmax = 8;
				Yframe = 10;
				//set speed
				animationDelay = 0.1f;
				//set wrapping
				CurWrap = wrap.OneShot;
				//set the current animation
				CurType = type;
				break;
			case animationType.ThrowCrouch:
				//set frame coordinates
				Xmin = 8;
				Xmax = 12;
				Yframe = 10;
				//set speed
				animationDelay = 0.1f;
				//set wrapping
				CurWrap = wrap.OneShot;
				
				//set the current animation
				CurType = animationType.CrouchStart;
				break;
			case animationType.ThrowFall:
				//set frame coordinates
				Xmin = 12;
				Xmax = 16;
				Yframe = 10;
				//set speed
				animationDelay = 0.1f;
				//set the current animation
				CurType = animationType.CrouchLeft;
				break;
			case animationType.Die:
				//set frame coordinates
				Xmin = 0;
				Xmax = 9;
				Yframe = 11;
				//set speed
				animationDelay = 0.07f;
				//always cut
				SetFrameMin();
				//set wrapping
				CurWrap = wrap.OneShotHold;
				//set the current animation
				CurType = animationType.Die;
				break;

			case animationType.HitLeft:
				//set frame coordinates
				Xmin = 16;
				Xmax = 17;
				Yframe = 4;
				//always cut
				SetFrameMin();
				//set speed
				animationDelay = 0.1f;
				//set the current animation
				CurType = type;
				break;

			case animationType.HitUp:
				//set frame coordinates
				Xmin = 16;
				Xmax = 17;
				Yframe = 5;
				//always cut
				SetFrameMin();
				//set speed
				animationDelay = 0.1f;
				//set the current animation
				CurType = type;
				break;

			case animationType.HitJump:
				//set frame coordinates
				Xmin = 16;
				Xmax = 17;
				Yframe = 6;
				//always cut
				SetFrameMin();
				//set speed
				animationDelay = 0.1f;
				//set the current animation
				CurType = type;
				break;

			case animationType.HitCrouch:
				//set frame coordinates
				Xmin = 16;
				Xmax = 17;
				Yframe = 7;
				//always cut
				SetFrameMin();
				//set speed
				animationDelay = 0.1f;
				//set the current animation
				CurType = animationType.CrouchLeft;
				break;
			
			}
		
	}
}

}
