﻿using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	private bool justConnectedToServer;
	private Rect joinRect;
	public bool spawned = false;
	
	public GameObject player;
	public World worldScript;
	public bool iAmDestroyed;

	// Use this for initialization
	void Start ()
	{
		worldScript = GameObject.Find("World").GetComponent<World>();
		iAmDestroyed = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnConnectedToServer()
	{
		justConnectedToServer = true;
	}

	void JoinTeamWindow(int windowID)
	{
		if(justConnectedToServer == true || iAmDestroyed == true)
		{
			if(GUILayout.Button("Spawn", GUILayout.Height(40 * 2)))
			{
				Network.Instantiate(player, new Vector3 (25, 25, 25), Quaternion.identity, 0);	
				justConnectedToServer = false;
				iAmDestroyed = false;
				spawned = true;
			}
		}
		
	}

	void OnGUI()
	{
		if(justConnectedToServer == true && Network.isClient || iAmDestroyed == true)
		{
			Screen.lockCursor = false;

			joinRect = new Rect(Screen.width / 2 - 330 / 2, Screen.height / 2 - 100 / 2, 330, 100);
			joinRect = GUILayout.Window(0, joinRect, JoinTeamWindow, "Join");
		}
	}












}