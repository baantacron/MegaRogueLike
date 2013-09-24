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
	public float jumpVelocity = 10f;
	public int maxSpeed = 20;
	public float accelerationRate = 0.8f;	//0 to 1
	
	//Jumping
	public static float gravity = 10f;
	private bool grounded = true;
	
	//Attacking
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
		
		////Movement
		//get player velocity in x-axis
		Vector3 xVelocity = Vector3.Project(thisRigidbody.velocity, cameraTransform.right);
		
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			//ground controls (same as air controls)
			//turn character image left
			if(facingLeft == false)
			{
				facingLeft = true;
				
				//rotate character
				
				//stop player x movement
				thisRigidbody.velocity -= xVelocity;
			}
			
			//do movement
			thisRigidbody.velocity -= xVelocity;
			thisRigidbody.velocity += Vector3.Lerp(xVelocity, (cameraTransform.right * -movementSpeed), accelerationRate * Time.deltaTime);
			
			
		}
		else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			//ground controls (same as air controls)
			//turn character image left
			if(facingLeft == true)
			{
				facingLeft = false;
				
				//rotate character
				
				//stop player x movement
				thisRigidbody.velocity -= xVelocity;
			}
			
			//do movement
			thisRigidbody.velocity -= xVelocity;
			thisRigidbody.velocity += Vector3.Lerp(xVelocity, (cameraTransform.right * movementSpeed), accelerationRate * Time.deltaTime);
		}
		else	//no key is pressed
		{
			if(grounded)
			{
				//stop player x movement
				thisRigidbody.velocity -= xVelocity;
			}
		}
		
		if(Input.GetKeyDown(KeyCode.Space))
		{
			//start jump
			if(grounded)
			{
				grounded = false;
				thisRigidbody.velocity += new Vector3(0, jumpVelocity, 0);
			}
			
		}
		else if(Input.GetKeyUp(KeyCode.Space))
		{
			//stopping jump early
			thisRigidbody.velocity -= new Vector3(0, thisRigidbody.velocity.y/2, 0);
		}
		
		
		
	}
	
	void OnCollisionEnter(Collision collided)
	{
		//dumb for now, lets you jump again whenever you collide with anything
		grounded = true;
	}
}
