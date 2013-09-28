using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class CustomCharacterController : MonoBehaviour {
	/* 
	 * Things this handles:
	 * -All player input
	 * 	-Movement
	 * 	-Jumping
	 * 		-"Mario" Jump (hold spacebar to jump higher)
	 * 		-Gravity
	 * 	-Attacking
	 * 		-Shooting
	 * 		-Swinging Sword
	 * -Handling Damage
	 */
	
	//Efficiency Variables
	private Transform thisTransform;
	private Rigidbody thisRigidbody;
	//using camera to ensure that player moves along the camera's local x-axis (for best results, only rotate camera about y-axis)
	private Transform cameraTransform;
	
	//Movement
	public float movementSpeed = 20f;
	public float accelerationRate = 0.8f;	//0 to 1
	
	//Jumping
	public float jumpVelocity = 10f;
	public static float gravity = 10f;
	public float airAccelerationRate = 0.3f;	//0 to 1
	
	//Movement & Jumping
	private bool grounded = true;
	private bool facingLeft = false;
	
	
	// Use this for initialization
	void Start () {
		Mathf.Clamp(accelerationRate, 0, 1);
		
		//set efficiency variables
		thisTransform = this.transform;
		thisRigidbody = this.rigidbody;
		cameraTransform = Camera.mainCamera.transform;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//+++++++++++++++++++++++++++++++++++++++++++++Movement+++++++++++++++++++++++++++++++++++++++++++++
		//get player velocity in x-axis
		Vector3 xVelocity = Vector3.Project(thisRigidbody.velocity, cameraTransform.right);
		
		//---------------------------------------------ground controls---------------------------------------------
		if(grounded == true)
		{
			//walk left
			if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			{
				MoveHorizontal(accelerationRate, true, xVelocity);
			}
			//walk right
			else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			{
				MoveHorizontal(accelerationRate, false, xVelocity);
			}
			//not moving left or right
			else
			{
				//stop player x movement
				thisRigidbody.velocity -= xVelocity;
			}
			
			//start jump
			if(Input.GetKeyDown(KeyCode.Space))
			{
				grounded = false;
				thisRigidbody.velocity += new Vector3(0, jumpVelocity, 0);
			}
		}
		//---------------------------------------------air controls---------------------------------------------
		else	//grounded == false
		{
			//move left in the air
			if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			{
				MoveHorizontal(airAccelerationRate, true, xVelocity);
			}
			//move right in the air
			else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			{
				MoveHorizontal(airAccelerationRate, false, xVelocity);
			}
			
			if(Input.GetKeyUp(KeyCode.Space))
			{
				//stopping jump early (if still ascending, slow ascent)
				if(thisRigidbody.velocity.y > 0)
				{
					thisRigidbody.velocity -= new Vector3(0, thisRigidbody.velocity.y/2, 0);
				}
			}
		}
		
		
		//+++++++++++++++++++++++++++++++++++++++++++++Shooting+++++++++++++++++++++++++++++++++++++++++++++
		 
	}
	
	void OnCollisionEnter(Collision collided)
	{
		//dumb for now, lets you jump again whenever you collide with anything
		grounded = true;
	}
	
	void MoveHorizontal(float acceleration, bool toMoveLeft, Vector3 xVelocity)
	{
		//turn character image left
		if(facingLeft != toMoveLeft)
		{
			facingLeft = toMoveLeft;
			
			//rotate character
			
			//don't stop player x movement
		}
		
		//do movement
		thisRigidbody.velocity -= xVelocity;
		if(toMoveLeft == true)
		{
			thisRigidbody.velocity += Vector3.Lerp(xVelocity, (cameraTransform.right * -movementSpeed), acceleration * Time.deltaTime);
		}
		else
		{
			thisRigidbody.velocity += Vector3.Lerp(xVelocity, (cameraTransform.right * movementSpeed), acceleration * Time.deltaTime);
		}
	}
}
