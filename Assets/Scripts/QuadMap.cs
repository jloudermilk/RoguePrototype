using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]


public class QuadMap : MonoBehaviour {



	public int verticesCount = 4;
	// Use this for initialization
	void Start () 
	{
		BuildMesh();
	}
	void BuildMesh()
	{
		//generage Mesh Data
		Vector3[] vertices = new Vector3[verticesCount];
		int[] triangles = new int[6 ]; 
		Vector3[] normals = new Vector3[verticesCount];
		Vector2[] uv = new Vector2[verticesCount];

		vertices[0] = new Vector3(0,0,0);
		vertices[1] = new Vector3(1,0,0);
		vertices[2] = new Vector3(0,0,-1);
		vertices[3] = new Vector3(1,0,-1);

		triangles[0] = 0;
		triangles[1] = 3;
		triangles[2] = 2;

		triangles[3] = 0;
		triangles[4] = 1;
		triangles[5] = 3;



		normals[0] = Vector3.up;
		normals[1] = Vector3.up;
		normals[2] = Vector3.up;
		normals[3] = Vector3.up;
		/*
		 * how it should be 
		uv[0] = new Vector2(0,0);
		uv[1] = new Vector3(1,0);
		uv[2] = new Vector3(0,1);
		uv[3] = new Vector3(1,1);
		*/
		//how it is 
		//unity considers the bottom left hand coordinate 0,0
		uv[0] = new Vector2(0,1);
		uv[1] = new Vector3(1,1);
		uv[2] = new Vector3(0,0);
		uv[3] = new Vector3(1,0);

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
		meshCollider.sharedMesh = mesh;

	}
		
}
