using UnityEngine;
using System.Collections;

public class TerrainBlock : MonoBehaviour {
	
	//ID in xy coorinates
	private int x = 0;
	private int y = 0;
	public float hp = 1.0f;
	//public Transform drop;
	private bool selected = false;
	//sprites are numbered left to right, in descending order
	public Sprite[] sprites = new Sprite[16];
	//variable to store if there are 
	private bool[] neighborExists = new bool[4];
	
	// Use this for initialization
	void Start () {
		//calculate sprite
		Recalculate();
	}
	
	
	void OnMouseEnter(){
		selected = true;
	}
	
	void OnMouseExit(){
		selected = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(selected){
			if(Input.GetMouseButton(0)){	//clicked
				//should go into destroy() function
				PerlinNoise perlin = PerlinNoise.getPerlin().GetComponent<PerlinNoise>();
				perlin.write (x,y,0.0f);
				Destroy(gameObject);
				
			}
		}
	}
	
	public void setID(int i, int j){
		x = i;
		y = j;
	}
	
	public void Recalculate ()
		{	//recalculates the sprite based on cardinal neighbors
				//check for blocks in 4 cardinal directions around us (up, right, down, left)
				int numNeighbors = 0;	//the number of neighbors we find (max 4)
				//anything above us?
				if (Physics.Raycast (transform.position, Vector3.up, 0.6f)) {
						numNeighbors++;
						neighborExists [0] = true;
				}
				if (Physics.Raycast (transform.position, Vector3.right, 0.6f)) {
						numNeighbors++;
						neighborExists [1] = true;
				}
				if (Physics.Raycast (transform.position, -Vector3.up, 0.6f)) {
						numNeighbors++;
						neighborExists [2] = true;
				}
				if (Physics.Raycast (transform.position, -Vector3.right, 0.6f)) {
						numNeighbors++;
						neighborExists [3] = true;
				}
		
				switch (numNeighbors) {
				case 0:
						//no neighbors
						GetComponent<SpriteRenderer> ().sprite = sprites [4];
						break;
				case 1:
						//1 neighbor
						if (neighborExists [0]) {		//neighbor above
							GetComponent<SpriteRenderer> ().sprite = sprites [3];
						}else if(neighborExists [1]) {	//neighbor to right
							GetComponent<SpriteRenderer> ().sprite = sprites [2];
						}else if(neighborExists [2]) {	//neighbor below
							GetComponent<SpriteRenderer> ().sprite = sprites [1];
						}else if(neighborExists [3]) {	//neighbor to left
							GetComponent<SpriteRenderer> ().sprite = sprites [0];
						}
						break;
				case 2:
						//2 neighbors
						if (neighborExists [0]) {	//neighbor above
								//and-----------------------------------------------
								if (neighborExists [1]) {	//neighbor to right
										GetComponent<SpriteRenderer> ().sprite = sprites [13];
										//13
								} else if (neighborExists [2]) {	//neighbor below
										GetComponent<SpriteRenderer> ().sprite = sprites [8];
										//8
								} else if (neighborExists [3]) {	//neighbor to left
										GetComponent<SpriteRenderer> ().sprite = sprites [15];
										//15
								}
						
						} else if (neighborExists [1]) {	//neighbor to right
							//and----------------------------------------------------
								if (neighborExists [2]) {	//neighbor below
									//5
									GetComponent<SpriteRenderer> ().sprite = sprites [5];
								} else if (neighborExists [3]) {	//neighbor to the left
									//12
									GetComponent<SpriteRenderer> ().sprite = sprites [12];
								}
						
						} else if (neighborExists [2]) {	//neighbor below
							//must be neighbor to the left
							GetComponent<SpriteRenderer> ().sprite = sprites [7];
						}
					    break;
					case 3:
						//1 no-neighbor
						if (!neighborExists [0]) {		//no neighbor above
							GetComponent<SpriteRenderer> ().sprite = sprites [6];
						}else if(!neighborExists [1]) {	//no neighbor to right
							GetComponent<SpriteRenderer> ().sprite = sprites [11];
						}else if(!neighborExists [2]) {	//no neighbor below
							GetComponent<SpriteRenderer> ().sprite = sprites [14];
						}else if(!neighborExists [3]) {	//no neighbor to left
							GetComponent<SpriteRenderer> ().sprite = sprites [9];
						}
						break;
					case 4:	
						//4 neighbors
						GetComponent<SpriteRenderer> ().sprite = sprites [10];
						break;
				}
		
		
		
		
		
	}
	
}
