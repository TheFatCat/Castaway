using UnityEngine;
using System.IO;
using System.Collections;

public class TerrainLoader : MonoBehaviour {

	public int dataWidth = 1;
	public int dataHeight = 1;
	public Transform groundBlock;
	public float gridx = 1.0f;
	public float gridy = 1.0f;
	private byte[,] data;
	
	public TerrainLoader getTerrainLoader(){	//gets this script
		return this;
	}
	
	// Use this for initialization
	void Start ()
		{
				//save a list of numbers into file.dat
				byte[] a = new byte[] { 5, 5, 0, 0, 0, 0, 1,   0, 0, 1, 0, 1,   0, 1, 1, 1, 1,   0, 1, 1, 1, 1,   0, 0, 0, 1, 1,  1,0,1,1,1,  0,0,0,1,1};
				using (BinaryWriter b = new BinaryWriter(File.Open("terrain/file.dat", FileMode.Create))) {
		
						foreach (byte i in a) {
								b.Write (i);
						}
				}
				//read the save file
				using (BinaryReader b = new BinaryReader(File.Open("terrain/file.dat", FileMode.Open))) {
						//position in the stream
						int pos = 2;
						int length = (int)b.BaseStream.Length;	//length of the stream
						//read the first two numbers
						dataWidth = (int)b.ReadByte ();
					
						dataHeight = (int)b.ReadByte ();
						//initialize the data variable
						data = new byte[dataWidth, dataHeight];
						//iterate through array
						for (int i=0; i<dataWidth; i++) {
								for (int j=0; j<dataHeight; j++) {
									//read the next byte
									//write it into the array	
									byte bit = (byte)b.ReadByte();
									data[i,j] = bit;
									//and for now, instantiate a block if bit is 1
									if(bit==1){
										Instantiate(groundBlock,transform.position + new Vector3((float)i * gridx,(float)j * -gridy,0.0f), Quaternion.identity);
									}
									
								}
						}
						
					
				}
		
	}
	
	public void write(int x, int y){
		
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
