using UnityEngine;
using System.Collections;

public class MovementUpdate : MonoBehaviour {

	private Vector3 lastPosition;
	private Quaternion lastRotation;
	private Transform myTransform;

	
	// Use this for initialization
	void Start ()
	{
		if(networkView.isMine == true)
		{
			myTransform = transform;
			
			networkView.RPC ("updateMovement", RPCMode.OthersBuffered, myTransform.position, myTransform.rotation, myTransform.localScale);
		}
		
		else
		{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(networkView.isMine == true)
		{
			if(Vector3.Distance(myTransform.position, lastPosition) >= 0.1f)
			{
				//lastPosition = myTransform.position;
				networkView.RPC ("updateMovement", RPCMode.OthersBuffered, myTransform.position, myTransform.rotation, myTransform.localScale);
			}
			
			if(Quaternion.Angle(myTransform.rotation, lastRotation) >= 1)
			{
				//lastRotation = myTransform.rotation;
				networkView.RPC ("updateMovement", RPCMode.OthersBuffered, myTransform.position, myTransform.rotation, myTransform.localScale);
			}
		}

		else
		{
			transform.position = Vector3.Lerp(lastPosition, transform.position, Time.deltaTime);
			transform.rotation = Quaternion.Lerp(lastRotation, transform.rotation, Time.deltaTime);
		}
	}
	
	[RPC]
	void updateMovement(Vector3 newPosition, Quaternion newRotation, Vector3 newScale)
	{
		transform.position = newPosition;
		//transform.position = Vector3.Lerp(lastPosition, newPosition, 0.1f);
		transform.rotation = newRotation;
		transform.localScale = newScale;
		lastPosition = newPosition;
		lastRotation = newRotation;
	}
	





}
