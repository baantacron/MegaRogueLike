using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBulletManager {
	
	//maximum number of player bullets in game at once (set in inspector)
	public int u_maxBullets = 3;
	
	//initial speed of each bullet
	public double u_bulletSpeed = 1.0;
	
	//instance of bullet
	public GameObject u_bullet;
	
	//list of bullets
	private List<Bullet> m_bullets;
	
	//singleton reference (should not be accessed)
	private static PlayerBulletManager m_self = null;
	//singleton getter/instanciator (call this instead)
	public static PlayerBulletManager getInstance()
	{
		if(m_self == null)
		{
			//create new instance and set it
			m_self = new PlayerBulletManager();
			//m_self = (PlayerBulletManager)Instantiate(PlayerBulletManager, Vector3.zero, Quaternion.identity);
			m_self.Init();
		}
		return m_self;
	}
	
	private void Init()
	{
		//initialize things that need initializing
		m_bullets = new List<Bullet>();
		
		//add self as listener for dead bullets
		Messenger.AddListener<Bullet>("playerBulletDestroyedEvt", BulletDestroyed);
	}
	
	//initializes a bullet with the default speed
	public void SpawnBullet(Vector3 startPos, Vector3 target, GameObject bulletPrefab)
	{
		//checks to see if we're at capacity for bullets
		if(m_bullets.Count < u_maxBullets)
		{
			//create bullet
			GameObject newBullet = (GameObject)GameObject.Instantiate(bulletPrefab, startPos, Quaternion.identity);
			//TODO: correctly rotate bullet
			Bullet bulletScript = (Bullet)(newBullet.GetComponent(typeof(Bullet)));
			bulletScript.m_speed = u_bulletSpeed;
			bulletScript.m_direction = target;
			
			//add new bullet to bullet list
			m_bullets.Add(bulletScript);
			
			Debug.Log("Bullet created. Current bullet count = " + m_bullets.Count + " / " + u_maxBullets);
		}
		else
		{
			//don't create bullet
			Debug.Log("Already at maximum bullets");
		}
	}
	
	//called to notify when a bullet has been destroyed
	public void BulletDestroyed(Bullet deadBullet)
	{
		//remove bullet from bullet list
		if(deadBullet != null)
		{
			Debug.Log("Standard bullet removal");
			m_bullets.Remove(deadBullet);
		}
		else
		{
			Debug.Log("NULL bullet removal");
			for(int i = 0; i < m_bullets.Count; i++)
			{
				if(m_bullets[i] == null)
				{
					m_bullets.RemoveAt(i);
					break;
				}
			}
		}
		
		Debug.Log("Bullet removed. Current bullet count = " + m_bullets.Count + " / " + u_maxBullets);
	}
}
