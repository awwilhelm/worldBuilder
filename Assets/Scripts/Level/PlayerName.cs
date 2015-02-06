using UnityEngine;
using System.Collections;

public class PlayerName : MonoBehaviour {
	
	public string playerName;
	
	
	void Awake()
	{
		if(networkView.isMine == true)
		{
			playerName = PlayerPrefs.GetString("playerName");
			
			foreach(GameObject objNameCheck in GameObject.FindObjectsOfType(typeof(GameObject)))
			{
				if(playerName == objNameCheck.name)
				{
					float x = Random.Range(0, 1000);
					playerName = "(" + x.ToString() + ")";
					
					PlayerPrefs.SetString("playerName", playerName);
				}
			}
			
			UpdateLocalGameManager(playerName);
			networkView.RPC("UpdateMyNameEverywhere", RPCMode.AllBuffered, playerName);
		}
	}
	
	void UpdateLocalGameManager(string pName)
	{
		GameObject gameManager = GameObject.Find("GameManager");
		PlayerDatabase dataScript = gameManager.GetComponent<PlayerDatabase>();
		
		dataScript.nameSet = true;
		dataScript.playerName = pName;
	}
	
	[RPC]
	void UpdateMyNameEverywhere(string pName)
	{
		gameObject.name = pName;
		playerName = pName;
		
		/*PlayerLabel labelScript = transform.GetComponent<PlayerLabel>();
		labelScript.playerName = pName;*/
	}
	
	
	
	
	
	
	
	
	
	
}
