using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
	//assumption: this object moves along forward axis
	
	public double m_speed;
	public Vector3 m_direction;
	
	//die after a bit
	public float u_timeToLive = 1.5f;
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
		Debug.Log("Current Time: " + Time.time + "\nTime to Die: " + m_timeToDie);
		
		//handle facing?
	}
	
	// Update is called once per frame
	void Update () {
		thisTransform.position += m_direction * (float)m_speed;
		
		if(Time.time >= m_timeToDie)
		{
			KillSelf();
		}
	}
	
	void OnCollisionEnter(Collision collided)
	{
		//yup, just die
		KillSelf();
	}
	
	//pretty self explanatory, but it also sends an event to the bulletmanager
	void KillSelf()
	{
		Messenger.Broadcast<Bullet>("playerBulletDestroyedEvt", this);
		Destroy(this.gameObject);
	}
}
