using UnityEngine;
using System.Collections;


//awesome!
[ExecuteInEditMode]

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]


public class TileMap : MonoBehaviour {

	public int sizeX = 100;
	public int sizeZ = 50;
	public float tileSize = 1.0f;

	public int verticesCount = 4;

	// Use this for initialization
	void Start () 
	{
		BuildMesh();
	}
public	void BuildMesh()
	{	int numTiles = sizeX * sizeZ;
		int numTris = numTiles *2;


		int vSizeX = sizeX+1;
		int vSizeZ = sizeZ+ 1;
		verticesCount = vSizeX * vSizeZ;

		//generage Mesh Data
		Vector3[] vertices = new Vector3[verticesCount];
		Vector3[] normals = new Vector3[verticesCount];
		Vector2[] uv = new Vector2[verticesCount];

		int[] triangles = new int[numTris *3]; 

		int x,z,vertCount;
		for(z = 0; z< vSizeZ;z++){
			for(x = 0; x< vSizeX;x++){
				 vertCount = z * vSizeX + x;
				//we need length of tiles + 1
				vertices[vertCount] = new Vector3(x*tileSize,0,z*tileSize);
				//all normals point up
				normals[vertCount] = Vector3.up;

				uv[vertCount] = new Vector2((float)x/vSizeX,(float)z/vSizeZ);

				Debug.Log("Vert"+(z * vSizeX + x)+vertices[z * vSizeX + x]);
			}
		}

		for(z = 0; z< sizeZ;z++){
			for(x = 0; x< sizeX;x++){
				int squareIndex = z * sizeX + x;
				int triOffset = squareIndex * 6;
				triangles[triOffset + 0] = z *vSizeX + x +  		0;
				triangles[triOffset + 2] = z *vSizeX + x + vSizeX + 1;
				triangles[triOffset + 1] = z *vSizeX + x + vSizeX + 0;
				
				triangles[triOffset + 3] = z *vSizeX + x +			0;
				triangles[triOffset + 5] = z *vSizeX + x + 			1;
				triangles[triOffset + 4] = z *vSizeX + x + vSizeX + 1;

				Debug.Log("Triangle 1: Point 1: " +triangles[triOffset + 0]+"Point 2: "+triangles[triOffset + 1]+"Point 3: "+triangles[triOffset + 2]);
				Debug.Log("Triangle 2: Point 1: " +triangles[triOffset + 3]+"Point 2: "+triangles[triOffset + 4]+"Point 3: "+triangles[triOffset + 5]);

			}
		}



		//create Mesh and populate data;
		Mesh mesh = new Mesh();

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;











		MeshFilter meshFilter = GetComponent<MeshFilter>();
		MeshRenderer meshRender = GetComponent<MeshRenderer>();
		MeshCollider meshCollider = GetComponent<MeshCollider>();

		meshFilter.mesh = mesh;
	}
		
}
