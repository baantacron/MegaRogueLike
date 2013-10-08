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
	
	//move reticule closer or further from the camera
	public float distanceFromNearClip;
	
	//if your reticule's up should be facing the camera, set this to true
	public bool upIsForward = false;
	
	//controls if the reticule will rotate to face the camera (true = does not rotate)
	public bool perspective = true;
	
	// Use this for initialization
	void Start () {
		if(reticule == null)
		{
			this.enabled = false;
		}
		else
		{
			this.enabled = true;
			reticuleTransform = reticule.transform;
			cameraTransform = Camera.main.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(perspective == false)
		{
			//Make sure reticule is facing camera
			reticuleTransform.LookAt(cameraTransform.position);
			if(upIsForward)
			{
				reticuleTransform.Rotate(reticuleTransform.right, PI/2);
			}
		}
		
		//move reticlue to mouse
		reticuleTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane+distanceFromNearClip));
		//note: it's gonna be really close to the camera so make it small
		
	}
}
