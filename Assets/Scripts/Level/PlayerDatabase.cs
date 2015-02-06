using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerDatabase : MonoBehaviour {

	public List<PlayerDataClass> PlayerList = new List<PlayerDataClass>();
	public NetworkPlayer networkPlayer;
	public string playerName;
	public bool nameSet = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(nameSet == true)
		{
			networkView.RPC("EditPlayerListWithName", RPCMode.AllBuffered, Network.player, playerName);
			nameSet = false;
		}
	}

	void OnPlayerConnected(NetworkPlayer netPlayer)
	{
		networkView.RPC ("AddPlayerToList", RPCMode.AllBuffered, netPlayer);
	}

	void OnPlayerDisconnected(NetworkPlayer netPlayer)
	{
		networkView.RPC ("RemovePlayerFromList", RPCMode.AllBuffered, netPlayer);
	}

	[RPC]
	void AddPlayerToList(NetworkPlayer nPlayer)
	{
		PlayerDataClass capture = new PlayerDataClass ();
		capture.networkPlayer = int.Parse (nPlayer.ToString ());
		PlayerList.Add (capture);
	}

	[RPC]
	void RemovePlayerFromList(NetworkPlayer nPlayer)
	{
		for(int i=0; i<PlayerList.Count; i++)
		{
			if(PlayerList[i].networkPlayer == int.Parse(nPlayer.ToString()))
			{
				PlayerList.RemoveAt(i);
			}
		}
	}

	[RPC]
	void EditPlayerListWithName (NetworkPlayer nPlayer, string pName)
	{
		for(int i=0; i<PlayerList.Count; i++)
		{
			if(PlayerList[i].networkPlayer == int.Parse(nPlayer.ToString()))
			{
				PlayerList[i].playerName = pName;
			}
		}
	}





}
