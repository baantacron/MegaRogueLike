public var jumpVelocity:float = 13;
public var jumplimit:float = 0; 
public var runVelocity:float = 10;
public var Bullet:GameObject;
public var posDiff:Vector3;
public static var MegaPos:Vector3;
public var bulletSpeed:float = 10;
public var left:boolean = false;
public static var bullcount: int = 0; 
public var MMhit:float = 0;
public var delay:float = 15.0;
public var lasthit:float;
static public var megapoints:float = 28; 
public var megablink:float = 0; 

private var grounds:Array = new Array();

// Sound Effect variables

public 	var HealthPickup: AudioClip;
public 	var Shot: AudioClip;
public 	var Hit: AudioClip;


function Awake() {
	lasthit = -delay;
}

function Update(){
	
	//Debug.Log(bullcount);
	
	if (MMhit == 1){
		Megamanhit ();
		//Debug.Log(megapoints);
	}
	
MegaPos = transform.position;


// Bulletcount fix
	if (bullcount > 3)
 		{
 			bullcount = 3;
 		}
 	
 	if (bullcount < 0)
 		{
 			bullcount = 0; 	
 		}

	
// Jumping
	if (Input.GetKeyDown("z") && IsGrounded()) 
		{
			rigidbody.velocity.y += jumpVelocity;	
		}
	
	if (Input.GetKeyUp("z") && rigidbody.velocity.y > 0) 
		{
		 
			rigidbody.velocity.y = 0 ;	
		}	
	
	if (Input.GetKeyDown(KeyCode.LeftArrow)) 
		{
			left = true; 
		}
	
	if (Input.GetKeyDown(KeyCode.RightArrow)) 
		{
			left = false; 
		}
	
// Maximum health
	
	if (megapoints > 28 ) 
		{
			megapoints = 28; 
		}
	
}

function FixedUpdate () 
	{


//Shooting
	if (Input.GetKeyDown("x")) 
		{
			FireBullet();
		}
	
	// limit the y velocity to the range [-jumpVelocity...jumpVelocity]
// Moving Horizontally
	rigidbody.velocity.y = Mathf.Max(-jumpVelocity, Mathf.Min(rigidbody.velocity.y, jumpVelocity) );
	if (MMhit == 0 || megablink >= 30)
		{
			var horiz:float = Input.GetAxis("Horizontal"); // An axis gives me values [-1...1]
			horiz *= (runVelocity - 5); // Change from [-1...1] to [-runVelocity...runVelocity]
			rigidbody.velocity.x = horiz;
		}


	if (Input.GetKeyDown(KeyCode.RightArrow)) 
			{
				if (Input.GetKeyDown(KeyCode.LeftArrow))
				{
						rigidbody.velocity.x = runVelocity;
				}
			}
	
	}



function OnCollisionEnter(info:Collision) 
	{
		//Normalize horizontal speed
		var other:GameObject = info.gameObject;
		if (other.tag == "Ground") 
			{
				grounds.Push(other);
			}
	
		//Running in to enemies. 
	
		if (MMhit == 0)
			{
	
				if (other.tag == "Menemy" || other.tag == "MenemyInvul") 
					{
						audio.PlayOneShot(Hit , 3.0F);
 						MMhit = 1;
 						lasthit = Time.time;
 						megapoints -=1;
 						renderer.enabled = false;
 						gameObject.layer = 12;
 			
 					if (left == false) 
 						{
 							transform.localPosition += new Vector3(-0.2, 0, 0);
 							rigidbody.velocity.y += jumpVelocity-6;
 							rigidbody.velocity.x = -1;
 							horiz = 0;
 						}
 					if (left == true) 
						{
 							transform.localPosition += new Vector3(0.2, 0, 0);
 							rigidbody.velocity.y += jumpVelocity-6;
 							rigidbody.velocity.x = 1;
 							horiz = 0;
 						}
 				
 					}
 		
 		
 				// Healing 
 				if (other.tag == "Health") 
 				{
 						if ( megapoints <= 28) 
 						{
 							audio.PlayOneShot(HealthPickup , 3.0F);
 							megapoints +=5;
 						}	
 				}
 				
 				if (other.tag == "BigHealth") 
 				{
 						if ( megapoints <= 28) 
 						{
 							audio.PlayOneShot(HealthPickup , 3.0F);
 							megapoints +=15;
 						}	
 				}		
 	
			} 
	
	
}

function OnCollisionExit(info:Collision) 
{
	var other:GameObject = info.gameObject;
	if (other.tag == "Ground") 	
	{
		// remove this GameObject from the grounds list
		for (var i=grounds.length-1; i>=0; i--) 
		{
			if (grounds[i] == other) 
			{
				grounds.RemoveAt(i);
			}
		}
	}
}

function IsGrounded():boolean 
	{
		return(grounds.length != 0 );

	}

 function FireBullet() 
 {
 	if (bullcount <= 2)
 	{
 		audio.PlayOneShot(Shot , 3.0F);
 		bullcount +=1;
 		
 		if (left == false) 
 		{
 			var bull:GameObject = Instantiate(Bullet, transform.position+posDiff, Quaternion.identity);
 			bull.rigidbody.velocity = Vector3(bulletSpeed,0,0);
 			
		} 
		
		if (left == true) 
		{
			var boll:GameObject = Instantiate(Bullet, transform.position-posDiff, Quaternion.identity);
 			boll.rigidbody.velocity = Vector3(-bulletSpeed,0,0);
		}
	}
 	 	
 }
 
 
 function Megamanhit () 
 {
	megablink += 1;

	var oddeven = (megablink % 2) == 0;
	renderer.enabled = oddeven;
	
	var t:float = Time.time - lasthit;
	if (t >= delay) 
	{
		lasthit += delay;
		 MMhit = 0;
		 //Debug.Log("poop");
		 renderer.enabled = true;
		 megablink = 0;
		 rigidbody.velocity.x = 0 ;
		 gameObject.layer = 10;
	}
	
 }
