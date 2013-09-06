function Update () {
	var v=Input.GetAxis("Horizontal");//This is just to show off the player. Rotate him around.
	//I also have the player parented to the platform. So he will spin with it.
	transform.Rotate(transform.up*v*-100*Time.deltaTime);
}