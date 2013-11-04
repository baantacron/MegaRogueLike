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
	private Vector3 m_velocity;		//alternative to thisRigidbody.velocity
	//using camera to ensure that player moves along the camera's local x-axis (for best results, only rotate camera about y-axis)
	private Transform cameraTransform;
	public string u_groundTag = "Ground";
	public string u_wallTag = "Wall";
	
	//Movement
	public float u_movementSpeed = 10f;
	private float u_accelerationRate = 1f;	//0 to 1
	
	//Jumping
	public float u_jumpVelocity = 15f;
	public float u_gravity = 0.8f;
	private float u_airAccelerationRate = 0.8f;	//0 to 1
	
	//WallJumping
	public float u_wallJumpBoost = 15f;
	
	//Movement & Jumping
	public enum MoveState {Grounded, Airborne, WallStuck, ERROR};
	private MoveState m_MoveState
	{
		get
		{
			return m_moveState;
		}
		set
		{
			m_moveState = value;
			StateChanged(value);
		}
	}
	private MoveState m_moveState;
	private bool m_facingLeft = false;
	
	private bool m_hasControl = true;
	
	//Shooting
	public GameObject u_bullet;
	public float u_bulletSpeed = 1f;
	
	// Use this for initialization
	void Start () {
		Mathf.Clamp(u_accelerationRate, 0, 1);
		
		//set efficiency variables
		thisTransform = this.transform;
		thisRigidbody = this.rigidbody;
		thisRigidbody.velocity = new Vector3();
		cameraTransform = Camera.main.transform;
		
		//using custom gravity
		thisRigidbody.useGravity = false;
		
		//initialize player as airborne and let them fall.
		m_MoveState = MoveState.Airborne;
		
		m_hasControl = true;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//++++++++++++++++++++++++++++++++++++++++++++++++Movement++++++++++++++++++++++++++++++++++++++++++++++++
		//get player velocity in x-axis
		Vector3 xVelocity = Vector3.Project(thisRigidbody.velocity, cameraTransform.right);
		
		//---------------------------------------------ground controls---------------------------------------------
		if(m_MoveState == MoveState.Grounded)
		{
			print("State = Grounded");
			
			if(m_hasControl)
			{
				//walk left
				if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
				{
					AccelerateHorizontal(u_accelerationRate, true, xVelocity, u_movementSpeed, Time.deltaTime);
				}
				//walk right
				else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
				{
					AccelerateHorizontal(u_accelerationRate, false, xVelocity, u_movementSpeed, Time.deltaTime);
				}
				else
				{
					//not moving
					thisRigidbody.velocity = Vector3.zero;
				}
				
				//start jump
				if(Input.GetKeyDown(KeyCode.Space))
				{
					thisRigidbody.velocity += new Vector3(0, u_jumpVelocity, 0);
					m_MoveState = MoveState.Airborne;
				}
			}
		}
		//---------------------------------------------air controls---------------------------------------------
		else if(m_MoveState == MoveState.Airborne)
		{
			print("State = Airborne");
			
			if(m_hasControl)
			{
				//move left in the air
				if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
				{
					AccelerateHorizontal(u_airAccelerationRate, true, xVelocity, u_movementSpeed, Time.deltaTime);
				}
				//move right in the air
				else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
				{
					AccelerateHorizontal(u_airAccelerationRate, false, xVelocity, u_movementSpeed, Time.deltaTime);
				}
				
				if(Input.GetKeyUp(KeyCode.Space))
				{
					//stopping jump early (if still ascending, slow ascent)
					if(thisRigidbody.velocity.y > 0)
					{
						thisRigidbody.velocity += new Vector3(0, -(thisRigidbody.velocity.y/2), 0);
					}
				}
			}
			
			//new gravity system: caps max falling speed to be same as max jumping speed
			if(thisRigidbody.velocity.y > -u_jumpVelocity)
			{
				thisRigidbody.velocity += new Vector3(0, -u_gravity, 0);
			}
			if(thisRigidbody.velocity.y <= -u_jumpVelocity)
			{
				thisRigidbody.velocity = new Vector3(0, -u_jumpVelocity, 0);
			}
		}
		//---------------------------------------------wallslide controls---------------------------------------------
		else if(m_MoveState == MoveState.WallStuck)
		{
			print("State = WallStuck");
			
			//for now, just apply gravity
			if(thisRigidbody.velocity.y > -u_jumpVelocity/2f)
			{
				thisRigidbody.velocity += new Vector3(0,  -(u_gravity * Time.deltaTime), 0);
			}
			else	//velocity.y <= -jumpVel
			{
				thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x, -u_jumpVelocity/2f, thisRigidbody.velocity.z);
			}
			
			//wall jump
			if(Input.GetKeyDown(KeyCode.Space))
			{
				if(m_facingLeft)
				{
					//push off and up to the left
					thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x - u_wallJumpBoost, u_jumpVelocity, thisRigidbody.velocity.z);
				}
				else
				{
					//push off and up to the right
					thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x + u_wallJumpBoost, u_jumpVelocity, thisRigidbody.velocity.z);
				}
				m_MoveState = MoveState.Airborne;
			}
		}
		
		//---------------------------------------------(ground || air) movement---------------------------------------------
		if(m_MoveState == MoveState.Grounded || m_MoveState == MoveState.Airborne)
		{
			//not moving left or right
			if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
			{
				//stop player x movement
				thisRigidbody.velocity -= xVelocity;
			}
		}
		
		
		//++++++++++++++++++++++++++++++++++++++++++++++++Shooting++++++++++++++++++++++++++++++++++++++++++++++++
		if(Input.GetMouseButtonDown(0))
		{
			//mouse-controlled shooting: shoot at position of click
			Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			target -= thisTransform.position;	//gets a vector from player to mouse position
			target.z = 0;	//only moving in x-y plane
			
			target = Vector3.Normalize(target);
			GameObject newBullet = (GameObject)Instantiate(u_bullet);
			newBullet.transform.position = thisTransform.position;
			Bullet bulletScript = (Bullet)(newBullet.GetComponent(typeof(Bullet)));
			if(!m_facingLeft)
				bulletScript.m_speed = u_bulletSpeed;
			else
				bulletScript.m_speed = u_bulletSpeed;
			
			bulletScript.m_direction = target;
			
			//TODO: bullet manager
		}
	}
	
	void OnCollisionEnter(Collision collided)
	{
		//landing on ground
		if(collided.gameObject.tag == u_groundTag || collided.gameObject.tag == u_wallTag)
		{
			//don't need to waste time re-grounding ourself
			if(m_MoveState != MoveState.Grounded)
			{
				//checking if we collided on our feet, on the floor
				if(collided.contacts.Length > 0)	//check to make sure it wasn't already destroyed or something
				{
					for(int i = 0; i < collided.contacts.Length; i++)
					{
						ContactPoint contact = collided.contacts[i];
						if(Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
						{
							//landed on feet
							m_MoveState = MoveState.Grounded;
							break;
						}
					}
					
					//making the tagging between wall and graound irrelevant
					for(int i = 0; i < collided.contacts.Length; i++)
					{
						ContactPoint contact = collided.contacts[i];
						float dotProd = Vector3.Dot(contact.normal, Vector3.up);
						if(Mathf.Abs(dotProd) < 0.5f)
						{
							//collided with the side of a platform, activate wallslide
							m_MoveState = MoveState.WallStuck;
							break;
						}
					}
				}
			}
			//can only start walljumping when you're in the air (and not already walljumping)
			else if(m_MoveState == MoveState.Airborne)
			{
				//make sure we collide with walls to the sides of the player
				if(collided.contacts.Length > 0)
				{
					for(int i = 0; i < collided.contacts.Length; i++)
					{
						ContactPoint contact = collided.contacts[i];
						float dotProd = Vector3.Dot(contact.normal, Vector3.up);
						if(Mathf.Abs(dotProd) < 0.5f)
						{
							if(dotProd < 0)
							{
								//wall on right, face left
								m_MoveState = MoveState.WallStuck;
								m_facingLeft = true;
								break;
							}
							else
							{
								//wall on left, face right
								m_MoveState = MoveState.WallStuck;
								m_facingLeft = false;
								break;
							}
						}
					}
				}
			}
		}
		//wall jumping
		/*else if(collided.gameObject.tag == u_wallTag)
		{
			//can only start walljumping when you're in the air (and not already walljumping)
			if(m_MoveState == MoveState.Airborne)
			{
				//make sure we collide with walls to the sides of the player
				if(collided.contacts.Length > 0)
				{
					for(int i = 0; i < collided.contacts.Length; i++)
					{
						ContactPoint contact = collided.contacts[i];
						float dotProd = Vector3.Dot(contact.normal, Vector3.up);
						if(Mathf.Abs(dotProd) < 0.5f)
						{
							if(dotProd < 0)
							{
								//wall on right, face left
								m_MoveState = MoveState.WallStuck;
								m_facingLeft = true;
								break;
							}
							else
							{
								//wall on left, face right
								m_MoveState = MoveState.WallStuck;
								m_facingLeft = false;
								break;
							}
						}
					}
				}
			}
		}*/
		
	}
	
	void OnCollisionExit(Collision collided)
	{
		//just left the ground: jumping, falling, or special case for wallslide
		if(collided.gameObject.tag == u_groundTag)
		{
			//don't need to waste time re-grounding ourself
			if(m_MoveState == MoveState.Grounded)
			{
				//checking if we collided on our feet, on the floor
				if(collided.contacts.Length == 1)
				{
					for(int i = 0; i < collided.contacts.Length; i++)
					{
						ContactPoint contact = collided.contacts[i];
						if(Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
						{
							//left the ground: jumping, falling
							m_MoveState = MoveState.Airborne;
							break;
						}
					}
				}
			}
			else if(m_MoveState == MoveState.WallStuck)
			{
				if(collided.contacts.Length == 0)
				{
					//special case for wallslide: just fell off side of ground platform, has not jumped
					m_MoveState = MoveState.Airborne;
					
					//TODO: Fix Spiderman Error
				}
				else
				{
					//special case for wallslide: slid down wall to ground
					m_MoveState = MoveState.Grounded;
				}
			}
		}
		//if you have finished sliding off a wall without touching the ground
		else if(collided.gameObject.tag == u_wallTag && m_MoveState != MoveState.Grounded)
		{
			m_MoveState = MoveState.Airborne;
		}
		else if(collided.gameObject.tag == "Default")
		{
			//TODO: needs testing + further implementation
			
			//checking if we just left the floor, not a wall
			/*if(collided.contacts.Length > 0)	//check to make sure it wasn't already destroyed or something
			{
				for(int i = 0; i < collided.contacts.Length; i++)
				{
					ContactPoint contact = collided.contacts[i];
					if(Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
					{
						//left the floor
						m_MoveState = MoveState.Airborne;
						break;
					}
				}
			}*/
		}
	}
	
	private void StateChanged(MoveState newState)
	{
		if(newState == MoveState.Grounded)
		{
			thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x, 0, thisRigidbody.velocity.z);
		}
		else if(newState == MoveState.Airborne)
		{
			
		}
		else if(newState == MoveState.WallStuck)
		{
			thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x, 0, thisRigidbody.velocity.z);
		}
	}
	
	/*
	 * Function: MoveHorizontal
	 * Returns: None
	 * Arguments:
	 * 		speed : velocity at which the target should move
	 * 		toMoveLeft : moving left? true. moving right? false.
	 * 		horizontalVelocity : player's current velocity in the direction of the main camera's x-axis
	 */
	private void MoveHorizontal(float speed, bool toMoveLeft, Vector3 horizontalVelocity)
	{
		//turn character image left
		if(m_facingLeft != toMoveLeft)
		{
			m_facingLeft = toMoveLeft;
			
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
		if(m_facingLeft != toMoveLeft)
		{
			m_facingLeft = toMoveLeft;
			
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


/*
 * if UN-collided with Ground
 * 		if already grounded
 * 			-NOT jumped: would be airborne already
 * 			-just fell off ground platform (normative): set to airborne
 * 			-just moved away from low-hanging platform: stay grounded
 * 			set airborne, only if platform was at feet
 * 		if airborne
 * 			-just left ground by jumping: stay airborne
 * 			-just left wall by walljumping: stay airborne
 * 			-just fell off ground (normative)
 * 			who cares
 * 		if wallslide
 * 			-
 */

