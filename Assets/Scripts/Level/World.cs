using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {
	
	public byte[,,] data;
	private const int worldX=160;//320
	private const int worldY=64;
	private const int worldZ=160;

	public GameObject chunk;
	public GameObject[,,] chunks;
	public int chunkSize=16;

	public bool generated = false;
	private float seed;
	private int chunkID;
	private int randomChunkx;
	private int randomChunkz;

	public GameObject teleportPrefab;
	private GameObject teleportPrefabInstance;
	public GameObject platformPrefab;
//	private int errorCount = 0;

	// Use this for initialization
	void Start () {
		chunkID = 0;
		data = new byte[worldX,worldY,worldZ];
		randomChunkx = Random.Range (0 + chunkSize, worldX - chunkSize);
		randomChunkz = Random.Range (0 + chunkSize, worldZ - chunkSize);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShouldGenerate()
	{
		seed = Random.Range(0.9f, 1.1f);
		networkView.RPC("UpdateSeedForMap", RPCMode.AllBuffered, seed);
		//UpdateMapForAll();
		networkView.RPC("GetData", RPCMode.AllBuffered);
		UpdateMapForAll();
		generated = true;
		Network.Instantiate (platformPrefab, new Vector3 (randomChunkx, 50, randomChunkz), Quaternion.identity, 0);

		GameObject teleportPrefabInstance = (GameObject)Network.Instantiate (teleportPrefab, new Vector3 (randomChunkx, 50, randomChunkz), Quaternion.identity, 0);
		Vector3 tpEndPos = teleportPrefab.transform.Find ("tpEnd").position;
		teleportPrefabInstance.transform.Find ("tpEnd").position = new Vector3 (tpEndPos.x + randomChunkx, tpEndPos.y + 10 + 50, tpEndPos.z + randomChunkz);

	}



	int PerlinNoise(int x,int y, int z, float scale, float height, float power){
		float rValue;
		rValue=Noise.GetNoise (((double)x) / scale, ((double)y)/ scale * seed, ((double)z) / scale);
		rValue*=height;

		if(power!=0){
			rValue=Mathf.Pow( rValue, power);
		}
		
		return (int) rValue;
	}

	[RPC]
	void GetData()
	{
		for (int x=0; x<worldX; x++){
			for (int z=0; z<worldZ; z++){
				int stone=PerlinNoise(x,0,z,10,1,1.2f);
				stone+= PerlinNoise(x,300,z,40,32,1)+10;
				//stone+= PerlinNoise(x,1,z,10,10,1);
				int dirt=PerlinNoise(x,100,z,50,2,0) +1; //Added +1 to make sure minimum grass height is 1
				for (int y=0; y<worldY; y++){
					if(y<=stone){
						data[x,y,z] = 1;
						//networkView.RPC("UpdateDataValue", RPCMode.AllBuffered, x, y, z, 1);
					} else if(y<=dirt+stone){ //Changed this line thanks to a comment
						data[x,y,z] = 2;
						//networkView.RPC("UpdateDataValue", RPCMode.AllBuffered, x, y, z, 2);
					}
				}
			}
		}

		//spawn teleporters






	}
	
	void UpdateMapForAll()
	{
		chunks=new GameObject[Mathf.FloorToInt(worldX/chunkSize),
		                      Mathf.FloorToInt(worldY/chunkSize),
		                      Mathf.FloorToInt(worldZ/chunkSize)];
		
		for (int x=0; x<chunks.GetLength(0); x++){
			for (int y=0; y<chunks.GetLength(1); y++){
				for (int z=0; z<chunks.GetLength(2); z++){
					
					chunks[x,y,z]= Network.Instantiate(chunk,
					                           new Vector3(x*chunkSize,y*chunkSize,z*chunkSize),
					                           new Quaternion(0,0,0,0),0) as GameObject;
					
					Chunk newChunkScript= chunks[x,y,z].GetComponent("Chunk") as Chunk;

					newChunkScript.CallUpdateChunkID(chunkID);
					newChunkScript.worldGO=gameObject;

					newChunkScript.CallUpdateChunkProp(chunkSize, x*chunkSize, y*chunkSize, z*chunkSize);

					chunkID++;

					/*newChunkScript.chunkSize=chunkSize;
					newChunkScript.chunkX=x*chunkSize;
					newChunkScript.chunkY=y*chunkSize;
					newChunkScript.chunkZ=z*chunkSize;*/
					
				}
			}
		}

		//Network.Instantiate();
	}


	public byte Block(int x, int y, int z)
	{
		
		if( x>=worldX || x<0 || y>=worldY || y<0 || z>=worldZ || z<0){
			return (byte) 1;
		}

		return data[x,y,z];
	}

	[RPC]
	void UpdateSeedForMap(float tempSeed)
	{
		seed = tempSeed;
	}








}