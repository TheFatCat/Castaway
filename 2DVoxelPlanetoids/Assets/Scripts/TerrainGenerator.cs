using UnityEngine;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

public class TerrainGenerator : MonoBehaviour {
	public bool generate = false;	//only generates if true
	//array holding all block information
	public byte[,] blocks;
	public byte[,] bgBlocks;
	//biome info
	public string biomePath = "terrain/DefaultBiome.txt";
	public string dataPath = "terrain/fg.dat";
	public string bgDataPath = "terrain/bg.dat";
	
	//..
	public int voxWidth;			//total width
   	public int voxHeight;			//total height
    public int chunkWidth = 100;
    public int chunkHeight = 100;
    public int numChunksX = 2;
    public int numChunksY = 3;
    private float[] xOffset;		//holds offsets
    private float[] yOffset;
    private float gradientSlope = 2.0f;	//gradient slope for ground
    private float gradientOffset = 0.0f;	//gradient offset for ground
    private float[] scale = new float[2];	//horizontal perlin multiplier (ground)
    private float[] height = new float[2];	//z direction perlin multiplier (ground)
    private float groundMin = 0.5f;		//value for disecting ground and not.
    //caves
    private bool caves = true;
    private float caveGradientSlope = 2.0f;
    private float caveGradientOffset = 0.0f;
    private float[] caveScale = new float[2];	//horizontal perlin multiplier (caves)
    private float[] caveHeight = new float[2];	//z direction perlin multiplier (caves)
    private Vector2 caveRange = new Vector2();	//range for cave generation in perlin
    //stone
    private bool stone = true;
    private float stoneGradientSlope = 2.0f;
    private float stoneGradientOffset = 0.0f;
    private float[] stoneScale = new float[2];	//horizontal perlin multiplier (caves)
    private float[] stoneHeight = new float[2];	//z direction perlin multiplier (caves)
    private Vector2 stoneRange = new Vector2();	//range for cave generation in perlin
    
	// Use this for initialization
	void Start () {
		if(generate){
			ReadBiomeData();
			//read all the data
					//randomize offsets
					xOffset = new float[Mathf.Max (scale.Length, caveScale.Length)];
					yOffset = new float[Mathf.Max (scale.Length, caveScale.Length)];
					for (int i = 0; i< xOffset.Length; i++) {	//true randomness left out for now....
						xOffset[i] = Random.Range(-100,100);
						yOffset[i] = Random.Range(-100,100);
					}
			//start the generation script
			StartCoroutine(Generate());
		}else{	//not generating, so quickly read the file
		
		}
	
	}
	
	void ReadBiomeData (){
		//read data
		using(StreamReader sr = new StreamReader(biomePath)){
			//name
			string s = sr.ReadLine();
			Debug.Log (s);
			//gradientslope
			gradientSlope = float.Parse(Regex.Match(sr.ReadLine(),@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.IgnorePatternWhitespace).Value);
			gradientOffset = float.Parse(Regex.Match(sr.ReadLine(),@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.IgnorePatternWhitespace).Value);
			scale = new float[int.Parse(Regex.Match(sr.ReadLine(),@"\d+",RegexOptions.IgnorePatternWhitespace).Value)];
			for(int i=0;i<scale.Length;i++){
				scale[i] = float.Parse(Regex.Match(sr.ReadLine(),@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.IgnorePatternWhitespace).Value);
			}
			height = new float[int.Parse(Regex.Match(sr.ReadLine(),@"\d+",RegexOptions.IgnorePatternWhitespace).Value)];
			for(int i=0;i<height.Length;i++){
				height[i] = float.Parse(Regex.Match(sr.ReadLine(),@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.IgnorePatternWhitespace).Value);
			}
			groundMin = float.Parse(Regex.Match(sr.ReadLine(),@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.IgnorePatternWhitespace).Value);
			caves = bool.Parse(Regex.Match(sr.ReadLine(),@"([t][r][u][e]|[f][a][l][s][e])",RegexOptions.IgnorePatternWhitespace).Value);
			caveGradientSlope = float.Parse(Regex.Match(sr.ReadLine(),@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.IgnorePatternWhitespace).Value);
			caveGradientOffset = float.Parse(Regex.Match(sr.ReadLine(),@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.IgnorePatternWhitespace).Value);
			caveScale = new float[int.Parse(Regex.Match(sr.ReadLine(),@"\d+",RegexOptions.IgnorePatternWhitespace).Value)];
			for(int i=0;i<caveScale.Length;i++){
				caveScale[i] = float.Parse(Regex.Match(sr.ReadLine(),@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.IgnorePatternWhitespace).Value);
			}
			caveHeight = new float[int.Parse(Regex.Match(sr.ReadLine(),@"\d+",RegexOptions.IgnorePatternWhitespace).Value)];
			for(int i=0;i<caveHeight.Length;i++){
				caveHeight[i] = float.Parse(Regex.Match(sr.ReadLine(),@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.IgnorePatternWhitespace).Value);
			}
			caveRange.x = float.Parse(Regex.Match(sr.ReadLine(),@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.IgnorePatternWhitespace).Value);
			caveRange.y = float.Parse(Regex.Match(sr.ReadLine(),@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.IgnorePatternWhitespace).Value);
			stone = bool.Parse(Regex.Match(sr.ReadLine(),@"([t][r][u][e]|[f][a][l][s][e])",RegexOptions.IgnorePatternWhitespace).Value);
			stoneGradientSlope = float.Parse(Regex.Match(sr.ReadLine(),@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.IgnorePatternWhitespace).Value);
			stoneGradientOffset = float.Parse(Regex.Match(sr.ReadLine(),@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.IgnorePatternWhitespace).Value);
			stoneScale = new float[int.Parse(Regex.Match(sr.ReadLine(),@"\d+",RegexOptions.IgnorePatternWhitespace).Value)];
			for(int i=0;i<stoneScale.Length;i++){
				stoneScale[i] = float.Parse(Regex.Match(sr.ReadLine(),@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.IgnorePatternWhitespace).Value);
			}
			stoneHeight = new float[int.Parse(Regex.Match(sr.ReadLine(),@"\d+",RegexOptions.IgnorePatternWhitespace).Value)];
			for(int i=0;i<stoneHeight.Length;i++){
				stoneHeight[i] = float.Parse(Regex.Match(sr.ReadLine(),@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.IgnorePatternWhitespace).Value);
			}
			stoneRange.x = float.Parse(Regex.Match(sr.ReadLine(),@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.IgnorePatternWhitespace).Value);
			stoneRange.y = float.Parse(Regex.Match(sr.ReadLine(),@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.IgnorePatternWhitespace).Value);
			
		}
	}
	
	void ReadSave(){
		//read the save file  for FOREGROUND data
		using (BinaryReader b = new BinaryReader(File.Open("terrain/fg.dat", FileMode.Open))) {
			//i = x
			//j = y
			for(int i = 0; i < numChunksX; i++){
				for(int j = 0; j < numChunksY; j++){
					//iterate through each chunk now
					//l = y
					//m = x
					for(int l = 0; l < chunkHeight; l++){
						for(int m = 0; m < chunkHeight; m++){
							blocks[i * chunkWidth + m,j * chunkHeight + l] = b.ReadByte();
							
						}
					}
				}
			}
		}
		//read the save file for BACKGROUND data
		using (BinaryReader b = new BinaryReader(File.Open("terrain/bg.dat", FileMode.Open))) {
			//i = x
			//j = y
			for(int i = 0; i < numChunksX; i++){
				for(int j = 0; j < numChunksY; j++){
					//iterate through each chunk now
					//l = y
					//m = x
					for(int l = 0; l < chunkHeight; l++){
						for(int m = 0; m < chunkHeight; m++){
							bgBlocks[i * chunkWidth + m,j * chunkHeight + l] = b.ReadByte();
							
						}
					}
				}
			}
		}
	
	}
	
	
	IEnumerator Generate(){
		Debug.Log("STARTED GENERATING DATA BITCH");
	    		//initialize blocks array
	    		blocks = new byte[voxWidth,voxHeight];
	    		bgBlocks = new byte[voxWidth,voxHeight];
	    		//iterate through array
				float y = 0.0f;
				while (y < voxHeight) {
						float x = 0.0f;
						while (x < voxWidth) {
								//calculate gradient
								//(ab - mx form, where a is planet thickness in tiles)
								float sample = gradientSlope * (3.0f * voxHeight - y) / voxHeight + gradientOffset;
								//--------------------GROUND------------------
								//calculate all levels of perlin noise
								for (int i = 0; i<scale.Length; i++) {
										sample += height [i] * (Mathf.PerlinNoise ((xOffset [i] + x) * scale [i], (yOffset [i] + y) * scale [i]) - 0.5f);
								}
								//clamp value
								sample = Mathf.Clamp (sample, 0, 1);
								//write in background array!!
								if (sample >= groundMin) {
									bgBlocks[(int)x,(int)y] = 1;			
                				}else{
                					bgBlocks[(int)x,(int)y] = 0;
                				}
								//--------------------CAVES--------------------
								float caveSample = caveGradientSlope * (3.0f * voxHeight - y) / voxHeight + caveGradientOffset;
								//float caveSample = 0.0f;
								//calculate all levels of perlin noise
								for (int i = 0; i<caveScale.Length; i++) {
										caveSample += caveHeight [i] * (Mathf.PerlinNoise ((xOffset [i] + x) * caveScale [i], (yOffset [i] + y) * caveScale [i]));
								}
                
								if (caveSample > caveRange.x && caveSample < caveRange.y) {//within range!
										caveSample = 1.0f;
								} else {
										caveSample = 0.0f;
								}
								if (caves) {
										sample -= caveSample;	//add caves!
								}
								//write in array!!
								if (sample >= groundMin) {
									//sample = 1.0f;
									blocks[(int)x,(int)y] = 1;		
                				}else{
                					//sample = 0.0f;
                					blocks[(int)x,(int)y] = 0;
                				}
                				
                				//------- done with basic ground generation. Now apply other filters!

                				//--------------------STONE--------------------
								float stoneSample = stoneGradientSlope * (3.0f * voxHeight - y) / voxHeight + stoneGradientOffset;
								//float stoneSample = 0.0f;
								//calculate all levels of perlin noise
								for (int i = 0; i<stoneScale.Length; i++) {
										stoneSample += stoneHeight [i] * (Mathf.PerlinNoise ((xOffset [i] + x) * stoneScale [i], (yOffset [i] + y) * stoneScale [i]));
								}
                
								if (stoneSample > stoneRange.x && stoneSample < stoneRange.y && stone) {//within range!
									if(blocks[(int)x,(int)y] != 0){
										blocks[(int)x,(int)y] = 4;
										bgBlocks[(int)x,(int)y] = 4;
									}
										
								}
								 
                				
               			x++;
            			}		
            y++;
            //yield return null;
        }
        yield return null;
		//do stuff when finished here.....
		//apply filters-----------------------------
		
		//modifies terrain to include grass
	    for(int i = 0;i<voxWidth;i++){	//x
	    	for(int j=0;j<voxHeight; j++){	//y
	    		//check block above (later will add side checks, etc)
	    		if(BgBlock(i,j+1) == 0 && blocks[i,j] == 1){
	    			//we have air above, and are dirt!
	    			blocks[i,j] = 2;	//set to grass
	    		}
	    	}
	    }
	    
    
		//write the data
		using (BinaryWriter b = new BinaryWriter(File.Open(dataPath, FileMode.Create))) {
			//i = x
			//j = y
			for(int j = 0; j < numChunksY; j++){
				for(int i = 0; i < numChunksX; i++){
					//iterate through each chunk now
					//l = y
					//m = x
					for(int l = 0; l < chunkHeight; l++){
						for(int m = 0; m < chunkHeight; m++){
							b.Write(blocks[i * chunkWidth + m,j * chunkHeight + l]);
							
						}
					}
				}
			}
		}
		//write the BACKGROUND
		using (BinaryWriter b = new BinaryWriter(File.Open(bgDataPath, FileMode.Create))) {
			//i = x
			//j = y
			for(int i = 0; i < numChunksX; i++){
				for(int j = 0; j < numChunksY; j++){
					//iterate through each chunk now
					//l = y
					//m = x
					for(int l = 0; l < chunkHeight; l++){
						for(int m = 0; m < chunkHeight; m++){
							b.Write(bgBlocks[i * chunkWidth + m,j * chunkHeight + l]);
							
						}
					}
				}
			}
		}
		Debug.Log ("FINISHED, BITCH");
	}
	
	byte Block (int x, int y)	//returns block at location
		{
				//int xx = x;
				//int yy = y;
				if (x <= -1) {
					//too far left
					x = 0;
				}else if(x >= voxWidth) {
					//too far right
					x = voxWidth - 1;
				}
				if(y <= -1){
					//too low
					y = 0;
				}else if(y >= voxHeight){
					//too high
					y = voxHeight - 1;
				}
				return blocks [x, y];
	}
	
	
	byte BgBlock (int x, int y)	//returns block at location
		{
				//int xx = x;
				//int yy = y;
				if (x <= -1) {
					//too far left
					x = 0;
				}else if(x >= voxWidth) {
					//too far right
					x = voxWidth - 1;
				}
				if(y <= -1){
					//too low
					y = 0;
				}else if(y >= voxHeight){
					//too high
					y = voxHeight - 1;
				
				}
				return bgBlocks [x, y];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
