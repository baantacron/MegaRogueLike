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
	 * 		-Ignore above! Now is the era of Megaman X jump
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
	private float accelerationRate = 1f;	//0 to 1
	
	//Jumping
	public float jumpVelocity = 10f;
	public float gravity = 10f;
	private float airAccelerationRate = 1f;	//0 to 1
	
	//Movement & Jumping
	private bool grounded = false;
	private bool facingLeft = false;
	
	
	// Use this for initialization
	void Start () {
		Mathf.Clamp(accelerationRate, 0, 1);
		
		//set efficiency variables
		thisTransform = this.transform;
		thisRigidbody = this.rigidbody;
		cameraTransform = Camera.main.transform;
		
		//using custom gravity
		thisRigidbody.useGravity = false;
		
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
				AccelerateHorizontal(accelerationRate, true, xVelocity, movementSpeed, Time.deltaTime);
			}
			//walk right
			else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			{
				AccelerateHorizontal(accelerationRate, false, xVelocity, movementSpeed, Time.deltaTime);
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
				AccelerateHorizontal(airAccelerationRate, true, xVelocity, movementSpeed, Time.deltaTime);
			}
			//move right in the air
			else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			{
				AccelerateHorizontal(airAccelerationRate, false, xVelocity, movementSpeed, Time.deltaTime);
			}
			
			if(Input.GetKeyUp(KeyCode.Space))
			{
				//stopping jump early (if still ascending, slow ascent)
				if(thisRigidbody.velocity.y > 0)
				{
					thisRigidbody.velocity -= new Vector3(0, thisRigidbody.velocity.y/2, 0);
				}
			}
			
			//new gravity system: caps max falling speed to be same as max jumping speed
			if(thisRigidbody.velocity.y > -jumpVelocity)
			{
				if(thisRigidbody.velocity.y < -gravity)
				{
					//just a check against max speed
					thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x, -jumpVelocity, thisRigidbody.velocity.z);
				}
				else
				{
					thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x, thisRigidbody.velocity.y - (gravity * Time.deltaTime), thisRigidbody.velocity.z);
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
	
	/*
	 * Function: MoveHorizontal
	 * Returns: None
	 * Arguments:
	 * 		speed : velocity at which the target should move
	 * 		toMoveLeft : moving left? true. moving right? false.
	 * 		horizontalVelocity : player's current velocity in the direction of the main camera's x-axis
	 */
	void MoveHorizontal(float speed, bool toMoveLeft, Vector3 horizontalVelocity)
	{
		//turn character image left
		if(facingLeft != toMoveLeft)
		{
			facingLeft = toMoveLeft;
			
			//rotate character
			
			//don't stop player x movement
		}
		
		//do movement
		thisRigidbody.velocity -= horizontalVelocity;
		horizontalVelocity = horizontalVelocity.normalized;
		horizontalVelocity *= speed * Time.deltaTime;
		thisRigidbody.velocity += horizontalVelocity;
	}
	
	
	/*
	 * Function: AccelerateHorizontal
	 * Returns: None
	 * Arguments:
	 * 		acceleration : rate at which velocity will be increased towards maxSpeed
	 * 		toMoveLeft : moving left? true. moving right? false.
	 * 		horizontalVelocity : player's current velocity in the direction of the main camera's x-axis
	 * 		maxSpeed : maximum speed to move
	 */
	void AccelerateHorizontal(float acceleration, bool toMoveLeft, Vector3 horizontalVelocity, float maxSpeed, float timeStep)
	{
		//turn character image left
		if(facingLeft != toMoveLeft)
		{
			facingLeft = toMoveLeft;
			
			//rotate character
			
			//don't stop player x movement
		}
		
		//do movement
		thisRigidbody.velocity -= horizontalVelocity;
		if(toMoveLeft == true)
		{
			thisRigidbody.velocity += Vector3.Lerp(horizontalVelocity, (cameraTransform.right * -maxSpeed), acceleration * timeStep * maxSpeed);
		}
		else
		{
			thisRigidbody.velocity += Vector3.Lerp(horizontalVelocity, (cameraTransform.right * maxSpeed), acceleration * timeStep * maxSpeed);
		}
	}
}
