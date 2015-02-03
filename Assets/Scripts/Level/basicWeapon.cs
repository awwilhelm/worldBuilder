using UnityEngine;
using System.Collections;

public class basicWeapon : MonoBehaviour {

	private float nextShot;
	private float fireRate;
	public float nextShotPercent;

	private Transform myTransform;
	private Transform myCamera;
	public float range;
	private RaycastHit hit;
	private Health healthScript;

	public Material playerHitColor;
	private NetworkPlayer netPlayer;

	// Use this for initialization
	void Start ()
	{
		if(networkView.isMine)
		{
			range = 3.0f;
			nextShot = Time.time;
			nextShotPercent = 100;
			fireRate = 0.01f; //0.5
			myCamera = transform.FindChild("CameraHead");
			transform.FindChild("Trigger").GetComponent<BoxCollider>().enabled = false;
			//myTransform = transform;
		}

		else
		{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(nextShot-Time.time >= 0)
		{
			nextShotPercent = ((nextShot - Time.time) / fireRate * 100);
		}
		if(nextShotPercent>100 || nextShot-Time.time < 0)
		{
			nextShotPercent = 100;
		}
		else if(nextShotPercent<1)
		{
			nextShotPercent = 1;
		}

		if(Input.GetMouseButtonDown(0) && Time.time > nextShot) 
		{

			if(Physics.Raycast(myCamera.position, myCamera.forward, out hit, range))
			{
				if(hit.transform.tag == "Trigger")
				{
					hit.transform.gameObject.GetComponent<Health>().TakeDamage(10);
					/*string name = hit.transform.name;
					print (name);
					foreach(GameObject player in transform.parent)
					{
						if(player.name == name && player.name != gameObject.name)
						{							
							player.gameObject.GetComponent<Health>().TakeDamage(10);
							print (player.name);
						}
					}*/
				}
			}
			nextShot = Time.time + fireRate;
		}
	}

	Vector3 ChangeRaycastPos(Transform camera)
	{
		Transform temp = camera;
		temp.transform.Translate(Vector3.forward * 0.25f);
		return temp.position;
	}




}
