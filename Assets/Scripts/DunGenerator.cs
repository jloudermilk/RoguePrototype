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
	public string SeedNumber;

	public struct roomConnection
	{
		public	Vector3 root, n1, n2;
		public	float d1, d2;
	}
	
	public GameObject Room;
	ChamberList CList;
	public List<GameObject> Chams;
	public List<roomConnection> BigRooms;




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

		Chams.ForEach(delegate {
			GameObject.Destroy(gameObject);
	});
		Chams.Clear();
		if (seed != 0)
			SimpleRNG.SetSeed (seed);

		SeedNumber = SimpleRNG.m_w.ToString();

		BigRooms = new List<roomConnection> ();
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
					vertices [vertCount].Set (x * tileSize - (vSizeX/2), 0, -z * tileSize + (vSizeZ/2));
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

	 	for (int i = 0; i < 20; i++) {
			
			SortChambers ();
		}
		 
		for (int i = 0; i < numChambers; i++) {
			if (CList[i].Width > 6 && CList[i].Height > 6) {
		
				roomConnection temp = new roomConnection ();
				temp.d1 = temp.d2 = Mathf.Infinity;
				temp.root = new Vector3 (CList[i].CenterX , 0,CList[i].CenterY);
				BigRooms.Add (temp);
			}
		}
		
		for (int k = 0; k< BigRooms.Count; k++) {
			for (int j = 0; j< BigRooms.Count; j++) {
				if (k != j) {
					float dis = Vector3.Distance (BigRooms [k].root, BigRooms [j].root);

					if (dis < BigRooms [k].d1) {
						roomConnection r1 = BigRooms [k];
						r1.d1 = dis;
						r1.n1 = BigRooms [j].root;
						BigRooms [k] = r1;
					} else if (dis < BigRooms [k].d2) {
						roomConnection r1 = BigRooms [k];
						r1.d2 = dis;
						r1.n2 = BigRooms [j].root;
						BigRooms [k] = r1;
					}
				}
			}
		}

	}
		
	void Update()
	{
		foreach (roomConnection r1 in BigRooms) {
			Debug.DrawLine (r1.root, r1.n1, Color.green );
			Debug.DrawLine (r1.root, r1.n2, Color.green);
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
}

