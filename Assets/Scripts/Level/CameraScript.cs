using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	
	private Camera myCamera;
	private Transform cameraHeadTransform;
	
	// Use this for initialization
	void Start ()
	{
		if(networkView.isMine == true)
		{
			myCamera = Camera.main;
			cameraHeadTransform = transform.FindChild("CameraHead");
		}
		
		else
		{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		myCamera.transform.position = cameraHeadTransform.position;
		myCamera.transform.rotation = cameraHeadTransform.rotation;
	}




}
