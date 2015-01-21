using UnityEngine;
using System.Collections;

public class SpawnPtScript : MonoBehaviour {

	public bool taken;

	// Use this for initialization
	void Start () {
		taken = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CallChangeTaken(bool take)
	{
		networkView.RPC ("ChangeTaken", RPCMode.AllBuffered, take);
	}

	[RPC]
	void ChangeTaken(bool take)
	{
		taken = take;
	}



}
