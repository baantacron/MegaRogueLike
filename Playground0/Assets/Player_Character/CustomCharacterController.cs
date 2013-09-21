using UnityEngine;
using System.Collections;

public class CustomCharacterController : MonoBehaviour {
	/* 
	 * Things this handles:
	 * -All player input
	 * 	-Movement
	 * 	-Jumping
	 * 		-Double Jump
	 * 		-Gravity
	 * 	-Attacking
	 * 		-Shooting
	 * 		-Swinging Sword
	 * -Handling Damage
	 */
	
	//Efficiency Variables
	private Transform thisTransform;
	private Rigidbody thisRigidbody;
	private Transform cameraTransform;
	
	//Movement
	public float movementSpeed;
	public float jumpVelocity;
	public int maxSpeed;
	public float accelerationRate;	//0 to 1
	
	//Jumping
	public static float gravity;
	private bool grounded = true;
	
	//Attacking
	private bool facingLeft = false;
	
	// Use this for initialization
	void Start () {
		movementSpeed = 15.0f;
		jumpVelocity = 5.0f;
		maxSpeed = 20;
		accelerationRate = 0.8f;
		
		//Mathf.Clamp(accelerationRate, 0, 1);
		
		gravity = 10.0f;
		
		//set efficiency variables
		thisTransform = this.transform;
		thisRigidbody = this.rigidbody;
		cameraTransform = Camera.mainCamera.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
		//Movement
		//get player velocity in x-axis
		Vector3 xVelocity = Vector3.Project(thisRigidbody.velocity, cameraTransform.right);
		
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			//turn character image left
			if(facingLeft == false)
			{
				facingLeft = true;
				
				//stop player x movement
				thisRigidbody.velocity -= xVelocity;
			}
			
			//move left
			thisRigidbody.velocity -= xVelocity;
			thisRigidbody.velocity += Vector3.Lerp(xVelocity, (cameraTransform.right * -movementSpeed), accelerationRate * Time.deltaTime);
			
		}
		else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			//turn character image left
			if(facingLeft == true)
			{
				facingLeft = false;
				
				//stop player x movement
				thisRigidbody.velocity -= xVelocity;
			}
			
			//move right
			//thisRigidbody.velocity += cameraTransform.right * (movementSpeed * Time.deltaTime);
			//thisRigidbody.velocity += cameraTransform.right * (Mathf.Lerp(thisRigidbody.velocity.x, maxSpeed, accelerationRate) * Time.deltaTime);
			thisRigidbody.velocity -= xVelocity;
			thisRigidbody.velocity += Vector3.Lerp(xVelocity, (cameraTransform.right * movementSpeed), accelerationRate * Time.deltaTime);
		}
		else	//no key is pressed
		{
			//stop player x movement
			thisRigidbody.velocity -= xVelocity;
		}
	}
}
