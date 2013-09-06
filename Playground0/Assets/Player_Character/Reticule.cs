using UnityEngine;
using System.Collections;

public class Reticule : MonoBehaviour {
	
	//constants
	private const float PI = 3.1415f;
	
	//efficiency variables
	private Transform reticuleTransform;
	private Transform cameraTransform;
	private RaycastHit hit;
	
	//set this in inspector for your reticule
	public GameObject reticule;
	
	//if your reticule's up should be facing the camera, set this to true
	public bool upIsForward = false;
	
	// Use this for initialization
	void Start () {
		this.enabled = true;
		reticuleTransform = reticule.transform;
		cameraTransform = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		//Make sure reticule is facing camera
		reticuleTransform.LookAt(cameraTransform.position);
		if(upIsForward)
		{
			reticuleTransform.RotateAround(reticuleTransform.right, PI/2);
		}
		
		//move reticlue to mouse
		reticuleTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane+0.01f));
		//note: it's gonna be really close to the camera so make it small
		
	}
}
