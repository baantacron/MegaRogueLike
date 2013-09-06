public 	var Reflec: AudioClip;

function OnBecameInvisible() { 
	_MEGASCRIPT.bullcount -= 1; 
	Destroy(this.gameObject); 
	
}

function OnCollisionEnter(info:Collision)
{

	var other:GameObject = info.gameObject;

	
	if (other.tag == "Invul" || other.tag == "MenemyInvul" ) {
		audio.PlayOneShot(Reflec , 3.0F); // Play sound. 
		rigidbody.velocity.x = -16; // change direction. 
		rigidbody.velocity.y = 40;
		Debug.Log("works");

	}


	if (other.tag == "Menemy"){
		Destroy(this.gameObject);
		_MEGASCRIPT.bullcount -= 1;  
	}
}


