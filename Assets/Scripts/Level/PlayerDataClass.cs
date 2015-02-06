using UnityEngine;
using System.Collections;

public class PlayerDataClass
{

	public int networkPlayer;
	public string playerName;

	public PlayerDataClass Constructor()
	{
		PlayerDataClass capture = new PlayerDataClass ();
		capture.networkPlayer = networkPlayer;
		capture.playerName = playerName;

		return capture;
	}



}
