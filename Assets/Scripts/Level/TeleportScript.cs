using UnityEngine;
using System.Collections;

public class TeleportScript : MonoBehaviour {

	private bool once = false;
	private const float waitTime = 2f;
	private float lastTpTime;
	// Use this for initialization
	void Start ()
	{
		if(networkView.isMine == true)
		{
			lastTpTime = Time.time;
		}

		else
		{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	void OnTriggerEnter(Collider collision)
	{
		if(networkView.isMine == true)
		{
			if (collision.tag == "ParticalCollider")
			{
				if(Time.time - lastTpTime > waitTime)
				{
					if(collision.transform.parent.name == "tpLocation")
					{
						once = true;
						Vector3 tpLocationPos = collision.transform.parent.parent.FindChild("tpLocation1").transform.position;
						Quaternion tpLocationRotation = collision.transform.parent.parent.FindChild("tpLocation").transform.rotation;
						GameObject.FindGameObjectWithTag("myPlayer").transform.position = new Vector3(tpLocationPos.x, tpLocationPos.y + 1.75f, tpLocationPos.z);
						GameObject.FindGameObjectWithTag("myPlayer").transform.rotation = Quaternion.Euler(tpLocationRotation.x, tpLocationRotation.y, tpLocationRotation.z);
						lastTpTime = Time.time;
						print ("2");
					}

					else if(collision.transform.parent.name == "tpLocation1")
					{
						once = true;
						Vector3 tpLocationPos = collision.transform.parent.parent.FindChild("tpLocation").transform.position;
						Quaternion tpLocationRotation = collision.transform.parent.parent.FindChild("tpLocation").transform.rotation;
						GameObject.FindGameObjectWithTag("myPlayer").transform.position = new Vector3(tpLocationPos.x, tpLocationPos.y + 1.75f, tpLocationPos.z);
						GameObject.FindGameObjectWithTag("myPlayer").transform.rotation = Quaternion.Euler(tpLocationRotation.x, tpLocationRotation.y, tpLocationRotation.z);
						lastTpTime = Time.time;
						print ("1");
					}

				}
			}
		}
	}












}
