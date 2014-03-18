using UnityEngine;
using System.Collections;

//awesome!
[ExecuteInEditMode]

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]


public class TileMapTest : MonoBehaviour
{

		public int sizeX = 100;
		public int sizeZ = 50;
		public int tileResolution = 8;
		public float tileSize = 1.0f;
		public int verticesCount = 4;
		public bool randomHeight = false;
		public Texture2D terranTiles;

		// Use this for initialization
		void Start ()
		{
				BuildMesh ();
		}

	Color[][] ChopUpTiles()
	{
		int numTilesPerRow = terranTiles.width/tileResolution;
		int numRows = terranTiles.height/tileResolution;

		Color[][] tiles = new Color[numTilesPerRow*numRows][];

		for(int y = 0; y < numRows; y++){
			for(int x = 0; x < numTilesPerRow; x++){

				tiles[y*numTilesPerRow +x] = terranTiles.GetPixels(x*tileResolution,y*tileResolution,tileResolution,tileResolution);

			}
		}
		return tiles;
	}
	public void BuildTexture()
	{




		int texWidth = sizeX * tileResolution;
		int texHeight= sizeZ * tileResolution;
	
		Texture2D texture = new Texture2D(texWidth,texHeight);

		Color[][] tiles = ChopUpTiles();

		for(int y = 0; y < sizeZ; y++){
			for(int x = 0; x < sizeX; x++){
				int tileOffset = Random.Range(0,4) * tileResolution;
				Color[] p = tiles[Random.Range(0,tiles.Length)];
				texture.SetPixels(x*tileResolution,y*tileResolution,tileResolution,tileResolution,p);
			}
		}
		texture.filterMode = FilterMode.Point;
		texture.Apply();

		MeshRenderer meshRender = GetComponent<MeshRenderer>();

		meshRender.sharedMaterials[0].mainTexture = texture;

	
		Debug.Log("Done Texture!");
	}

		public	void BuildMesh ()
		{
				int numTiles = sizeX * sizeZ;
				int numTris = numTiles * 2;


				int vSizeX = sizeX + 1;
				int vSizeZ = sizeZ + 1;
				verticesCount = vSizeX * vSizeZ;

				//generage Mesh Data
				Vector3[] vertices = new Vector3[verticesCount];
				Vector3[] normals = new Vector3[verticesCount];
				Vector2[] uv = new Vector2[verticesCount];

				int[] triangles = new int[numTris * 3]; 

				int x, z, vertCount;
	
				for (z = 0; z< vSizeZ; z++) {
						for (x = 0; x< vSizeX; x++) {
								float y = (randomHeight) ? Random.Range (-1f, 1f) : 0.0f;
								vertCount = z * vSizeX + x;
								//we need length of tiles + 1
								vertices [vertCount] = new Vector3 (x * tileSize, y, z * tileSize);
								//all normals point up
								normals [vertCount] = Vector3.up;

								uv [vertCount] = new Vector2 ((float)x / sizeX, (float)z / sizeZ);
		
						}
				}

				for (z = 0; z< sizeZ; z++) {
						for (x = 0; x< sizeX; x++) {
								int squareIndex = z * sizeX + x;
								int triOffset = squareIndex * 6;
								triangles [triOffset + 0] = z * vSizeX + x + 0;
								triangles [triOffset + 2] = z * vSizeX + x + vSizeX + 1;
								triangles [triOffset + 1] = z * vSizeX + x + vSizeX + 0;
				
								triangles [triOffset + 3] = z * vSizeX + x + 0;
								triangles [triOffset + 5] = z * vSizeX + x + 1;
								triangles [triOffset + 4] = z * vSizeX + x + vSizeX + 1;

						}
				}



				//create Mesh and populate data;
				Mesh mesh = new Mesh ();

				mesh.vertices = vertices;
				mesh.triangles = triangles;
				mesh.normals = normals;
				mesh.uv = uv;



				MeshFilter meshFilter = GetComponent<MeshFilter> ();
				MeshRenderer meshRender = GetComponent<MeshRenderer> ();
				MeshCollider meshCollider = GetComponent<MeshCollider> ();
				mesh.name = "TileMap";
				meshFilter.mesh = mesh;
				meshCollider.sharedMesh = mesh;
		BuildTexture();
		}
		
}
