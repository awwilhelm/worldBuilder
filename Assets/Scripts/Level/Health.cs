using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	private int maxHealth;
	public int health;
	public NetworkPlayer attacker;

	public Texture healthTex;

	public Material playerColor;
	public Material playerHitColor;
	private bool hit = false;
	public float healthPercent;

	// Use this for initialization
	void Start ()
	{
		if(networkView.isMine)
		{
			maxHealth = 100;
			health = maxHealth;
			healthPercent = 100;
			print ("health " + health);
			networkView.RPC("UpdateHealth", RPCMode.AllBuffered, health);
		}

		else
		{
			enabled = false;
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
		healthPercent = (float)health / maxHealth * 100.0f;

		if(healthPercent>100)
		{
			healthPercent = 100;
		}
		else if(healthPercent<1)
		{
			healthPercent = 1;
		}


		if(hit)
		{
			StartCoroutine(FlashHit());
			networkView.RPC("HitChange", RPCMode.AllBuffered, false);
		}

		if(health<=0)
		{
			Spawn spawnScript = GameObject.Find("SpawnManager").GetComponent<Spawn>();
			spawnScript.iAmDestroyed = true;

			Network.RemoveRPCs(Network.player);
			networkView.RPC("DestroySelf", RPCMode.All);
		}
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
		if(health < 0)
			health = 0;
		networkView.RPC("UpdateHealth", RPCMode.AllBuffered, health);
		networkView.RPC("HitChange", RPCMode.AllBuffered, true);
	}

	[RPC]
	void HitChange(bool tempHit)
	{
		hit = tempHit;

	}

	[RPC]
	void UpdateHealth(int tempHealth)
	{
		health = tempHealth;
	}

	IEnumerator FlashHit()
	{
		networkView.RPC("UpdatePlayerHitColor", RPCMode.AllBuffered, transform.name);
		yield return new WaitForSeconds(0.1f);
		networkView.RPC("UpdatePlayerColor", RPCMode.AllBuffered, transform.name);
	}

	[RPC]
	void UpdatePlayerHitColor(string name)
	{
		GameObject.Find("Graphics").renderer.material = playerHitColor;
	}

	[RPC]
	void UpdatePlayerColor(string name)
	{
		GameObject.Find("Graphics").renderer.material = playerColor;
	}

	[RPC]
	void DestroySelf()
	{
		Destroy(transform.parent.gameObject);
	}










}
