using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {

	private HealthScript healthScript;
	private basicWeapon basicWeap;

	public Texture fireRateTex;

	// Use this for initialization
	void Start ()
	{
		if(networkView.isMine)
		{
			healthScript = transform.FindChild("Trigger").GetComponent<HealthScript>();
			basicWeap = gameObject.GetComponent<basicWeapon>();
		}

		else
		{
			enabled = false;
		}
	}

	void OnGUI()
	{
		GUI.DrawTexture(new Rect(Screen.width/10, Screen.height - Screen.height/5, basicWeap.nextShotPercent, 10), fireRateTex);
		GUI.DrawTexture(new Rect(Screen.width/10, Screen.height - Screen.height/8, healthScript.GetHealthPercentage(), 20), fireRateTex);
	}

}
