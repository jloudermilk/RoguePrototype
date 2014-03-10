using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileMapGraphics))]
public class TileMapGraphicsInspector : Editor {


	TileMapGraphics tileMap;
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			if(GUILayout.Button("Regenerate"))
			{
			tileMap= (TileMapGraphics) target;
				tileMap.BuildMesh();
			}
			
			
		}
	}
