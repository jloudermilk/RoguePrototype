using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DunGenerator))]

public class DunGeneratorInspector : Editor{


		
		
		DunGenerator dunGen;
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			if(GUILayout.Button("Regenerate"))
			{
				dunGen= (DunGenerator) target;
				dunGen.BuildMesh();
			}
			
			
		}
	}
