using UnityEngine;
using System.Threading;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class PerlinVoxelMesh : MonoBehaviour {
	
	public int ChunkIDX = 0;	//for keeping track of chunk position
	public int ChunkIDY = 0;	//for keeping track of chunk position
	
	private bool finished = false;
	private bool update = false;
	//wont be nessessary...
	public bool foreground = true;
	private List<Vector3> newVertices = new List<Vector3>();	//mesh vertices
	private List<Vector4> tangents = new List<Vector4>();		//tangent normals
	//private List<Vector3> newVertices2 = new List<Vector3>();	//mesh vertices for BG
  	private List<int> newTriangles = new List<int>();			//mesh triangles
  	//private List<int> newTriangles2 = new List<int>();			//mesh triangles for BG
 	private List<Vector3> colVertices = new List<Vector3>();	//collider mesh vertices
	private List<int> colTriangles = new List<int>();			//collider mesh triangles
	private int squareCount;
	private int colCount;
	private List<Vector2> newUV = new List<Vector2>();			//list of UV coordinates for mesh
	private List<Vector2> newUV2 = new List<Vector2>();			//list of secondary uv coordinates
	private Mesh mesh;
	//private Mesh BgMesh;	//mesh of background
	private MeshCollider col;
	private float tUnit = 0.25f; //one tile width/height
	private Vector2 tDirt = new Vector2 (1, 0);
	private Vector2 tGrass = new Vector2 (0, 0);
	private Vector3 tStone = new Vector2 (2,0);
	private Vector3 tSand = new Vector2 (3,0);
	//falling sand particle
	public Transform sandParticle;
	//array holding all block information
	public byte[,] blocks;
	public byte[,] bgBlocks;
	//..
	public int voxWidth;			//this chunk's voxel width
    public int voxHeight;			//this chunk's voxel height
    public Vector2 GeneralOffset;	//different for each terrain generator
    private float[] xOffset;		//holds offsets
    private float[] yOffset;
   
   	public float[] scale = new float[2];	//horizontal perlin multiplier (ground)
   	public float[] caveScale = new float[2];	//horizontal perlin multiplier (caves)
   	/*
    public float gradientSlope = 2.0f;	//gradient slope for ground
    public float gradientOffset = 0.0f;	//gradient offset for ground
    
    public float[] height = new float[2];	//z direction perlin multiplier (ground)
    public float groundMin = 0.5f;		//value for disecting ground and not.
    //caves
    public bool caves = true;
    public float caveGradientSlope = 2.0f;
    public float caveGradientOffset = 0.0f;
    
    public float[] caveHeight = new float[2];	//z direction perlin multiplier (caves)
    public Vector2 caveRange = new Vector2();	//range for cave generation in perlin
    //stone
    public bool stone = true;
    public float stoneGradientSlope = 2.0f;
    public float stoneGradientOffset = 0.0f;
    public float[] stoneScale = new float[2];	//horizontal perlin multiplier (caves)
    public float[] stoneHeight = new float[2];	//z direction perlin multiplier (caves)
    public Vector2 stoneRange = new Vector2();	//range for cave generation in perlin
    */
	//image stuff
	public Texture2D bumpMap;
	private Color[] bumpix;
	private Texture2D noiseTex;
    private Color[] pix;
    //public Renderer shadowPlane;
    PerlinVoxelMesh bgPVM; // the background mesh (if any)
    
    public void setOffset(int x, int y){
    	//x conditions
    	if(x<0){
    		//negative, spawn chunk 1 world size left
    		GeneralOffset.x = x * voxWidth;
    		ChunkIDX = x+10;
    	}else if(x>9){
    		//negative, spawn chunk 1 world size left
    		GeneralOffset.x = x * voxWidth;
    		ChunkIDX = x-10;
    	}else{
    		//negative, spawn chunk 1 world size left
    		GeneralOffset.x = x * voxWidth;
    		ChunkIDX = x;
    	}
    	//y conditions
    	if(y<0){
    		Destroy (gameObject);
    	}else if(y>2){
    		Destroy (gameObject);
    	}
    	ChunkIDY = y;
    	GeneralOffset.y = y * voxHeight;

    }
    
    
	void Start ()
		{
				//if we are a foreground mesh, create a background mesh
				if (foreground) {
						bgPVM = Instantiate (this, transform.position + Vector3.forward * 2, Quaternion.identity) as PerlinVoxelMesh;
						bgPVM.foreground = false;
						bgPVM.transform.parent = transform;
				}
				mesh = GetComponent<MeshFilter> ().mesh;
				col = GetComponent<MeshCollider> ();
   				if(!foreground){	//darken if in background
   					renderer.material.SetColor("_EmisColor",new Color(0.7f,0.7f,0.7f,0.7f));
   				}
				xOffset = new float[Mathf.Max (scale.Length, caveScale.Length)];
				yOffset = new float[Mathf.Max (scale.Length, caveScale.Length)];
				//randomize offsets
				for (int i = 0; i< xOffset.Length; i++) {	//true randomness left out for now....
						xOffset[i] = Random.Range(-100,100)	+ GeneralOffset.x;
						//xOffset [i] = GeneralOffset.x;
						//yOffset [i] = GeneralOffset.y;
						yOffset[i] = Random.Range(-100,100) + GeneralOffset.y;
				}
				//image stuff
				//make new blank texture, and array of pixels
        		noiseTex = new Texture2D(voxWidth, voxHeight);
        		noiseTex.wrapMode = TextureWrapMode.Clamp;
        		bumpMap = new Texture2D(voxWidth, voxHeight);
        		bumpMap.wrapMode = TextureWrapMode.Clamp;
        		pix = new Color[noiseTex.width * noiseTex.height];
        		bumpix = new Color[noiseTex.width * noiseTex.height];
				
				//calculate!
				ReadTerrain();
				
				//yield return null;
				
				//CalcNoise ();
   				//GrassPass();
   				//SimpleTerrain();
   				//Debug.Log("Going to build mesh");
				
				//StartCoroutine(BuildMesh ());
				
				//Debug.Log ("after callin method...");
				
				
	}
	
	public void SimpleTerrain (){
		voxWidth = 5;
		voxHeight = 5;
		blocks = new byte[,]{{1,1,1,1,2},{1,0,0,2,3},{1,1,0,3,3},{1,0,0,2,3},{1,1,1,1,2}};
	}
	
	public void ReadTerrain (){
		//if(foreground){
			//read the save file  for FOREGROUND data
			using (BinaryReader b = new BinaryReader(File.Open("terrain/fg.dat", FileMode.Open))) {
				//q = y
				//r = x
				//iterate through 
				blocks = new byte[voxWidth,voxHeight];
				/*	since the data structure runs x->y->X->Y,
					position = (numChunksX * ChunkIDY + ChunkIDX) * ChunkArea
				*/
				int pos = (10 * ChunkIDY + ChunkIDX) * (voxWidth * voxHeight);
				Debug.Log("Generated mesh at position in file: " + pos);
				b.BaseStream.Seek(pos,SeekOrigin.Begin);
				
				for(int j = 0; j < voxHeight;j++){
					for(int i = 0; i < voxWidth; i++){
						blocks[i,j] = b.ReadByte();
					}
					
				}
				
			}
			
			//read the save file for BACKGROUND data
			using (BinaryReader b = new BinaryReader(File.Open("terrain/bg.dat", FileMode.Open))) {
				//q = y
				//r = x
				//iterate through 
				bgBlocks = new byte[voxWidth,voxHeight];
				int pos = (3 * ChunkIDX + ChunkIDY) * (voxWidth * voxHeight);
				Debug.Log(pos);
				b.BaseStream.Seek(pos,SeekOrigin.Begin);
				
				for(int j = 0; j < voxHeight;j++){
					for(int i = 0; i < voxWidth; i++){
						bgBlocks[i,j] = b.ReadByte();
					}
				}
				
			}
			
		StartCoroutine(BuildMesh ());
	}
	/*
	void CalcNoise (){
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
								float sample = gradientSlope * (3.0f * voxHeight - y - GeneralOffset.y) / voxHeight + gradientOffset;
								//float sample = gradientSlope * (3.0f * voxHeight - y) / voxHeight + gradientOffset;
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
								float caveSample = caveGradientSlope * (3.0f * voxHeight - y - GeneralOffset.y) / voxHeight + caveGradientOffset;
								//float caveSample = caveGradientSlope * (3.0f * voxHeight - y) / voxHeight + caveGradientOffset;
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
								float stoneSample = stoneGradientSlope * (3.0f * voxHeight - y  - GeneralOffset.y) / voxHeight + stoneGradientOffset;
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
        finished = true;
    }
    
    void GrassPass(){	//modifies terrain to include grass
    	for(int i = 0;i<voxWidth;i++){	//x
    		for(int j=0;j<voxHeight; j++){	//y
    			//check block above (later will add side checks, etc)
    			if(Block(i,j+1) == 0 && blocks[i,j] == 1){
    				//we have air above, and are dirt!
    				blocks[i,j] = 2;	//set to grass
    			}
    			
    		}
    	}
    }
    */
    
    
	
	IEnumerator BuildMesh ()
		{	//takes byte array and generates mesh and collider
				//started building
				finished = false;
				for (int px=0; px<blocks.GetLength(0); px++) {
						for (int py=0; py<blocks.GetLength(1); py++) {
								//if background block is not air
								if (bgBlocks [px, py] != 0) {
										//image stuff
										//set the texture color
										if (isNearEmpty (px - 6, py - 5, 13)) {
												if (isNearEmpty (px - 1, py - 1, 3)) {// bg is sufficiently close to edge
														pix [(int)(py * noiseTex.width + px)] = new Color (0.0f, 0.0f, 0.0f, 1.0f);
												} else {
														pix [(int)(py * noiseTex.width + px)] = new Color (0.0f, 0.0f, 0.0f, 0.5f);
												}
												
												//check foreground
												if (foreground) {
												
														if (isNearEmptyFg (px - 2, py - 2, 5)) {//but foreground isn't!!!!
																if (!isNearEmptyFg (px - 1, py - 1, 3)) {
																		pix [(int)(py * noiseTex.width + px)] = new Color (0.0f, 0.0f, 0.0f, 0.5f);
																}
														} else {
																pix [(int)(py * noiseTex.width + px)] = new Color (0.0f, 0.0f, 0.0f, 0.0f);
														}
														
												} else {	//if we are bg, and there is a foreground on top of us, set pix to 0 for shadow effect
														if (blocks [px, py] != 0) {
													
																pix [(int)(py * noiseTex.width + px)] = new Color (0.0f, 0.0f, 0.0f, 0.0f);
													
														}
											
												}//done checking fg
												
										} else {
												pix [(int)(py * noiseTex.width + px)] = new Color (0.0f, 0.0f, 0.0f, 0.0f);
										}
								} else {
										if (foreground) {
												if (isNearEmptyFg (px - 2, py - 2, 5)) {//but foreground isn't!!!!
														if (!isNearEmptyFg (px - 1, py - 1, 3)) {
																pix [(int)(py * noiseTex.width + px)] = new Color (0.0f, 0.0f, 0.0f, 0.5f);
														} else {
																pix [(int)(py * noiseTex.width + px)] = new Color (0.0f, 0.0f, 0.0f, 1.0f);
														}
												} else {
														pix [(int)(py * noiseTex.width + px)] = new Color (0.0f, 0.0f, 0.0f, 0.0f);
												}
										} else {
												pix [(int)(py * noiseTex.width + px)] = new Color (0.0f, 0.0f, 0.0f, 1.0f);
										}
								}
								//Actually build the mesh
								if(foreground){
									//If the block is not air
									if (blocks [px, py] != 0) {
											bumpix [(int)(py * noiseTex.width + px)] = new Color (0, 0f, 0f, 0.5f);
											// GenCollider here, this will apply it
											// to every block other than air
											GenCollider (px, py);
	
											switch (blocks [px, py]) {
											case 1:
													GenSquare ((int)GeneralOffset.x + px, (int)GeneralOffset.y + py, tDirt);
													break;
											case 2:
													GenSquare ((int)GeneralOffset.x + px, (int)GeneralOffset.y + py, tGrass);
													break;
											case 3:
													GenSquare ((int)GeneralOffset.x + px, (int)GeneralOffset.y + py, tSand);
													break;
											case 4:
													GenSquare ((int)GeneralOffset.x + px, (int)GeneralOffset.y + py, tStone);
													break;
											}
	     									
	     									
											
									} else {//End air block check
										bumpix [(int)(py * noiseTex.width + px)] = new Color (0, 0f, 0f, 0.0f);
									}
								}else{	//we are not foreground, so use bgblocks
									//If the block is not air
									if (bgBlocks [px, py] != 0) {
											bumpix [(int)(py * noiseTex.width + px)] = new Color (0, 0f, 0f, 0.5f);
											// GenCollider here, this will apply it
											// to every block other than air
											GenCollider (px, py);
	
											switch (bgBlocks [px, py]) {
											case 1:
													GenSquare ((int)GeneralOffset.x + px, (int)GeneralOffset.y + py, tDirt);
													break;
											case 2:
													GenSquare ((int)GeneralOffset.x + px, (int)GeneralOffset.y + py, tGrass);
													break;
											case 3:
													GenSquare ((int)GeneralOffset.x + px, (int)GeneralOffset.y + py, tSand);
													break;
											case 4:
													GenSquare ((int)GeneralOffset.x + px, (int)GeneralOffset.y + py, tStone);
													break;
											}
	     									
	     									
											
									} else {//End air block check
										bumpix [(int)(py * noiseTex.width + px)] = new Color (0, 0f, 0f, 0.0f);
									}
								}
								
									
								
						}
						if ((px % 15) == 0) {
								//let other stuff do things every 30 columns
								yield return null;
						}
				}
				
				StartCoroutine(blur());
				
				UpdateMesh();
				finished = true;
	}
	
	public IEnumerator blur(){
		//image stuff
				noiseTex.SetPixels(pix);
				
				
        		//noiseTex.Apply();
				//Color[] pix2 = new Color[voxWidth * voxHeight];
				for(int i = 0; i<voxWidth;i++){
					for(int j = 0; j<voxHeight;j++){
						float sum = 0.0f;
						//int blurSize = 1;
						
						sum +=noiseTex.GetPixel(i + 3,j + 3).a * 0.04f;
						sum +=noiseTex.GetPixel(i - 1,j + 1).a * 0.10f;
						sum +=noiseTex.GetPixel(i - 3,j + 3).a * 0.04f;
						sum +=noiseTex.GetPixel(i + 1,j + 1).a * 0.10f;
						sum +=noiseTex.GetPixel(i,j).a * 0.24f;
						sum +=noiseTex.GetPixel(i - 2,j - 2).a * 0.04f;
						sum +=noiseTex.GetPixel(i - 1,j - 1).a * 0.10f;
						sum +=noiseTex.GetPixel(i + 2,j - 2).a * 0.04f;
						sum +=noiseTex.GetPixel(i + 1,j - 1).a * 0.10f;
						
						//sum += pix2[(j + 5) * noiseTex.width + (i)] * 0.025;
						pix[j * noiseTex.width + i] = new Color(0.0f,0.0f,0.0f,sum); 
					}
					if(i%100 == 0){
						yield return null;
					}
				}
				
				//yield return null;
				
				noiseTex.SetPixels(pix);
        		noiseTex.Apply();
        		bumpMap.SetPixels(bumpix);
				bumpMap.Apply();
        		renderer.material.SetTexture("_Illum",noiseTex);
        		bumpMap = NormalMap(bumpMap,1f);
        		bumpMap.Apply();
        		renderer.material.SetTexture("_BumpMap",bumpMap);
        		//shadowPlane.material.mainTexture = noiseTex;
	}

	private Texture2D NormalMap(Texture2D source,float strength) {
	         strength=Mathf.Clamp(strength,0.0F,10.0F);
	         Texture2D result;
	         float xLeft;
	         float xRight;
	         float yUp;
	         float yDown;
	         float yDelta;
	         float xDelta;
	         result = new Texture2D (source.width, source.height, TextureFormat.ARGB32, true);
	         for (int by=0; by<result.height; by++) {
	                    for (int bx=0; bx<result.width; bx++) {
	                    			//closest
	                                xLeft = (1f -source.GetPixel(bx-1,by).a)*strength * 0.5f;
	                                xRight = (1f -source.GetPixel(bx+1,by).a)*strength * 0.5f;
	                                yUp = (1f -source.GetPixel(bx,by-1).a)*strength * 0.5f;
	                                yDown = (1f -source.GetPixel(bx,by+1).a)*strength * 0.5f;
	                                //1 further
	                                xLeft += (1f -source.GetPixel(bx-2,by).a)*strength * 0.3f;
	                                xRight += (1f -source.GetPixel(bx+2,by).a)*strength * 0.3f;
	                                yUp += (1f -source.GetPixel(bx,by-2).a)*strength * 0.3f;
	                                yDown += (1f -source.GetPixel(bx,by+2).a)*strength * 0.3f;
	                                //2 further
	                                xLeft += (1f -source.GetPixel(bx-3,by).a)*strength * 0.2f;
	                                xRight += (1f -source.GetPixel(bx+3,by).a)*strength * 0.2f;
	                                yUp += (1f -source.GetPixel(bx,by-3).a)*strength * 0.2f;
	                                yDown += (1f -source.GetPixel(bx,by+3).a)*strength * 0.2f;
	                                
	                                xDelta = ((xLeft-xRight)+1)*0.5f;
	                                yDelta = ((yUp-yDown)+1)*0.5f;
	                                //result.SetPixel(bx,by,new Color(xDelta,yDelta,1.0f,yDelta));
	                                result.SetPixel(bx,by,new Color(xDelta,yDelta,1.0f,xDelta));
	                    }
	         }
	         result.Apply();
	         return result;
	}
	
	public bool isNearEmpty(int x, int y, int radius){	//iterate through all the blocks in radius until empty one is found
		for(int i=0;i < radius;i++){
			for(int j=0;j < radius+1;j++){
				if(BgBlock(x + i,y + j) == 0){
					//found an empty block!
					return true;
				}
			}
		}
		//didn't find anything
		return false;
	}
	
	public bool isNearEmptyFg(int x, int y, int radius){	//iterate through all the blocks in radius until empty one is found
		for(int i=0;i < radius;i++){
			for(int j=0;j < radius+1;j++){
				if(Block(x + i,y + j) == 0){
					//found an empty block!
					return true;
				}
			}
		}
		//didn't find anything
		return false;
	}
			
	public Vector2 CalcLightingOffset (int x, int y)
		{
		
				if (Block (x, y + 1) == 0) {	//up
						if (Block (x + 1, y) == 0) {	//right
								if (Block (x, y - 1) == 0) {	//down
										if (Block (x - 1, y) == 0) {	//left
											//URDL
											return new Vector2(0,6);
										} else {	//not left
											//URD
											return new Vector2(3,7);
										}
								} else if (Block (x - 1, y) == 0) {	//!down left
									//URL
									return new Vector2(2,7);
								} else {	//not left
									//UR
									return new Vector2(3,6);
								}
						} else if (Block (x, y - 1) == 0) {	//!Right, down
								if (Block (x - 1, y) == 0) {	//left
									//UDL
									return new Vector2(1,7);
								} else {	//not left
									//UD
									return new Vector2(0,4);
								}
						} else if (Block (x - 1, y) == 0) {	//left
								//UL
								return new Vector2(1,6);
							} else {	//not left
							//U	
							return new Vector2(2,6);	
						}
				} else if (Block (x + 1, y) == 0) {	//not up,   right
						if (Block (x, y - 1) == 0) {	//down
							if (Block (x - 1, y) == 0) {	//left
								//RDL
								return new Vector2(0,7);
							} else { //not left
								//RD
								return new Vector2(3,4);
							}
						} else if (Block (x - 1, y) == 0) {	//left
								//RL
								return new Vector2(0,5);
							} else { //not left
								//R
								return new Vector2(3,5);
							}
				} else if (Block (x, y - 1) == 0) {	//not right, down
						if (Block (x - 1, y) == 0) {	//left
							//DL
							return new Vector2(1,4);
						} else {//not left
							//D
							return new Vector2(2,4);
						}
				} else if (Block (x - 1, y) == 0) {	//left
					//L
					return new Vector2(1,5);
					
					//----------------------------------------Diagonal checks----------------------------------------
				}else if (Block (x - 1, y + 1) == 0) {	//upLeft
						if (Block (x + 1, y + 1) == 0) {	//upright
								if (Block (x + 1, y - 1) == 0) {	//downright
										if (Block (x - 1, y - 1) == 0) {	//downleft
											//URDL
											return new Vector2(7,7);
										} else {	//not left
											//URD
											return new Vector2(6,4);
										}
								} else if (Block (x - 1, y - 1) == 0) {	//!downright downleft?
									//URL
									return new Vector2(6,5);
								} else {	//not left
									//UR
									return new Vector2(5,7);
								}
						} else if (Block (x + 1, y - 1) == 0) {	//!upRight, downright?
								if (Block (x - 1, y - 1) == 0) {	//downleft?
									//UDL
									return new Vector2(6,6);
								} else {	//not left
									//UD
									return new Vector2(7,6);
								}
						} else if (Block (x - 1, y - 1) == 0) {	//downleft
								//UL
								return new Vector2(5,4);
							} else {	//not left
							//U	
							return new Vector2(4,7);	
						}
				} else if (Block (x + 1, y + 1) == 0) {	//not upleft, upright?
						if (Block (x + 1, y - 1) == 0) {	//downright?
							if (Block (x - 1, y - 1) == 0) {	//downleft
								//RDL
								return new Vector2(6,7);
							} else { //not left
								//RD
								return new Vector2(5,6);
							}
						} else if (Block (x - 1, y - 1) == 0) {	//downleft
								//RL
								return new Vector2(7,5);
							} else { //not left
								//R
								return new Vector2(4,6);
							}
				} else if (Block (x + 1, y - 1) == 0) {	//not upright, downright?
						if (Block (x - 1, y - 1) == 0) {	//downleft
							//DL
							return new Vector2(5,5);
						} else {//not left
							//D
							return new Vector2(4,5);
						}
				} else if (Block (x - 1, y - 1) == 0) {	//left
					//L
					return new Vector2(4,4);
					
					//----------------------------------------Tier 2 checks----------------------------------------
				} else if (Block (x, y + 2) == 0) {	//up
						if (Block (x + 2, y) == 0) {	//right
								if (Block (x, y - 2) == 0) {	//down
										if (Block (x - 2, y) == 0) {	//left
											//URDL
											return new Vector2(0,2);
										} else {	//not left
											//URD
											return new Vector2(3,3);
										}
								} else if (Block (x - 2, y) == 0) {	//!down left
									//URL
									return new Vector2(2,3);
								} else {	//not left
									//UR
									return new Vector2(3,2);
								}
						} else if (Block (x, y - 2) == 0) {	//!Right, down
								if (Block (x - 2, y) == 0) {	//left
									//UDL
									return new Vector2(1,3);
								} else {	//not left
									//UD
									return new Vector2(0,0);
								}
						} else if (Block (x - 2, y) == 0) {	//left
								//UL
								return new Vector2(1,2);
							} else {	//not left
							//U	
							return new Vector2(2,2);	
						}
				} else if (Block (x + 2, y) == 0) {	//not up,   right
						if (Block (x, y - 2) == 0) {	//down
							if (Block (x - 2, y) == 0) {	//left
								//RDL
								return new Vector2(0,3);
							} else { //not left
								//RD
								return new Vector2(3,0);
							}
						} else if (Block (x - 2, y) == 0) {	//left
								//RL
								return new Vector2(0,1);
							} else { //not left1
								//R
								return new Vector2(3,1);
							}
				} else if (Block (x, y - 2) == 0) {	//not right, down
						if (Block (x - 2, y) == 0) {	//left
							//DL
							return new Vector2(1,0);
						} else {//not left
							//D
							return new Vector2(2,0);
						}
				} else if (Block (x - 2, y) == 0) {	//left
					//L
					return new Vector2(1,1);
					
					//----------------------------------------Diagonal Tier 2 checks----------------------------------------
				}
				//just black
				return new Vector2 (2,1);
		}
	
	void GenSquare (int x, int y, Vector2 texture)
		{	//adds a square to the mesh, and checks uv placement
   
				newVertices.Add (new Vector3 (x, y, transform.position.z));
				newVertices.Add (new Vector3 (x + 1, y, transform.position.z));
				newVertices.Add (new Vector3 (x + 1, y - 1, transform.position.z));
				newVertices.Add (new Vector3 (x, y - 1, transform.position.z));
   
				newTriangles.Add (squareCount * 4);
				newTriangles.Add ((squareCount * 4) + 1);
				newTriangles.Add ((squareCount * 4) + 3);
				newTriangles.Add ((squareCount * 4) + 1);
				newTriangles.Add ((squareCount * 4) + 2);
				newTriangles.Add ((squareCount * 4) + 3);
				
				tangents.Add(new Vector4(-1f,0,0,-1f));
				tangents.Add(new Vector4(-1f,0,0,-1f));
				tangents.Add(new Vector4(-1f,0,0,-1f));
				tangents.Add(new Vector4(-1f,0,0,-1f));
				
				//before checking uv, figure out which uv to use
				//Vector2 uvOffset = CalcLightingOffset(int)GeneralOffset.x + px,int)GeneralOffset.y + py);
				float uvScale = 1.0f / (float)voxWidth;
				Vector2 uvOffset = new Vector2(x - GeneralOffset.x,y - GeneralOffset.y);
				//set secondary uv's
				
				newUV2.Add (new Vector2 (uvScale * uvOffset.x, uvScale * uvOffset.y + uvScale));
				newUV2.Add (new Vector2 (uvScale * uvOffset.x + uvScale, uvScale * uvOffset.y + uvScale));
				newUV2.Add (new Vector2 (uvScale * uvOffset.x + uvScale, uvScale * uvOffset.y));
				newUV2.Add (new Vector2 (uvScale * uvOffset.x, uvScale * uvOffset.y));
				
   				
				newUV.Add (new Vector2 (tUnit * texture.x, tUnit * texture.y + tUnit));
				newUV.Add (new Vector2 (tUnit * texture.x + tUnit, tUnit * texture.y + tUnit));
				newUV.Add (new Vector2 (tUnit * texture.x + tUnit, tUnit * texture.y));
				newUV.Add (new Vector2 (tUnit * texture.x, tUnit * texture.y));
   
				squareCount++;
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
	
	void ColliderTriangles (){	//adds triangle to collider mesh list
				colTriangles.Add (colCount * 4);
				colTriangles.Add ((colCount * 4) + 1);
				colTriangles.Add ((colCount * 4) + 3);
				colTriangles.Add ((colCount * 4) + 1);
				colTriangles.Add ((colCount * 4) + 2);
				colTriangles.Add ((colCount * 4) + 3);

	}
	
	void GenCollider (int x, int y)	//generates mesh for collider
		{
				if(!foreground){	//early out if we're not foreground
					return;
				}
				//only make a collider if neighbor is air #Efficiency!
				//Top
				if (Block (x, y + 1) == 0) {
						colVertices.Add (new Vector3 ((int)GeneralOffset.x + x,(int)GeneralOffset.y +  y, 1));
						colVertices.Add (new Vector3 ((int)GeneralOffset.x + x + 1,(int)GeneralOffset.y +  y, 1));
						colVertices.Add (new Vector3 ((int)GeneralOffset.x + x + 1,(int)GeneralOffset.y +  y, 0));
						colVertices.Add (new Vector3 ((int)GeneralOffset.x + x,(int)GeneralOffset.y +  y, 0));
  
						ColliderTriangles ();
  
						colCount++;
				}
  				
				//bot
				if (Block (x, y - 1) == 0) {
						colVertices.Add (new Vector3 ((int)GeneralOffset.x + x,(int)GeneralOffset.y +  y - 1, 0));
						colVertices.Add (new Vector3 ((int)GeneralOffset.x + x + 1,(int)GeneralOffset.y +  y - 1, 0));
						colVertices.Add (new Vector3 ((int)GeneralOffset.x + x + 1,(int)GeneralOffset.y +  y - 1, 1));
						colVertices.Add (new Vector3 ((int)GeneralOffset.x + x,(int)GeneralOffset.y +  y - 1, 1));
  
						ColliderTriangles ();
						colCount++;
				}
  
				//left
				if (Block (x - 1, y) == 0) {
						colVertices.Add (new Vector3 ((int)GeneralOffset.x + x,(int)GeneralOffset.y +  y - 1, 1));
						colVertices.Add (new Vector3 ((int)GeneralOffset.x + x,(int)GeneralOffset.y +  y, 1));
						colVertices.Add (new Vector3 ((int)GeneralOffset.x + x,(int)GeneralOffset.y +  y, 0));
						colVertices.Add (new Vector3 ((int)GeneralOffset.x + x,(int)GeneralOffset.y +  y - 1, 0));
  
						ColliderTriangles ();
  
						colCount++;
				}
  
				//right
				if (Block (x + 1, y) == 0) {
						colVertices.Add (new Vector3 ((int)GeneralOffset.x + x + 1,(int)GeneralOffset.y +  y, 1));
						colVertices.Add (new Vector3 ((int)GeneralOffset.x + x + 1,(int)GeneralOffset.y +  y - 1, 1));
						colVertices.Add (new Vector3 ((int)GeneralOffset.x + x + 1,(int)GeneralOffset.y +  y - 1, 0));
						colVertices.Add (new Vector3 ((int)GeneralOffset.x + x + 1,(int)GeneralOffset.y +  y, 0));
  
						ColliderTriangles ();
  
						colCount++;
				}
				
  
		}
		
	public bool addBlock (float offsetX, float offsetY, byte b)
		{	//adds block of type b to coordinates (false if not empty)
				int x = Mathf.RoundToInt (offsetX);
				int y = Mathf.RoundToInt (offsetY);
				Debug.Log (x + " " + y);
				if (x < blocks.GetLength (0) + GeneralOffset.x && y < blocks.GetLength (1) + GeneralOffset.y && x >= GeneralOffset.x && y >= GeneralOffset.y) {
  
						if (blocks [x - (int)GeneralOffset.x, y - (int)GeneralOffset.y] == 0) {
								update = true;
								blocks [x - (int)GeneralOffset.x, y - (int)GeneralOffset.y] = b;
								//now set background mesh
								if (foreground) {
										bgPVM.addBlock (offsetX, offsetY, b);
								}
								return true;
						}
				}
				return false;
	
	}
	
	
	
	public bool RemoveBlock (float offsetX, float offsetY)		//removes a block at coordinates (false if already empty)
		{
				int x = Mathf.RoundToInt (offsetX);
				int y = Mathf.RoundToInt (offsetY);
  
				if (x < blocks.GetLength (0) + GeneralOffset.x && y < blocks.GetLength (1) + GeneralOffset.y && x >= GeneralOffset.x && y >= GeneralOffset.y) {
  
						if (blocks [x - (int)GeneralOffset.x, y - (int)GeneralOffset.y] != 0) {
								update = true;
								blocks [x - (int)GeneralOffset.x, y - (int)GeneralOffset.y] = 0;
								//now set background mesh
								if (foreground) {
										bgPVM.RemoveBlock (offsetX, offsetY);
								}
								return true;
						}
				}
				return false;
		}
	
	public bool isBlockSandy (float offsetX, float offsetY)	//true if sand, false if not
		{
				int x = Mathf.RoundToInt (offsetX);
				int y = Mathf.RoundToInt (offsetY);
  
				if (x < blocks.GetLength (0) + GeneralOffset.x && y < blocks.GetLength (1) + GeneralOffset.y && x >= GeneralOffset.x && y >= GeneralOffset.y) {
  
						if (blocks [x - (int)GeneralOffset.x, y - (int)GeneralOffset.y] == 3) {
								return true;
						}
				}
				return false;
	
	
		}
		
	public Vector3 checkSandPosition(float xx, float yy, float z){
		Debug.Log ("checked sand position");
		
		//0.5 added / removed since sand blocks are offset by this much when instantiated
		int x = Mathf.RoundToInt(xx - 0.5f) - (int)GeneralOffset.x;
		int y = Mathf.RoundToInt(yy + 0.5f) - (int)GeneralOffset.y;

		if(Block(x,y-1) == 0){	//empty space below us
			return new Vector3(xx,yy- 1.0f,z);	//tell to go down
		}else{
			//randomness 
			bool r = Random.value > 0.5f;
			if(r){	//right then left
				if(Block(x+1,y-1) == 0){//right?
					return new Vector3(xx + 1.0f,yy- 1.0f,z);	//tell to go right
				}else if(Block(x-1,y-1) == 0){//left?
					return new Vector3(xx - 1.0f,yy- 1.0f,z);	//tell to go left
				}
				//still haven't got anything...
			}else{	//left then right
				if(Block(x-1,y-1) == 0){//left?
					return new Vector3(xx - 1.0f,yy- 1.0f,z);	//tell to go left
				}else if(Block(x+1,y-1) == 0){//right?
					return new Vector3(xx + 1.0f,yy- 1.0f,z);	//tell to go right
				}
				//still haven't got anything...
			}
		}
		//if we got to this point, time to stop
		return new Vector3(xx,yy,z);
	}
		
	public IEnumerator RemoveSandBlock (int x, int y)
		{	//only called when a sand block falls (internally only)
				//check if sand block
				if (blocks [x, y] == 3) {	//its a sand block!
						Debug.Log ("removing sandy block");
						blocks [x, y] = 0;	//remove from array
						//instantiate sand particle
						Transform sp = Instantiate (sandParticle, new Vector3 (GeneralOffset.x + x + 0.5f, GeneralOffset.y + y - 0.5f, transform.position.z), Quaternion.identity) as Transform;
						sp.GetComponent<SandBlock> ().SetPVM (this);
						//start rendering mesh
						//StartCoroutine (BuildMesh ());
						update = true;
						//wait a bit
						yield return new WaitForSeconds (Random.Range (0.1f, 0.2f));
				}
						if (Block (x, y + 1) == 3) {
								//block above is sand
								StartCoroutine (RemoveSandBlock (x, y + 1));
								//do the same thing to it!
								//wait a bit
								yield return new WaitForSeconds (Random.Range(0.1f,0.2f));
						} 
						//no block above us
						bool r = Random.value > 0.5f;
						if (r) {	//check right, then left
								if (Block (x + 1, y + 1) == 3) {	//block up-right of us
										StartCoroutine (RemoveSandBlock (x + 1, y + 1));
										//do the same thing to it!
										//wait a bit
										yield return new WaitForSeconds (Random.Range(0.1f,0.2f));
								}
								if (Block (x - 1, y + 1) == 3) {	//block up-left of us!
										StartCoroutine (RemoveSandBlock (x - 1, y + 1));
										//do the same thing to it!
										//wait a bit
										yield return new WaitForSeconds (Random.Range(0.1f,0.2f));
								}
								Debug.Log ("finished method");
						} else {	//check left, then right
								if (Block (x - 1, y + 1) == 3) {	//block up-right of us
										StartCoroutine (RemoveSandBlock (x - 1, y + 1));
										//do the same thing to it!
										//wait a bit
										yield return new WaitForSeconds (Random.Range(0.1f,0.2f));
								}
								if (Block (x + 1, y + 1) == 3) {	//block up-left of us!
										StartCoroutine (RemoveSandBlock (x + 1, y + 1));
										//do the same thing to it!
										//wait a bit
										yield return new WaitForSeconds (Random.Range(0.1f,0.2f));
								}
								Debug.Log ("finished method");
								//start rendering mesh
								//StartCoroutine (BuildMesh ());
						}
				
		}
					
	void UpdateMesh (){	//updates the mesh
				mesh.Clear ();
				mesh.vertices = newVertices.ToArray ();
				mesh.triangles = newTriangles.ToArray ();
				mesh.uv = newUV.ToArray ();
				mesh.uv2 = newUV2.ToArray();
				mesh.Optimize ();
				mesh.RecalculateNormals ();
				mesh.tangents = tangents.ToArray();
		
				if(foreground){//update collider mesh
					Mesh newMesh = new Mesh();
					newMesh.vertices = colVertices.ToArray();
					newMesh.triangles = colTriangles.ToArray();
					newMesh.Optimize();
					col.sharedMesh= newMesh;
	 				
	 				//reset collider lists
					colVertices.Clear();
					colTriangles.Clear();
					colCount=0;
				}
				
				//reset lists and square count
				squareCount=0;
 				newVertices.Clear();
 				newTriangles.Clear();
 				tangents.Clear();
 				newUV.Clear();
 				newUV2.Clear();

	}
	// Update is called once per frame
	void Update (){
		Vector3 v = Camera.main.transform.position + new Vector3(((Input.mousePosition.x/Screen.width) - 0.5f) * 2.0f * Camera.main.orthographicSize * Screen.width/Screen.height,((Input.mousePosition.y/Screen.height) - 0.5f) * 2.0f * Camera.main.orthographicSize,0.0f);
		
		if(Input.GetButton("Fire2") && foreground){
			//Debug.Log ("added block " + v.x + " " +v.y);
			if(addBlock(v.x-0.5f,v.y+0.5f,3)){
				StartCoroutine(RemoveSandBlock(Mathf.RoundToInt(v.x- 0.5f) - (int)GeneralOffset.x ,Mathf.RoundToInt(v.y+0.5f) - (int)GeneralOffset.y));
				//update = true;
			}
		}else if(Input.GetButton("Fire1") && foreground){
			//remove block
			if(RemoveBlock(v.x-0.5f,v.y+0.5f)){
				StartCoroutine(RemoveSandBlock(Mathf.RoundToInt(v.x- 0.5f) - (int)GeneralOffset.x ,Mathf.RoundToInt(v.y+0.5f) - (int)GeneralOffset.y));
				//update = true;
			}	
			
		}
				if (update && finished) {	//finished loading!
					StartCoroutine(BuildMesh ());	//update included in build
					
					update = false;
				}
	}
}
