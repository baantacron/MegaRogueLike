using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
	//assumption: this object moves along forward axis
	
	public float m_speed;
	public Vector3 m_direction;
	
	//die after a bit
	public float u_timeToLive = 3f;
	private float m_timeToDie;
	
	//efficiency variables
	private Transform thisTransform;
	private Rigidbody thisRigidbody;
	
	
	// Use this for initialization
	void Start () {
		thisTransform = this.transform;
		thisRigidbody = this.rigidbody;
		thisRigidbody.useGravity = false;
		
		m_timeToDie = Time.time + u_timeToLive;
		
		//handle facing
	}
	
	// Update is called once per frame
	void Update () {
		thisTransform.position += m_direction * m_speed;
		
		if(Time.time >= m_timeToDie)
		{
			Destroy(this.gameObject);
		}
	}
	
	void OnCollisionEnter(Collision collided)
	{
		//yup, just die
		Destroy(this.gameObject);
	}
}
