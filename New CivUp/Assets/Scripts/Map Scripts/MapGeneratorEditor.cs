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
		}
	}
}

[CustomEditor(typeof(ProvinceGenerator))]
public class ProvinceGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		ProvinceGenerator ProvGen = (ProvinceGenerator)target;

		if (DrawDefaultInspector())
		{
			if (ProvGen.autoUpdate)
			{
				ProvGen.GenerateProvinceMap();
			}
		}

		if (GUILayout.Button("Generate"))
		{
			ProvGen.GenerateProvinceMap();
		}

		if (GUILayout.Button("Clear Map"))
		{
			ProvGen.ProvinceMap.ClearAllTiles();
		}
	}
}
