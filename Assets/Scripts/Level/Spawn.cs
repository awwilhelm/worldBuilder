using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	private bool justConnectedToServer;
	private Rect joinRect;
	public bool spawned = false;
	
	public GameObject playerPrefab;
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
				GameObject[] objs = GameObject.FindGameObjectsWithTag("SpawnPoint") as GameObject[];
				int randomSpawn = Random.Range(0, 3);
				int index = 0;
				foreach(GameObject spawn in objs)
				{
					SpawnPtScript spawnPtScript = spawn.GetComponent<SpawnPtScript>();
					if(randomSpawn == index)
					{
						if(spawnPtScript.taken == false)
						{
							spawnPtScript.CallChangeTaken(true);

							GameObject player_instance = (GameObject)Network.Instantiate(playerPrefab,
							                                                             new Vector3 (spawn.transform.position.x, spawn.transform.position.y+2, spawn.transform.position.z),
							                                                             Quaternion.identity, 0);
							player_instance.transform.tag = "myPlayer";
						}
					}
					index++;
				}
				//Network.Instantiate(player, new Vector3 (25, 25, 25), Quaternion.identity, 0);	
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
