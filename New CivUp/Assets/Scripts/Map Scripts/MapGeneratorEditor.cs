using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GroundMapGenerator))]
public class MapGeneratorEditor : Editor
{

	public override void OnInspectorGUI()
	{
		GroundMapGenerator mapGen = (GroundMapGenerator)target;

		if (DrawDefaultInspector())
		{
			if (mapGen.autoUpdate)
			{
				mapGen.GenerateGroundMap();
			}
		}

		if (GUILayout.Button("Generate"))
		{
			mapGen.GenerateGroundMap();
		}

		if (GUILayout.Button("Clear Map"))
		{
			mapGen.GroundMap.ClearAllTiles();
			mapGen.SubGroundMap.ClearAllTiles();
		}
	}
}
