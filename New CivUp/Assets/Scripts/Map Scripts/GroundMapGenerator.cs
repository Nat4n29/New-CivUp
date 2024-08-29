//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundMapGenerator : MonoBehaviour
{
    public GameObject BaseMap;
    public Tilemap GroundMap;
    public Tilemap SubGroundMap;

    public int Height;
    public int Width;
    public int scale;

    //Ground
    public Tile GroundTile;
    public Tile SubGroundTile;
    [SerializeField, Range(0f, 1f)]
    public float groundNoiseThreshold = 0.5f;
    public float groundValue;

    public TypeTerrain[] _typeTerrain;

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [SerializeField, Range(0, 8)]
    public int octaves = 4;
    public float persistence = 0.5f;
    public float lacunarity = 2.0f;
    public float SeedX { get; set; } = 0f;
    public float SeedY { get; set; } = 0f;
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public bool autoSeed;
    public bool autoUpdate;

    public void Start()
    {
        GenerateGroundMap();
    }

    // PerlinNoise Map Generator with Octaves, Persistence, and Lacunarity
    public void GenerateGroundMap()
    {
        if (autoSeed)
        {
            SeedX = Random.Range(0f, 100f);
            SeedY = Random.Range(0f, 100f);
        }

        // Limpa a Tilemap antes de gerar um novo mapa
        GroundMap.ClearAllTiles();

        // Obtenha a escala e posição do BaseMap
        Vector3 baseMapScale = BaseMap.transform.localScale;
        Vector3 baseMapPosition = BaseMap.transform.position;

        // Calcule o tamanho do tilemap em tiles
        float tileSizeX = baseMapScale.x / Width;
        float tileSizeY = baseMapScale.y / Height;

        // Ajuste o offset para começar a gerar os tiles de forma alinhada ao BaseMap
        Vector3 offset = new Vector3(baseMapPosition.x - baseMapScale.x / 2, baseMapPosition.y - baseMapScale.y / 2, 0);

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                // Calcula a posição do tile levando em consideração o tamanho do tile e o offset
                float posX = j * tileSizeX + offset.x;
                float posY = i * tileSizeY + offset.y;

                float xCoord = (float)j / Width * scale + SeedX;
                float yCoord = (float)i / Height * scale + SeedY;

                groundValue = 0f;
                float amplitude = 1f;
                float frequency = 1f;
                float maxPossibleValue = 0f;

                for (int octave = 0; octave < octaves; octave++)
                {
                    float sampleX = xCoord * frequency;
                    float sampleY = yCoord * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    groundValue += perlinValue * amplitude;

                    maxPossibleValue += amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                groundValue /= maxPossibleValue;

                // Garante que o valor de groundValue esteja entre 0 e 1
                groundValue = Mathf.Clamp01((groundValue + 1f) / 2f);

                bool isGround = groundValue > groundNoiseThreshold;

                Vector3Int tilePosition = GroundMap.WorldToCell(new Vector3(posX, posY, 0));

                if (isGround)
                {
                    GroundMap.SetTile(tilePosition, GroundTile);

                    SubGroundMap.SetTile(tilePosition, SubGroundTile);
                }

                for (int x = 0; x < _typeTerrain.Length; x++)
                {
                    if (groundValue > _typeTerrain[x].Altitude)
                    {
                        GroundMap.SetTile(tilePosition, _typeTerrain[x].TerrainTile);
                    }
                }
            }
        }
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
