using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class RoguelikeTest : MonoBehaviour {

    private void Start()
    {
		Mesh room1 = MeshBuilder.BuildQuad(3, 5, 0, 0);
		Mesh room2 = MeshBuilder.BuildQuad(3, 5, -6, 0);

		MeshFilter meshFilter = GetComponent<MeshFilter>();
		MeshRenderer meshRender = GetComponent<MeshRenderer>();
		MeshCollider meshCollider = GetComponent<MeshCollider>();

		CombineInstance[] combine = new CombineInstance[2];

        combine[0].mesh = room1;
        combine[0].transform = transform.localToWorldMatrix;
        combine[1].mesh = room2;
		combine[1].transform = transform.localToWorldMatrix;
		meshFilter.mesh = new Mesh();
        meshFilter.mesh.CombineMeshes(combine,true);
    }



}

public class MeshBuilder : MonoBehaviour
{

    public static Mesh BuildQuad(uint Width = 1, uint Length = 1, int X = 0, int Y = 0)
	{
        //generage Mesh Data
		Vector3[] vertices = new Vector3[4];
		int[] triangles = new int[6];
		Vector3[] normals = new Vector3[4];
		Vector2[] uv = new Vector2[4];

		vertices[0] = new Vector3(X, 0, Y);
        vertices[1] = new Vector3(X + Width, 0, Y);
        vertices[2] = new Vector3(X, 0, Y + Length);
		vertices[3] = new Vector3(X + Width, 0, Y + Length);

		triangles[0] = 0;
		triangles[1] = 3;
		triangles[2] = 2;

		triangles[3] = 0;
		triangles[4] = 1;
		triangles[5] = 3;

		normals[0] = Vector3.up;
		normals[1] = Vector3.up;
		normals[2] = Vector3.up;

        //unity considers the bottom left hand coordinate 0,0
		uv[0] = new Vector2(0, 1);
		uv[1] = new Vector3(1, 1);
		uv[2] = new Vector3(0, 0);
		uv[3] = new Vector3(1, 0);

		//create Mesh and populate data;
		Mesh mesh = new Mesh();

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;

		return mesh; 
    }
        
}
