using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
	//assumption: this object moves along forward axis
	
	private float m_speed;
	
	public float m_timeToLive;
	
	private Vector3 m_direction;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
