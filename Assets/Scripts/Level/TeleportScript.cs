using UnityEngine;
using System.Collections;

public class TeleportScript : MonoBehaviour {

	private bool once = false;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	void OnTriggerEnter(Collider collision)
	{
		if (collision.tag == "ParticalCollider")
		{
			if(once == false)
			{
				if(collision.transform.parent.name == "tpLocation")
				{
					//need to make game object with myPlayer tag
					GameObject.FindGameObjectWithTag("myPlayer").transform.position = collision.transform.parent.parent.FindChild("tpLocation1").transform.position;
					once = true;
				}

				else if(collision.transform.parent.name == "tpLocation1")
				{
					GameObject.FindGameObjectWithTag("myPlayer").transform.position = collision.transform.parent.parent.FindChild("tpLocation").transform.position;
					once = true;
				}
				print (collision.gameObject.name);

			}
		}
	}












}
