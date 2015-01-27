using UnityEngine;
using System.Collections;

public class TeleportScript : MonoBehaviour {

	private const float waitTime = 1f;
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
					if(collision.transform.parent.name == "tpStart")
					{
						Vector3 tpLocationPos = collision.transform.parent.parent.FindChild("tpEnd").transform.position;
						Vector3 tpLocationRotation = collision.transform.parent.parent.FindChild("tpEnd").transform.eulerAngles;
						GameObject.FindGameObjectWithTag("myPlayer").transform.position = new Vector3(tpLocationPos.x, tpLocationPos.y + 1.75f, tpLocationPos.z);
						GameObject.FindGameObjectWithTag("myPlayer").transform.rotation = Quaternion.Euler(tpLocationRotation.x, tpLocationRotation.y, tpLocationRotation.z);
						lastTpTime = Time.time;
					}

					else if(collision.transform.parent.name == "tpEnd")
					{
						Vector3 tpLocationPos = collision.transform.parent.parent.FindChild("tpStart").transform.position;
						Vector3 tpLocationRotation = collision.transform.parent.parent.FindChild("tpStart").transform.eulerAngles;
						GameObject.FindGameObjectWithTag("myPlayer").transform.position = new Vector3(tpLocationPos.x, tpLocationPos.y + 1.75f, tpLocationPos.z);
						GameObject.FindGameObjectWithTag("myPlayer").transform.rotation = Quaternion.Euler(tpLocationRotation.x, tpLocationRotation.y, tpLocationRotation.z);
						lastTpTime = Time.time;
					}

				}
			}
		}
	}












}
