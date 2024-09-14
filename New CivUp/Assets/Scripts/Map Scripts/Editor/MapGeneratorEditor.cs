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
				ClearDataProvince(mapGen);
				mapGen.Start();
			}
		}

		if (GUILayout.Button("Generate"))
		{
			ClearDataProvince(mapGen);
			mapGen.Start();
		}

		if (GUILayout.Button("Clear Map"))
		{
			mapGen.GroundMap.ClearAllTiles();
			mapGen.SubGroundMap.ClearAllTiles();

			mapGen.WaterMap.ClearAllTiles();
			mapGen.RiverMap.ClearAllTiles();

			mapGen.ProvinceMap.ClearAllTiles();
			mapGen.ProvinceWaterMap.ClearAllTiles();
			mapGen.CityMap.ClearAllTiles();
			mapGen.StateMap.ClearAllTiles();
			mapGen.CountryMap.ClearAllTiles();

			ClearDataProvince(mapGen);
		}
	}

	void ClearDataProvince(GeneratorManeger mapGen)
	{
		if (mapGen.DataProvince._provinces.Count != 0)
		{
			mapGen.DataProvince._provinces.Clear();
			mapGen.DataProvince._waterProvince.Clear();
			mapGen.DataProvince._cities.Clear();
			mapGen.DataProvince._states.Clear();
			mapGen.DataProvince._countries.Clear();
		}
	}
}
