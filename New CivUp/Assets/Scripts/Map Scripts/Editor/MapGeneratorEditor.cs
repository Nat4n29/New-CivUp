using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GeneratorManeger))]
public class MapGeneratorEditor : Editor
{


	public override void OnInspectorGUI()
	{
		GeneratorManeger mapGen = (GeneratorManeger)target;

		if (DrawDefaultInspector())
		{
			if (mapGen.autoUpdate)
			{
				mapGen.Start();
			}
		}

		if (GUILayout.Button("Generate"))
		{
			mapGen.Start();
		}

		if (GUILayout.Button("Clear Map"))
		{
			mapGen.GroundMap.ClearAllTiles();
			mapGen.SubGroundMap.ClearAllTiles();

			mapGen.WaterMap.ClearAllTiles();

			mapGen.ProvinceMap.ClearAllTiles();
			mapGen.ProvinceWaterMap.ClearAllTiles();
			mapGen.CityMap.ClearAllTiles();
			mapGen.StateMap.ClearAllTiles();
			mapGen.CountryMap.ClearAllTiles();
		}
	}
}
