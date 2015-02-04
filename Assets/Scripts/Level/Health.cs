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


			networkView.RPC("DestroySelf", RPCMode.AllBuffered);
		}
	}

	public void TakeDamage(int damage)
	{
		//This is called by the person who fires.. Weird.
		print ("Called");
		health -= damage;
		if(health < 0)
			health = 0;
		else
		{
			networkView.RPC("UpdateHealth", RPCMode.AllBuffered, health);
			networkView.RPC("HitChange", RPCMode.AllBuffered, true);
		}
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
		//print (gameObject.transform.parent.name);
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
