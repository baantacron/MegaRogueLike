using UnityEngine;
using System.Collections;

public class __Player_Move : MonoBehaviour {
	
	public float			speed= 5.0f;
	public float 			minSpeed = 0.5f;
	public float 			minInput = 0.5f;
	public float			brakeEasing = 2f;

			bool canMoveX = true;
			bool canMoveZ = true;

	void Start () {
	}
	

	void Update () {
	
	}
	
	void FixedUpdate () {
		//TO DO::::::::::::::
		//loses too much momentum when turnign or changing direction
		//need quicker startup and slower finish
		
		float xInput = Input.GetAxis("Horizontal");
		//if both x-axis keys are released or both x-axis keys are pressed at the same time while Isaac is moving, he will stop moving along the x-axis.
		bool conflictX = (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.D));
		if( Mathf.Abs(xInput) < minInput || conflictX ){
				StopX ();
				//SetXVelocity(0.0f);
				if(conflictX){
					canMoveX = false;
				}
		}
		else{
			//print ("ok to continue");
			canMoveX = true;
		}
		if( canMoveX ){
				SetXVelocity ( xInput*speed );
		}
		//y-axis stuff
		float yInput = Input.GetAxis("Vertical");
		bool conflictY = (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.S));
		if( Mathf.Abs(yInput) < minInput || conflictY ){
				StopY ();
				//SetXVelocity(0.0f);
				if(conflictY){
					canMoveZ = false;
				}
		}
		else{
			//print ("ok to continue");
			canMoveZ = true;
		}
		if( canMoveZ ){
				SetYVelocity ( yInput*speed );
		}
	

	}
	
	
	
	//---------
	void StopX (){ //stops too quickly
		bool stopped =(Mathf.Abs (GetXVelocity()) <= minSpeed);
		if(stopped) {
			SetXVelocity(0);
			//print ("stoppedX is done stopping.");
		}
		else {
			//print ("stopping.");
			float newX = (GetXVelocity ()* (1/brakeEasing))*-1;
			Vector3 xDirection = new Vector3(newX,0,0);
			rigidbody.AddForce (xDirection, ForceMode.VelocityChange);
			//the above code makes it stop instantaneously. changing forcemode to accelerate made it not stop.
		}
	}
	void SetXVelocity(float x) {
		Vector3 oppositeX = -(Vector3.Scale (rigidbody.velocity,Vector3.right));
		rigidbody.AddForce (oppositeX,ForceMode.VelocityChange);
		rigidbody.AddForce (Vector3.right*x, ForceMode.VelocityChange);
	}
	float GetXVelocity (){
		float xVel = rigidbody.velocity.x;
		return xVel;
	}
	
	/// y axis stuff
	void StopY (){ //stops too quickly
		bool stopped =(Mathf.Abs (GetYVelocity()) <= minSpeed);
		if(stopped) {
			SetYVelocity(0);
		//	print ("stoppedY is done stopping.");
		}
		else {
			//print ("stopping.");
			float newY = (GetYVelocity ()* (1/brakeEasing))*-1;
			Vector3 yDirection = new Vector3(0,newY,0);
			rigidbody.AddForce (yDirection, ForceMode.VelocityChange);
			//the above code makes it stop instantaneously. changing forcemode to accelerate made it not stop.
		}
	}
	void SetYVelocity(float y) {
		Vector3 oppositeY = -(Vector3.Scale (rigidbody.velocity,Vector3.up));
		rigidbody.AddForce (oppositeY,ForceMode.VelocityChange);//cancels out y-velocity with equal and opposite y velocity
		rigidbody.AddForce (Vector3.up*y, ForceMode.VelocityChange);//replaces with the desired y-velocity (y)
	}
	float GetYVelocity (){
		float yVel = rigidbody.velocity.y;
		return yVel;
	}
	
}



	

