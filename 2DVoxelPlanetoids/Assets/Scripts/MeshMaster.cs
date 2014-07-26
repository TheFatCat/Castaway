using UnityEngine;
using System.Collections;

public class MeshMaster : MonoBehaviour {
	//master class that keeps track of all pvm's

	public Transform player;
	//public GameObject block;
	//public Vector2 limitSize;	//length of the limits
	public Vector2 tileSize;
	//
	private Vector2 maxpos;
	private Vector2 minpos;
	private Vector2 maxTileEdge;
	private Vector2 minTileEdge;
	//public bool pos = true;	//true if in middle, false if at edge
	private int u = 1;	//index of next chunk
	public int nextX = 1;
	public int prevX = 0;
	public int nextY = 0;
	public int prevY = 1;
	
	//
	public GameObject pvm;	//the actual mesh prefab to be instantiated
	public GameObject[,] meshes = new GameObject[3,3];
	public GameObject[] meshes3 = new GameObject[3];
	public GameObject[] bgMeshes = new GameObject[3];
	
	// Use this for initialization
	void Start () {
		Debug.Log(meshes[0,0]);
		//assume player is at halfway of the middle tile
		maxTileEdge = new Vector2(player.position.x + tileSize.x * 0.5f,player.position.y + tileSize.y * 0.5f);
		minTileEdge = new Vector2(player.position.x - tileSize.x * 0.5f,player.position.y - tileSize.y * 0.5f);
		maxpos = new Vector2(player.position.x,player.position.y );
		minpos = new Vector2(player.position.x - tileSize.x,player.position.y - tileSize.y);
		
		//instantiate first meshes.
		meshes[0,0] = Instantiate(pvm,Vector3.zero,Quaternion.identity) as GameObject;
		meshes[0,0].GetComponent<PerlinVoxelMesh>().setOffset(0,2);	//0 is first x, 2 is biggest y (in a world of 3, 2)
		meshes[1,0] = Instantiate(pvm,Vector3.zero,Quaternion.identity) as GameObject;
		meshes[1,0].GetComponent<PerlinVoxelMesh>().setOffset(1,2);
		//instantiate next row
		meshes[0,1] = Instantiate(pvm,Vector3.zero,Quaternion.identity) as GameObject;
		meshes[0,1].GetComponent<PerlinVoxelMesh>().setOffset(0,1);	//0 is first x, 1 is next largest y
		meshes[1,1] = Instantiate(pvm,Vector3.zero,Quaternion.identity) as GameObject;
		meshes[1,1].GetComponent<PerlinVoxelMesh>().setOffset(1,1);
		u = 2;
		
		//set previous and next block identities
		nextX = 2;	//formerly 'u'
		prevX = -1;
		nextY = 0;
		prevY = 3;
		
		
	}
	
	void ShiftRegisterLeft(){	//shifts register one to the left, and will tell update references inside as well
		for(int i=0;i<3;i++){
			//meshes col 0 is overwritten!
			//delete column 0
			Destroy(meshes[0,i]);
			//shift columns
			meshes[0,i] = meshes[1,i];
			meshes[1,i] = meshes[2,i];
			meshes[2,i] = null;
		}
		//move mesh master
		Vector3 position = transform.position;
		position.x += tileSize.x;
		transform.position = position;
	}
	
	void ShiftRegisterUp(){	//shifts register one up, and will update references inside as well
		Debug.Log ("shifted up");
		for(int j=0;j<3;j++){
			//meshes row 0 is overwritten!
			//delete row 0
			Destroy(meshes[j,0]);
			//shift rows
			meshes[j,0] = meshes[j,1];
			meshes[j,1] = meshes[j,2];
			meshes[j,2] = null;
		}
		//move mesh master
		Vector3 position = transform.position;
		position.y -= tileSize.y;
		transform.position = position;
	}
	
	void ShiftRegisterRight(){	//shifts register one to the right, and will tell update references inside as well
		for(int i=0;i<3;i++){
			//meshes[2] col is overwritten, so delete it!
			Destroy(meshes[2,i]);
			meshes[2,i] = meshes[1,i];
			meshes[1,i] = meshes[0,i];
			meshes[0,i] = null;
		}
		//move mesh master
		Vector3 position = transform.position;
		position.x -= tileSize.x;
		transform.position = position;
	}
	
	void ShiftRegisterDown(){	//shifts register one Down, and will update references inside as well
		Debug.Log ("shifted down----");
		for(int j=0;j<3;j++){
			//meshes[2] row is overwritten, so delete it!
			Destroy(meshes[j,2]);
			meshes[j,2] = meshes[j,1];
			meshes[j,1] = meshes[j,0];
			meshes[j,0] = null;
		}
		//move mesh master
		Vector3 position = transform.position;
		position.y += tileSize.y;
		transform.position = position;
	}
	
	// Update is called once per frame
	void Update () {
	
		float x = player.position.x;	//coordinate changes would hapen here
		if(x > maxpos.x){
			//move right boundary, instantiate tile in [2]
			maxpos.x += tileSize.x;
			if(meshes[2,1] == null){
				for(int i=0;i<3;i++){
					if(meshes[1,i] != null){
						meshes[2,i] = Instantiate(pvm,Vector3.zero,Quaternion.identity) as GameObject;
						meshes[2,i].GetComponent<PerlinVoxelMesh>().setOffset(nextX,prevY - 1-i);
					}
				}	
				u++;
				nextX++;
			}else{
				Debug.Log ("FAILED AT INSTANTIATING 2");
			}
			//u++;
			
		}
		if(x > maxTileEdge.x){
			if(meshes[2,1] == null){//empty mesh in 3d slot, do alt. right edge
				maxTileEdge.x += tileSize.x;
				minTileEdge.x += tileSize.x;
			}else{//do normal right edge
				//move left bound, move left edge, move right edge, shift register to left, delete tile in [0]
				minpos.x += tileSize.x;
				minTileEdge.x += tileSize.x;
				maxTileEdge.x += tileSize.x;
				ShiftRegisterLeft();
				prevX++;
			}
		}
		if(x < minpos.x){
			//move left bound, shift register RIGHT, instantiate tile in [0], shift pos (1)
			minpos.x -= tileSize.x;
			ShiftRegisterRight();
			for(int i=0;i<3;i++){
				if(meshes[1,i]!= null){	//only instantiate if theres nothing there
					meshes[0,i] = Instantiate(pvm,Vector3.zero,Quaternion.identity) as GameObject;
					meshes[0,i].GetComponent<PerlinVoxelMesh>().setOffset(prevX,prevY - 1 - i);
				}else{
					Debug.Log ("FAILeD at instantiating 0");
				}
			}
			prevX--;
			//pos = true;
		}
		if(x < minTileEdge.x){
			if(meshes[2,1] == null){	//empty mesh in 3d slot (there's always a mesh in the middle of a nonempty column)
				//move left edge, move right edge, move right bound, delete tile[2] col switch pos
				minTileEdge.x -= tileSize.x;
				maxTileEdge.x -= tileSize.x;
			}else{	//no empty mesh in 3d slot
				//move left edge, move right edge, move right bound, delete tile[2] col switch pos
				minTileEdge.x -= tileSize.x;
				maxpos.x -= tileSize.x;
				maxTileEdge.x -= tileSize.x;
				for(int i=0;i<3;i++){
					Destroy(meshes[2,i]);
					meshes[2,i] = null;
				}
				nextX--;
			}
		}
		
		float y = player.position.y;	//coordinate changes would happen here
		if(y > maxpos.y){
			//corresponds to x < minpos.x
			//move left bound, shift register RIGHT, instantiate tile in [0], shift pos (1)
			maxpos.y += tileSize.y;
			ShiftRegisterDown();
			
			for(int j=0;j<3;j++){
				if(meshes[j,0]==null && meshes[j,1]!= null){	//only instantiate if theres nothing there, and a block below
					meshes[j,0] = Instantiate(pvm,Vector3.zero,Quaternion.identity) as GameObject;
					meshes[j,0].GetComponent<PerlinVoxelMesh>().setOffset(prevX + 1 + j,prevY);
				}else{
					Debug.Log ("FAILeD at instantiating 0");
				}
			}
			prevY++;
			
		}
		if(y > maxTileEdge.y){
			//corresponds to x < minTileEdge.x
			if(meshes[1,2] == null){	//empty mesh in 3d slot (there's always a mesh in the middle of a nonempty row)
				//move top edge, move bottom edge, move bottom bound, delete tile[2] row switch pos
				minTileEdge.y += tileSize.y;
				maxTileEdge.y += tileSize.y;
			}else{	//no empty mesh in 3d slot
				//move top edge, move bottom edge, move bottom bound, delete tile[2] row switch pos
				minTileEdge.y += tileSize.y;
				minpos.y += tileSize.y;
				maxTileEdge.y += tileSize.y;
				for(int j=0;j<3;j++){
					Destroy(meshes[j,2]);
					meshes[j,2] = null;
				}
				nextY++;
			}
		}
		if(y < minpos.y){
			//corresponds to x > maxpos.x
			//move lower boundary, instantiate tile in [2] row
			minpos.y -= tileSize.y;
			if(meshes[1,2] == null){
				for(int j=0;j<3;j++){
					if(meshes[j,1] != null){
						Debug.Log ("filled " + j + " , " + "2 with a thing");
						meshes[j,2] = Instantiate(pvm,Vector3.zero,Quaternion.identity) as GameObject;
						meshes[j,2].GetComponent<PerlinVoxelMesh>().setOffset(prevX + 1 +j,nextY);
					}
				}	
				
				nextY--;
			}else{
				Debug.Log ("FAILED AT INSTANTIATING 2");
			}
			
		
		}
		if(y < minTileEdge.y){
			//corresponds to x > maxTileEdge.x
			if(meshes[1,2] == null){//empty mesh in 3d slot, do alt. right edge
				maxTileEdge.y -= tileSize.y;
				minTileEdge.y -= tileSize.y;
			}else{//do normal right edge
				//move left bound, move left edge, move right edge, shift register to left, delete tile in [0]
				maxpos.y -= tileSize.y;
				minTileEdge.y -= tileSize.y;
				maxTileEdge.y -= tileSize.y;
				ShiftRegisterUp();
				prevY--;
			}
		}
		
		
		
		/*
		if(player.position.x > maxpos.x && meshes[2]!=1){	//getting close to right side
			//load next right
			meshes[2] = 1;
			minpos.x += limitSize.x;
		}
		if(player.position.x > maxTileEdge.x){	//reached right edge!! forget about left
			//unload left
			meshes[0] = 0;
		}
		if(player.position.x < minpos.x && meshes[0]!=1){	//getting close to left edge
			//load next left
			meshes[0] = 1;
			minpos.x -= limitSize.x;
		}
		if(player.position.x > maxTileEdge.x){	//reached left edge! forget about right
			//unload left
			meshes[2] = 0;
		
		}
		*/
		
		Debug.DrawLine(new Vector3(maxpos.x,maxpos.y,0.0f),new Vector3(minpos.x,maxpos.y,0.0f),Color.blue);
		Debug.DrawLine(new Vector3(minpos.x,maxpos.y,0.0f),new Vector3(minpos.x,minpos.y,0.0f),Color.blue);
		Debug.DrawLine(new Vector3(minpos.x,minpos.y,0.0f),new Vector3(maxpos.x,minpos.y,0.0f),Color.blue);
		Debug.DrawLine(new Vector3(maxpos.x,minpos.y,0.0f),new Vector3(maxpos.x,maxpos.y,0.0f),Color.blue);
		Debug.DrawLine(new Vector3(maxTileEdge.x,maxTileEdge.y,0.0f),new Vector3(minTileEdge.x,maxTileEdge.y,0.0f),Color.red);
		Debug.DrawLine(new Vector3(minTileEdge.x,maxTileEdge.y,0.0f),new Vector3(minTileEdge.x,minTileEdge.y,0.0f),Color.red);
		Debug.DrawLine(new Vector3(minTileEdge.x,minTileEdge.y,0.0f),new Vector3(maxTileEdge.x,minTileEdge.y,0.0f),Color.red);
		Debug.DrawLine(new Vector3(maxTileEdge.x,minTileEdge.y,0.0f),new Vector3(maxTileEdge.x,maxTileEdge.y,0.0f),Color.red);
		
	}
	
	public void DebugArray(){
			for(int i=0;i<3;i++){
				string s = " ";
				for(int j=0;j<3;j++){
					bool b = meshes[i,j]==null;
					Debug.Log (b);
				}
				Debug.Log(s);
				Debug.Log ("..............");
			}
			Debug.Log ("---------------");
		
	}
}
