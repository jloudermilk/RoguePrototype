using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileMap))]
public class TileMapInspector : Editor {
	TileMap tileMap;
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if(GUILayout.Button("Regenerate"))
		{
			tileMap= (TileMap) target;
			tileMap.BuildMesh();
		}


	}
}
