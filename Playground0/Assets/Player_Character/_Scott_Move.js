#pragma strict

// ==== Buttons ====
// "Space" currently jumps. 
// I'm using a 

// ==== Character Variables ====
public var jumpVelocity:float = 7; // Jumping speed. #Jump1 , #Jump2
public var MMhit:float = 0; // Damage taken. 
public var runVelocity:float = 3; // The movement of speed. #Run1
public var megablink:float = 0; // #InvincibilityFrames1
public var left:boolean = false; // #Direction1

// ==== Weapon Variables ====
public var gen_sword:boolean = true;

// ==== Other game objects ====
public var Bullet:GameObject;
public var posDiff:Vector3;
public var bulletSpeed:float = 10;

// ==== System Stuff ====
private var grounds:Array = new Array(); // Ground Array for jumping / ground collision. #Basic1

function Start () {

}

function Update () { // (includes jumping, and movement). 

// ==== Update Jump ==== 
	
	if (Input.GetKeyDown("space")) // For the initial jump. 
		{
			rigidbody.velocity.y += jumpVelocity;	// #Jump1
		}
	
	if (Input.GetKeyUp("space") && rigidbody.velocity.y > 0) // For stopping the jump when the key is no longer held down. #Jump2
		{
			rigidbody.velocity.y = (rigidbody.velocity.y / 2) ;	// For doing so cleanly, and not as if it were Sonic 06'. 
		}
		
// ==== Update Movement ====

	rigidbody.velocity.y = Mathf.Max(-jumpVelocity, Mathf.Min(rigidbody.velocity.y, jumpVelocity) );
		
	if (MMhit == 0 || megablink >= 30) // #InvincibilityFrames1
		{
			var horiz:float = Input.GetAxis("Horizontal"); // An axis gives me values [-1...1] #Horiz1
			horiz *= (runVelocity - 0); // Change from [-1...1] to [-runVelocity...runVelocity] #Run1
			rigidbody.velocity.x = horiz; // #Horiz1
		}

// ==== Directional Movement ====
	if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown('a')) // #Direction1
		{
			left = true; 
			//DirectionRotate();
		}
	
	if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown('d')) // #Direction1
		{
			left = false;
			//DirectionRotate();
		}

// ==== Sword Swing ==== 

	if (gen_sword == true)
		{
			gen_sword = false; 
		}
}

 function FireBullet() // This is what actually fires the bullet. 
 {
 		if (left == false) // #Direction1
 		{
 			var bull:GameObject = Instantiate(Bullet, transform.position+posDiff, Quaternion.identity);
 			bull.rigidbody.velocity = Vector3(bulletSpeed,0,0);
 			
		} 
		
		if (left == true) // #Direction1
		{
			var boll:GameObject = Instantiate(Bullet, transform.position-posDiff, Quaternion.identity);
 			boll.rigidbody.velocity = Vector3(-bulletSpeed,0,0);
		} 	
 } 
 
 function DirectionRotate()
 {
 	if ( left == true)
		{
			transform.Rotate(Time.deltaTime, 180, 0);
		}
	
	if ( left == false)
		{
			transform.Rotate(Time.deltaTime, 180, 0);
		}

 }
 