using UnityEngine;
using SRNG;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class DunGenerator : MonoBehaviour
{
	public class ChamberList : List<Chamber>
	{
	}

	public int minX = 3, minY = 3;
	public int  maxX = 11, maxY = 11;
	public int numChambers = 1;
	public int verticesCount = 4;
	public int tileSize = 1;


	public GameObject Cham;
	ChamberList CList;
	public List<GameObject> Chams;

	public class Chamber
	{
		public int Left, Top;
		public int Width, Height;
		public int Range = 8, Offset = 3;
		public bool Overlapping = false;
		public int moveX;
		public int moveY;
		public ChamberList Neighbors;

		public int Right {
			get{ return Left + Width - 1;}
		}
		
		public int Bottom {
			get{ return Top + Height - 1;}
		}
		
		public float CenterX {
			get{ return (float)(Left + Width / 2f);}
		}
		
		public float CenterY {
			get{ return (float)(Top + Height / 2f);}
		}

		public Chamber ()
		{
			Neighbors = new ChamberList ();
			SimpleRNG.SetSeed (31337);

		}

		public Chamber (uint seed)
		{
			Neighbors = new ChamberList ();
			SimpleRNG.SetSeed (seed);

		}

		public bool Equals (Chamber other)
		{
			return Left == other.Left && Top == other.Top && Width == other.Width && Height == other.Height;
			
		}

		public void SetRange (int range, int offset)
		{
			Range = range;
			Offset = offset;
		}

		public void SetData ()
		{
			Width = (int)(SimpleRNG.GetNormal(0.25,0.25) * Range) + Offset+1;
			if(Width < 3)
				Width = 3;
			Height = (int)(SimpleRNG.GetNormal(0.25,0.25)  * Range) + Offset+1;
			if(Height < 3)
				Height = 3;
			if(Height > 6 && Width < 6)
				Height = 3;
			if(Height < 6 && Width > 6)
						Width = 3;

			Top = (int)(SimpleRNG.GetNormal () * (Range ));
			Left = (int)(SimpleRNG.GetNormal () * (Range ));

	

		}

		public void SetData (int width, int height)
		{
			Width = width;
			Height = height;
			Top = (int)(SimpleRNG.GetUniform () * Range) + Offset;
			Left = (int)(SimpleRNG.GetUniform () * Range) + Offset;

			
		}

		public void SetData (int top, int left, int width, int height)
		{
			Width = width;
			Height = height;
			Top = top;
			Left = left;

		}

		public bool CollidesWith (Chamber other)
		{
			if (Left > other.Right)
				return false;

			if (Top > other.Bottom)
				return false;
		
			if (Right < other.Left)
				return false;

			if (Bottom < other.Top)
				return false;

			return true;
		}

		public void Separate (Chamber other)
		{
			float distX  = CenterX - other.CenterX;
			float distY = CenterY - other.CenterY;


			if(distX > 0)
				moveX++;
			else
				moveX--;
			if(distY> 0)
				moveY++;
			else
				moveY--;



//			moveX  = CenterX - other.CenterX;
//			moveY = CenterY - other.CenterY;
//			

		}



	}


	// Use this for initialization
	void Start ()
	{
		BuildMesh ();
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

		CList = new ChamberList ();
		Chamber c;

		for (int i = 0; i < numChambers; i++) {
			
			c = new Chamber ();
			CList.Add (c);
		}


		for (int i = 0; i < numChambers; i++) {
						
			c = CList [i];
		
		
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


			GameObject cham = GameObject.Instantiate (Cham, new Vector3 (c.Left, 0, c.Top), Quaternion.identity) as GameObject;
			//create Mesh and populate data;
			Mesh mesh = new Mesh ();

			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.normals = normals;
			mesh.uv = uv;
			MeshFilter meshFilter = cham.GetComponent<MeshFilter> ();
			MeshRenderer meshRender = cham.GetComponent<MeshRenderer> ();
			MeshCollider meshCollider = cham.GetComponent<MeshCollider> ();
			mesh.name = "Chamber " + i;
			cham.name = "Chamber " + i;
			if(c.Width > 6 && c.Height >6)
				meshRender.renderer.material.color = new Color(1,0,0,1);
			meshFilter.mesh = mesh;
			meshCollider.sharedMesh = mesh;

			Chams.Add (cham);
		}
		for(int i  = 0; i < 100;i++)
		{
			SortChambers();
		}


	}
	/*
	public void SortChambers ()
	{

		Chamber c = new Chamber ();
		for (int i = 0; i < numChambers; i++) {
				
			c = CList [i];
			foreach (Chamber other in CList) {
				if (c.CollidesWith (other)) {
					c.Neighbors.Add (other);
					c.Separate (other);
				}
			}

		}

		for (int i = 0; i < numChambers; i++) {

		}


	}

	*/
	public void SortChambers ()
	{
		
		Chamber c = new Chamber ();
		Chamber c2 = new Chamber ();

		for (int i = 0; i < numChambers; i++) {
			
			c = CList [i];
			c.Neighbors.Clear ();
			c.moveX = 0;
			c.moveY = 0;
			for (int j = 0; j < numChambers; j++) {
				c2 = CList [j];
				if (!(c.Equals (c2)) && c.CollidesWith (c2)) {
					c.Neighbors.Add (c2);
					c.Separate (c2);
				}
			}
		}
			

		
		for (int i = 0; i < numChambers; i++) {
			c = CList [i];
			c.Left += c.moveX;
			c.Top += c.moveY;
			Chams [i].transform.position += new Vector3 (c.moveX, 0, c.moveY);
		}
	}
}

