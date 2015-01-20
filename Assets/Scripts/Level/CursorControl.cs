using UnityEngine;
using System.Collections;

public class CursorControl : MonoBehaviour {

	private GameObject multiplayerManager;
	private Multiplayer multiScript;
	
	// Use this for initialization
	void Start ()
	{
		if(networkView.isMine == true)
		{
			multiplayerManager = GameObject.Find("MultiplayerManager");
			multiScript = multiplayerManager.GetComponent<Multiplayer>();
		}
		
		else
		{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(multiScript.showDisconnectWindow == false)
		{
			Screen.lockCursor = true;
		}
		
		if(multiScript.showDisconnectWindow == true)
		{
			Screen.lockCursor = false;
		}
	}
}
