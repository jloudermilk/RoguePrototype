using UnityEngine;
using SRNG;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class DunGenerator : MonoBehaviour
{

	public int minX = 3, minY = 3;
	public int  maxX = 11, maxY = 11;
	public int numChambers = 1;
	public int verticesCount = 4;
	public int tileSize = 1;
	public GameObject Cham;


	public class Chamber
	{
		public float Left, Top, Bottom, Right, CenterX, CenterY;
		public int Width, Height;
		public int Range = 8, Offset = 3;

		public Chamber ()
		{
			SimpleRNG.SetSeed (31337);

		}

		public Chamber (uint seed)
		{
			SimpleRNG.SetSeed (seed);

		}

		public void SetRange (int range, int offset)
		{
			Range = range;
			Offset = offset;
		}

		public void SetData ()
		{
			Width = (int)(SimpleRNG.GetUniform () * Range) + Offset;
			Height = (int)(SimpleRNG.GetUniform () * Range) + Offset;
			Top = (float)(SimpleRNG.GetUniform () * Range) + Offset;
			Left = (float)(SimpleRNG.GetUniform () * Range) + Offset;

			Right = Left + Width;
			Bottom = Top + Height;
			CenterX = Left + Width / 2;
			CenterY = Top + Height / 2;

		}

		public void SetData (int width, int height)
		{
			Width = width;
			Height = height;
			Top = (float)(SimpleRNG.GetUniform () * Range) + Offset;
			Left = (float)(SimpleRNG.GetUniform () * Range) + Offset;
			Right = Left + Width;
			Bottom = Top + Height;
			CenterX = Left + Width / 2;
			CenterY = Top + Height / 2;
			
		}

		public void SetData (float top, float left)
		{
			Width = (int)(SimpleRNG.GetUniform () * Range) + Offset;
			Height = (int)(SimpleRNG.GetUniform () * Range) + Offset;
			Top = top;
			Left = left;
			Right = Left + Width;
			Bottom = Top + Height;
			CenterX = Left + Width / 2;
			CenterY = Top + Height / 2;
			
		}

		public void SetData (float top, float left, int width, int height)
		{
			Width = width;
			Height = height;
			Top = top;
			Left = left;
			Right = Left + Width;
			Bottom = Top + Height;
			CenterX = Left + Width / 2;
			CenterY = Top + Height / 2;
		}


	}


	// Use this for initialization
	void Start ()
	{
		BuildMesh (0);
	}

	public void BuildMesh ()
	{
		SimpleRNG.SetSeedFromSystemTime ();
		BuildMesh (0);
	}

	public void BuildMesh (uint seed)
	{
		if (seed != 0)
			SimpleRNG.SetSeed (seed);

		Vector3[] vertices = new Vector3[0];
		Vector3[] normals = new Vector3[0];
		Vector2[] uv = new Vector2[0];
		
		int[] triangles = new int[0];

		for (int i = 0; i < numChambers; i++) {

			Chamber c = new Chamber ();
			c.SetRange (maxX - minX, minX);
			c.SetData ();
			int numTiles = c.Width * c.Height;
			int numTris = numTiles * 2;

			int vSizeX = c.Width + 1;
			int vSizeZ = c.Height + 1;
			int numVerts = vSizeX * vSizeZ;

			vertices = new Vector3[numVerts];
			normals = new Vector3[numVerts];
			uv = new Vector2[numVerts];

			triangles = new int[numTris * 3]; 

			int x, z, vertCount;

			for (z = 0; z< vSizeZ; z++) {
				for (x = 0; x< vSizeX; x++) {
					
					vertCount = z * vSizeX + x;
					//we need length of tiles + 1
					vertices [vertCount].Set (x * tileSize, 0, -z * tileSize);
					//all normals point up
					normals [vertCount].Set (0, 1, 0); 
					//seting UV Coord
					uv [vertCount] .Set ((float)x / c.Width, 1f - (float)z / c.Height);


		
				}

			}
			for (z = 0; z< c.Height; z++) {
				for (x = 0; x< c.Width; x++) {
					int squareIndex = z * c.Width + x;
					int triOffset = squareIndex * 6;
					triangles [triOffset + 0] = z * vSizeX + x + 0;
					triangles [triOffset + 1] = z * vSizeX + x + vSizeX + 1;
					triangles [triOffset + 2] = z * vSizeX + x + vSizeX + 0;
					
					triangles [triOffset + 3] = z * vSizeX + x + 0;
					triangles [triOffset + 4] = z * vSizeX + x + 1;
					triangles [triOffset + 5] = z * vSizeX + x + vSizeX + 1;
					
				}
			}


			GameObject cham = GameObject.Instantiate(Cham,new Vector3(c.Left,0,c.Top),Quaternion.identity) as GameObject;
			//create Mesh and populate data;
			Mesh mesh = new Mesh ();

			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.normals = normals;
			mesh.uv = uv;
			MeshFilter meshFilter = cham.GetComponent<MeshFilter> ();
			MeshRenderer meshRender = cham.GetComponent<MeshRenderer> ();
			MeshCollider meshCollider = GetComponent<MeshCollider> ();
			mesh.name = "Chamber " + i;
			meshFilter.mesh = mesh;
			meshCollider.sharedMesh = mesh;
		
		}


	}
}
