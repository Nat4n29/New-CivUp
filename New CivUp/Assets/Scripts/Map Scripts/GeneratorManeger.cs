using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GeneratorManeger : MonoBehaviour
{
    public GameObject BaseMap;
    public Tilemap GroundMap;
    public Tilemap SubGroundMap;
    public Tilemap WaterMap;

    public int Height;
    public int Width;
    public int scale;

    //____________________________________________________________________________________________________________________
    [SerializeField, Range(0, 8)]
    public int octaves = 4;
    public float persistence = 0.5f;
    public float lacunarity = 2.0f;
    public float SeedX { get; set; } = 0f;
    public float SeedY { get; set; } = 0f;

    [SerializeField, Range(0, 1)]
    public float fallOffStart = 0.7f;
    [SerializeField, Range(0, 1)]
    public float fallOffEnd = 1f;

    public bool autoSeed;
    public bool autoUpdate;

    //Ground______________________________________________________________________________________________________________
    public Tile GroundTile;
    public Tile SubGroundTile;
    [SerializeField, Range(0f, 1f)]
    public float groundNoiseThreshold = 0.5f;

    public TypeTerrain[] _typeTerrain;

    //Water_______________________________________________________________________________________________________________
    public Tile WaterTile;
    public float WaterAltitude;
    public TypeWater[] _typeWater;

    //River______________________________________________________________________________________________________________
    public Tilemap RiverMap;
    public Tile RiverTile;
    public int RiverLength = 10; // Comprimento do rio
    [SerializeField, Range(0f, 1f)]
    public float RiverHeight = 0.9f;

    //Province____________________________________________________________________________________________________________
    public Tilemap ProvinceMap;
    public Tilemap ProvinceWaterMap;
    public Tile tileProv;

    public Tilemap CityMap;
    public Tilemap StateMap;
    public Tilemap CountryMap;

    DataProvince dataProvince;

    //____________________________________________________________________________________________________________________
    public void Start()
    {
        if (autoSeed)
        {
            SeedX = Random.Range(0f, 100f);
            SeedY = Random.Range(0f, 100f);
        }

        dataProvince = FindAnyObjectByType<DataProvince>();

        GroundMapGenerator.GenerateGroundMap(BaseMap,GroundMap,SubGroundMap,Height,Width,scale,octaves,persistence,lacunarity,SeedX,SeedY,fallOffStart,fallOffEnd,GroundTile,SubGroundTile,groundNoiseThreshold,_typeTerrain);
        WaterMapGenerator.GenerateWaterMap(BaseMap, WaterMap, Height,Width,scale, octaves, persistence, lacunarity,SeedX,SeedY,fallOffStart,fallOffEnd,WaterAltitude, WaterTile,_typeWater);
        ProvinceGenerator.GenerateProvinceMap(BaseMap,ProvinceMap,ProvinceWaterMap,CityMap,StateMap,CountryMap,Height,Width,fallOffStart,fallOffEnd,scale,SeedX,SeedY,octaves,persistence,lacunarity,groundNoiseThreshold,tileProv,dataProvince);
    }

    void Update()
    {
        
    }
}

[System.Serializable]
public struct TypeTerrain
{
    public string Name;
    public Tile TerrainTile;
    [SerializeField, Range(0f, 1f)]
    public float Altitude;
}

[System.Serializable]
public struct TypeWater
{
    public string Name;
    public Tile WaterType;
    [SerializeField, Range(0f, 0.52f)]
    public float Altitude;
}
