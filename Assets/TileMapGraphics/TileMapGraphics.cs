using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class TileMapGraphics : MonoBehaviour
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
	
	Color[][] ChopUpTiles ()
	{
		int numTilesPerRow = terranTiles.width / tileResolution;
		int numRows = terranTiles.height / tileResolution;
		
		Color[][] tiles = new Color[numTilesPerRow * numRows][];
		
		for (int y = 0; y < numRows; y++) {
			for (int x = 0; x < numTilesPerRow; x++) {
				
				tiles [y * numTilesPerRow + x] = terranTiles.GetPixels (x * tileResolution, y * tileResolution, tileResolution, tileResolution);
				
			}
		}
		return tiles;
	}

	public void BuildTexture ()
	{
		
		TileMapData map = new TileMapData (sizeX, sizeZ);
		
		
		int texWidth = sizeX * tileResolution;
		int texHeight = sizeZ * tileResolution;
		
		Texture2D texture = new Texture2D (texWidth, texHeight);
		
		Color[][] tiles = ChopUpTiles ();
		
		for (int y = 0; y < sizeZ; y++) {
			for (int x = 0; x < sizeX; x++) {
				int check = (int)map.GetTile (x, y);
				Color[] p = tiles [check];
				texture.SetPixels (x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
			}
		}
		texture.filterMode = FilterMode.Point;
		texture.Apply ();
		
		MeshRenderer meshRender = GetComponent<MeshRenderer> ();
		
		meshRender.sharedMaterials [0].mainTexture = texture;
		
		
		Debug.Log ("Done Texture!");
	}
	
	public	void BuildMesh ()
	{
	


		int numTiles = sizeX * sizeZ;
		int numTris = numTiles * 2;

		Vector3[] vertices;
		Vector3[] normals;
		Vector2[] uv;
		
		int[] triangles = new int[numTris * 3]; 
		
		if (!randomHeight) {
			int vSizeX = sizeX + 1;
			int vSizeZ = sizeZ + 1;
			verticesCount = vSizeX * vSizeZ;
		
			//generage Mesh Data
			 vertices = new Vector3[verticesCount];
			 normals = new Vector3[verticesCount];
			uv = new Vector2[verticesCount];
		
			int x, z, vertCount;
		
			for (z = 0; z< vSizeZ; z++) {
				for (x = 0; x< vSizeX; x++) {
			
					vertCount = z * vSizeX + x;
					//we need length of tiles + 1
					vertices [vertCount].Set (x * tileSize, 0, -z * tileSize);
					//all normals point up
					normals [vertCount].Set (0, 1, 0); 
					//seting UV Coord
					uv [vertCount] .Set ((float)x / sizeX, 1f - (float)z / sizeZ);




				
				}
			}
		
			for (z = 0; z< sizeZ; z++) {
				for (x = 0; x< sizeX; x++) {
					int squareIndex = z * sizeX + x;
					int triOffset = squareIndex * 6;
					triangles [triOffset + 0] = z * vSizeX + x + 0;
					triangles [triOffset + 1] = z * vSizeX + x + vSizeX + 1;
					triangles [triOffset + 2] = z * vSizeX + x + vSizeX + 0;
				
					triangles [triOffset + 3] = z * vSizeX + x + 0;
					triangles [triOffset + 4] = z * vSizeX + x + 1;
					triangles [triOffset + 5] = z * vSizeX + x + vSizeX + 1;
				
				}
			}
		} else {

			verticesCount = sizeX * sizeZ * 4;
			
			//generage Mesh Data
			 vertices = new Vector3[verticesCount];
			 normals = new Vector3[verticesCount];
			 uv = new Vector2[verticesCount];
			
			 
			
			int x, z, vertCount =0;

			for (z = 0; z< sizeZ; z++) {
				for (x = 0; x< sizeX; x++) {
					float y = Random.Range (-1f, 1f);
						
					//we need length of tiles + 1
					vertices [vertCount].Set (x * tileSize, y, -z * tileSize);
					//all normals point up
					normals [vertCount].Set (0, 1, 0); 
					//seting UV Coord
					uv [vertCount] .Set ((float)x / sizeX, 1f - (float)z / sizeZ);
						
						
					vertices [vertCount + 1].Set (x * tileSize + 1, y, -z * tileSize);
					//all normals point up
					normals [vertCount + 1].Set (0, 1, 0); 
					//seting UV Coord
					uv [vertCount + 1] .Set ((float)x / sizeX, 1f - (float)z / sizeZ);
						
						
					vertices [vertCount + 2].Set (x * tileSize, y, -z * tileSize - 1);
					//all normals point up
					normals [vertCount + 2].Set (0, 1, 0); 
					//seting UV Coord
					uv [vertCount + 2] .Set ((float)x / sizeX, 1f - (float)z / sizeZ);
						
						
					vertices [vertCount + 3].Set (x * tileSize + 1, y, -z * tileSize - 1);
					//all normals point up
					normals [vertCount + 3].Set (0, 1, 0); 
					//seting UV Coord
					uv [vertCount + 3] .Set ((float)x / sizeX, 1f - (float)z / sizeZ);
						
						
					int squareIndex = z * sizeX + x;
					int triOffset = squareIndex * 6;
					triangles [triOffset + 0] = vertCount;
					triangles [triOffset + 1] = vertCount + 1;
					triangles [triOffset + 2] = vertCount + 2;
						
					triangles [triOffset + 3] = vertCount + 1;
					triangles [triOffset + 4] = vertCount + 2;
					triangles [triOffset + 5] = vertCount + 3;
						
						
					vertCount += 4;
				}
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
		BuildTexture ();
	}
	
}

