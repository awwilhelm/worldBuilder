﻿using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

	private float health;
	private float maxHealth;
	private float healthPercent;

	void Awake()
	{
		if(networkView.isMine)
		{
			networkView.RPC ("UpdateHealth", RPCMode.AllBuffered, 100.0f);
			maxHealth = 100;
		}
		else
		{
			enabled = false;
		}
	}

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

		if(health<=0)
		{
			Spawn spawnScript = GameObject.Find("SpawnManager").GetComponent<Spawn>();
			spawnScript.iAmDestroyed = true;

			networkView.RPC("DestroySelf", RPCMode.AllBuffered);
		}
	}

	public void TakeDamage(int damage)
	{
		float newHealth = health - damage;
		networkView.RPC ("UpdateHealth", RPCMode.AllBuffered, newHealth);
	}

	public float GetHealthPercentage ()
	{
		return healthPercent;
	}

	[RPC]
	void UpdateHealth (float tempHealth)
	{
		health = tempHealth;
	}

	[RPC]
	void DestroySelf()
	{
		//Might have to change myPlayer to something else.  Everything works until the person dies..
		//It seems that the trigger lookup is not being deleted!
		
		Network.RemoveRPCs(GameObject.FindGameObjectWithTag("Player").transform.Find("CameraHead").networkView.viewID);
		Network.RemoveRPCs(Network.player);
		Network.RemoveRPCs(GameObject.FindGameObjectWithTag("Player").networkView.viewID);
		//Network.RemoveRPCs (GameObject.FindGameObjectWithTag ("Player").GetComponents<NetworkView> ()[0].networkView);
		//Network.DestroyPlayerObjects(Network.player);
		//Destroy(GameObject.FindGameObjectWithTag("Player").gameObject);
		Network.Destroy (transform.networkView.viewID);
		Network.Destroy (transform.parent.gameObject);
	}





}
