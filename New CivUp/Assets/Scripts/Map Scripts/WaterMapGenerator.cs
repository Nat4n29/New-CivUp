using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterMapGenerator : MonoBehaviour
{
    public GameObject BaseMap;
    public Tilemap WaterMap;
    public Tile WaterTile;
    public int Height;
    public int Width;

    public float WaterAltitude;

    public TypeWater[] _typeWater;

    GroundMapGenerator _groundMapGenerator;

    public void Start()
    {
        _groundMapGenerator = FindAnyObjectByType<GroundMapGenerator>();

        GenerateWaterMap();
    }

    public void GenerateWaterMap()
    {
        WaterMap.ClearAllTiles();
        
        // Obtenha a escala e posição do BaseMap
        Vector3 baseMapScale = BaseMap.transform.localScale;
        Vector3 baseMapPosition = BaseMap.transform.position;

        // Calcule o tamanho do tilemap em tiles
        float tileSizeX = baseMapScale.x / Width;
        float tileSizeY = baseMapScale.y / Height;

        // Ajuste o offset para começar a gerar os tiles de forma alinhada ao BaseMap
        Vector3 offset = new Vector3(baseMapPosition.x - baseMapScale.x / 2, baseMapPosition.y - baseMapScale.y / 2, 0);

        //Gera o Falloff Map
        float[,] fallOffMap = FallOffGenerator.GenerateFallOff(new Vector3Int(Width, Height, 0), _groundMapGenerator.fallOffStart, _groundMapGenerator.fallOffEnd);

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                // Calcula a posição do tile levando em consideração o tamanho do tile e o offset
                float posX = j * tileSizeX + offset.x;
                float posY = i * tileSizeY + offset.y;

                float xCoord = (float)j / Width * _groundMapGenerator.scale + _groundMapGenerator.SeedX;
                float yCoord = (float)i / Height * _groundMapGenerator.scale + _groundMapGenerator.SeedY;

                _groundMapGenerator.groundValue = 0f;
                float amplitude = 1f;
                float frequency = 1f;
                float maxPossibleValue = 0f;

                for (int octave = 0; octave < _groundMapGenerator.octaves; octave++)
                {
                    float sampleX = xCoord * frequency;
                    float sampleY = yCoord * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    _groundMapGenerator.groundValue += perlinValue * amplitude;

                    maxPossibleValue += amplitude;

                    amplitude *= _groundMapGenerator.persistence;
                    frequency *= _groundMapGenerator.lacunarity;
                }

                _groundMapGenerator.groundValue /= maxPossibleValue;

                // Garante que o valor de _groundMapGenerator.groundValue esteja entre 0 e 1
                _groundMapGenerator.groundValue = Mathf.Clamp01((_groundMapGenerator.groundValue + 1f) / 2f);

                //Aplica o Falloff
                _groundMapGenerator.groundValue *= fallOffMap[j, i];

                bool isWater = _groundMapGenerator.groundValue > WaterAltitude;

                Vector3Int tilePosition = WaterMap.WorldToCell(new Vector3(posX, posY, 0));

                if (isWater)
                {
                    WaterMap.SetTile(tilePosition, WaterTile);
                }

                for (int x = 0; x < _typeWater.Length; x++)
                {
                    if (_groundMapGenerator.groundValue > _typeWater[x].Altitude)
                    {
                        WaterMap.SetTile(tilePosition, _typeWater[x].WaterType);
                    }
                }
            }
        }
    }
}

[System.Serializable]
public struct TypeWater
{
    public string Name;
    public Tile WaterType;
    [SerializeField, Range(0f, 0.52f)]
    public float Altitude;
}