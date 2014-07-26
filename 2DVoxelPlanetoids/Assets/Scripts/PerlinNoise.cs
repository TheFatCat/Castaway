using UnityEngine;
using System.Collections;

public class PerlinNoise : MonoBehaviour {
	/*
	*	A script that generates a 2Dimensional terrain based on n layers of perlin noise.
	*
	*
	*
	*
	*/
	private static Transform perlin;
	public Transform groundBlock;
	public bool generateBlocks = false;
	//private Transform[,] blocks;	//a wasteful array of all the blocks we instantiated
    public int voxWidth;
    public int voxHeight;
    public Vector2 GeneralOffset;	//different for each terrain generator
    private float[] xOffset;
    private float[] yOffset;
    public float gradientSlope = 2.0f;
    public float gradientOffset = 0.0f;	//gradient offset
    public float[] scale = new float[2];	//horizontal perlin multiplier (ground)
    public float[] height = new float[2];	//z direction perlin multiplier (ground)
    public float groundMin = 0.5f;		//value for disecting ground and not.
    public bool caves = true;
    public float caveGradientSlope = 2.0f;
    public float caveGradientOffset = 0.0f;
    public float[] caveScale = new float[2];	//horizontal perlin multiplier (caves)
    public float[] caveHeight = new float[2];	//z direction perlin multiplier (caves)
    public Vector2 caveRange = new Vector2();	//range for cave generation in perlin
    private Texture2D noiseTex;
    private Color[] pix;
    
    public static Transform getPerlin(){	//getter function
    	return perlin;
    }
    
    public void setOffset(float[] x, float[] y){
    	xOffset = x;
    	yOffset = y;
    }
    
    //initialization
    void Start() {
    	//perlin = this.transform;
    	//initialize offset variables
    	xOffset = new float[Mathf.Max(scale.Length,caveScale.Length)];
    	yOffset = new float[Mathf.Max(scale.Length,caveScale.Length)];
    	//randomize offsets
    	for(int i = 0; i< xOffset.Length;i++){
    		//xOffset[i] = Random.Range(0.0f,1000.0f)	+ GeneralOffset.x;
    		xOffset[i] = GeneralOffset.x;
    		yOffset[i] = GeneralOffset.y;
    		//yOffset[i] = Random.Range(0.0f,1000.0f) + GeneralOffset.y;
    	}
    	//new blank array of transforms
    	//blocks = new Transform[voxWidth,voxHeight];
    	//make new blank texture, and array of pixels
        noiseTex = new Texture2D(voxWidth, voxHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        renderer.material.mainTexture = noiseTex;
        //calculate!
        CalcNoise();
    }
    
    void CalcNoise ()
		{
				float y = 0.0f;
				while (y < noiseTex.height) {
						float x = 0.0f;
						while (x < noiseTex.width) {
								//calculate gradient
								float sample = gradientSlope * ((2.0f * y) - noiseTex.height) / noiseTex.height + gradientOffset;
								//--------------------GROUND------------------
								//calculate all levels of perlin noise
								for (int i = 0; i<scale.Length; i++) {
										sample += height [i] * (Mathf.PerlinNoise ((xOffset [i] + x) * scale [i], (yOffset [i] + y) * scale [i]) - 0.5f);
								}
								//clamp value
								sample = Mathf.Clamp (sample, 0, 1);
								//--------------------CAVES--------------------
								float caveSample = caveGradientSlope * (noiseTex.height - y) / noiseTex.height + caveGradientOffset;
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
                
                
								//instantiate the transform
								if (sample >= groundMin) {
										sample = 1.0f;
										if (generateBlocks) {	
												//*RENDER BLOCK*//
												//Transform block = Instantiate (groundBlock, transform.position + new Vector3 (voxWidth * 0.5f - x - 0.5f, voxHeight * 0.5f - y - 0.5f, -0.5f), Quaternion.identity) as Transform;
												//block.GetComponent<TerrainBlock> ().setID ((int)x, (int)y);
												//block.transform.parent = transform;
										}
                }else{
                	sample = 0.0f;
                }
                //end instantiate
                
                //set the texture color
                pix[(int)(y * noiseTex.width + x)] = new Color(0.0f, 0.0f,0.0f, sample);
                
                x++;
            }
            y++;
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
        //renderer.material.mainTexture.mipMapBias = 3;
        //GetComponent<Light>().cookie = noiseTex;
    }
    
    public void write(int x, int y, float val){
    	Debug.Log("wrote " + x + " " + y);
    	pix[(int)(y * noiseTex.width + x)] = new Color(val, val, val, val);
    	//pix[0] = new Color(val, 0.0f, 0.0f);
    	noiseTex.SetPixels(pix);
        noiseTex.Apply();
        //GetComponent<Light>().cookie = noiseTex;

    }
    void Update() {
        if(!generateBlocks){
        	CalcNoise();	//only re-generate if we're not instanitating
        }
    }
}