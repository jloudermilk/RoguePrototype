using UnityEngine;
using SRNG;
using System.Collections.Generic;

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
	public int sortPasses = 100;
	public string SeedNumber;

	public struct DelNode
	{
		public Chamber c;
		public List<DelNode> nodes ;
	}


	
	public GameObject Room;
	ChamberList CList;
	ChamberList Bigrooms;
	public List<GameObject> Chams;
	public DelNode Root;
	public LineRenderer lineRenderer;


	// Use this for initialization
	void Start ()
	{
		lineRenderer = gameObject.GetComponent<LineRenderer> ();
		Root = new DelNode ();
		BuildMesh ();

	}

	public void BuildMesh ()
	{
		SimpleRNG.SetSeedFromSystemTime ();
		BuildMesh (0);
	}

	public void BuildMesh (uint seed)
	{
		// seed up the random number generator
		if (seed != 0)
			SimpleRNG.SetSeed (seed);
		SeedNumber = SimpleRNG.m_w.ToString ();



		//initialize some variables for mesh generation
		Vector3[] vertices = new Vector3[0];
		Vector3[] normals = new Vector3[0];
		Vector2[] uv = new Vector2[0];
		//Triangle list
		int[] triangles = new int[0];


		// the list of Chamber data structures 
		CList = new ChamberList ();
		Chamber c;

		//generate a bunch of chambers with randomize data
		for (int i = 0; i < numChambers; i++) {
			
			c = new Chamber ();
			c.SetRange (maxX - minX, minX);
			c.SetData ();
			CList.Add (c);
		}

		//sort through the chambers to have them spaced out
		// after a number of passes remove remaining overlaps 
		for (int i = 0; i < sortPasses; i++) {
			SortChambers ();
			if (i == sortPasses - 1) {
				for (int j = CList.Count-1; j> 0; j--) {
					if (CList [j].Overlapping) {
						CList.RemoveAt (j);
					}
				}
			}
		}


		//generate Mesh data and create game objects that contain those meshes
		//will be moved to a single container
		for (int i = 0; i < CList.Count; i++) {
			
			c = CList [i];
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
					vertices [vertCount].Set (x * tileSize - (vSizeX / 2), 0, -z * tileSize + (vSizeZ / 2));
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
			
			
			GameObject cham = GameObject.Instantiate (Room, new Vector3 (c.CenterX, 0, c.CenterY), Quaternion.identity) as GameObject;
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
			if (c.Width > 6 && c.Height > 6) {
				
				
				meshRender.renderer.material.color = new Color (1, 0, 0, 1);
			}
			
			
			meshFilter.mesh = mesh;
			meshCollider.sharedMesh = mesh;
			cham.GetComponent<ChamberTest> ().Copy (c);
			Chams.Add (cham);
		}
		

		// find all the Larger rooms
		for (int i = 0; i < CList.Count; i++) {
			if (CList [i].Width > 6 && CList [i].Height > 6) {
				Bigrooms.Add (CList [i]);
			}
		}
		Root.nodes = new List<DelNode>(Bigrooms.Count);

		for (int i = 0; i < Bigrooms.Count-3; i++) {
			Root.nodes[i].c = Bigrooms[i];
			Root.nodes.nodes = new List<DelNode>(Bigrooms.Count - i);
			for (int j = i+1; j < Bigrooms.Count-3; j++) {
				Root.nodes[i].nodes[j].c =Bigrooms [j];
				Root.nodes.nodes.nodes = new List<DelNode>(Bigrooms.Count - j);
				for (int k = j+1; k < Bigrooms.Count-3; k++) {
					Root.nodes[i].nodes[j].nodes[k].c =Bigrooms [k];
				}
			}
		
		}

		
	}

	void Update ()
	{

		for (int i = 0; i < Bigrooms.Count; i++) {
			Root.nodes[i].
			for (int j = i+1; j < Bigrooms.Count; j++) {
				Root.nodes [i].nodes.Add (Bigrooms [j]);
				for (int k = j+1; k < Bigrooms.Count; k++) {
					Root.nodes [i].nodes [j].nodes.Add (Bigrooms [k]);
				}
			}
			
		}
		DrawCircle (10, new Vector2 (0, 0));

	}

	void DrawCircle (float radius, Vector2 center)
	{
		float theta_scale = 0.1f;             //Set lower to add more points
		float RHO = (2.0f * Mathf.PI);

		int size = (int)(RHO / theta_scale); //Total number of points in circle.

		lineRenderer.material = new Material (Shader.Find ("Particles/Additive"));
		lineRenderer.SetColors (Color.red, Color.red);
		lineRenderer.SetWidth (0.2F, 0.2F);
		lineRenderer.SetVertexCount (size + 1);
		float x, y;
		int i = 0;
		for (float theta = 0f; theta < RHO; theta += 0.1f) {
			x = radius * Mathf.Cos (theta) + center.x;
			y = radius * Mathf.Sin (theta) + center.y;
			
			Vector3 pos = new Vector3 (x, 1, y);
			lineRenderer.SetPosition (i, pos);
			i += 1;
		}
		i = 0;
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

	/*
	public void SortChambers ()
	{
		
		Chamber c = new Chamber ();
		Chamber c2 = new Chamber ();

		for (int i = 0; i < numChambers; i++) {
			
			c = CList [i];
			c.Neighbors.Clear ();
			c.Overlapping = false;
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
			if (c.moveX != 0) {
				c.Left += 1 * (int)Mathf.Sign (c.moveX);
				if (c.Width % 2 != 0)
					c.Left += 1 * (int)Mathf.Sign (c.moveX);
			}
			if (c.moveY != 0) {
				c.Top += 1 * (int)Mathf.Sign (c.moveY);
				if (c.Height % 2 != 0)
					c.Top += 1 * (int)Mathf.Sign (c.moveY);
			}
			Chams [i].transform.position = new Vector3 (c.CenterX, 0, c.CenterY);
			Chams [i].GetComponent<ChamberTest> ().Copy (c);
		}	
	}
	*/
	/*
	public void SortChambers ()
	{
		
		Chamber c = new Chamber ();
		Chamber c2 = new Chamber ();
		
		for (int i = 0; i < numChambers; i++) {
			
			c = CList [i];
			c.Neighbors.Clear ();
			c.Overlapping = false;
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
			if (c.moveX != 0) {
				c.Left += 1 * (int)Mathf.Sign (c.moveX);
				if (c.Width % 2 != 0)
					c.Left += 1 * (int)Mathf.Sign (c.moveX);
			}
			if (c.moveY != 0) {
				c.Top += 1 * (int)Mathf.Sign (c.moveY);
				if (c.Height % 2 != 0)
					c.Top += 1 * (int)Mathf.Sign (c.moveY);
			}
		
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
			c.Overlapping = false;
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
		
		}
	}
}

