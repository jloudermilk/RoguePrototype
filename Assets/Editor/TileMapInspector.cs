using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileMapTest))]
public class TileMapInspector : Editor {
	TileMapTest tileMap;
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if(GUILayout.Button("Regenerate"))
		{
			tileMap= (TileMapTest) target;
			tileMap.BuildMesh();
		}


	}
}
