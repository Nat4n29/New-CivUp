using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProvinceGenerator : MonoBehaviour
{
    public static void GenerateProvinceMap(GameObject BaseMap, Tilemap ProvinceMap, Tilemap ProvinceWaterMap, Tilemap CityMap, Tilemap StateMap, Tilemap CountryMap, int Height, int Width, float fallOffStart, float fallOffEnd, int scale, float SeedX, float SeedY, int octaves, float persistence, float lacunarity, float groundNoiseThreshold, Tile tileProv, DataProvince dataProvince)
    {
        ProvinceMap.ClearAllTiles();
        ProvinceWaterMap.ClearAllTiles();
        CityMap.ClearAllTiles();
        StateMap.ClearAllTiles();
        CountryMap.ClearAllTiles();

        // Obtenha a escala e posição do BaseMap
        Vector3 baseMapScale = BaseMap.transform.localScale;
        Vector3 baseMapPosition = BaseMap.transform.position;

        // Calcule o tamanho do tilemap em tiles
        float tileSizeX = baseMapScale.x / Width;
        float tileSizeY = baseMapScale.y / Height;

        // Ajuste o offset para começar a gerar os tiles de forma alinhada ao BaseMap
        Vector3 offset = new Vector3(baseMapPosition.x - baseMapScale.x / 2, baseMapPosition.y - baseMapScale.y / 2, 0);

        float[,] fallOffMap = FallOffGenerator.GenerateFallOff(new Vector3Int(Width, Height, 0), fallOffStart, fallOffEnd);

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                // Calcula a posição do tile levando em consideração o tamanho do tile e o offset
                float posX = j * tileSizeX + offset.x;
                float posY = i * tileSizeY + offset.y;

                float xCoord = (float)j / Width * scale + SeedX;
                float yCoord = (float)i / Height * scale + SeedY;

                float groundValue = 0f;
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

                //Aplica o Falloff
                groundValue *= fallOffMap[j, i];

                bool isProvince = groundValue > groundNoiseThreshold;

                Vector3Int tilePosition = ProvinceMap.WorldToCell(new Vector3(posX, posY, 0));

                //Province
                if (isProvince)
                {
                    ProvinceMap.SetTile(tilePosition, tileProv);
                    int provinceId = dataProvince._provinces.Count;
                    dataProvince._provinces.Add(new Province(provinceId + 1, tilePosition));
                    // Remove o tile de água, se existir
                    ProvinceWaterMap.SetTile(tilePosition, null);
                    dataProvince._waterProvince.RemoveAll(p => p.ProvincePosition == tilePosition);
                }
                else
                {
                    // Água
                    if (!ProvinceMap.HasTile(tilePosition))
                    {
                        ProvinceWaterMap.SetTile(tilePosition, tileProv);
                        int waterProvinceId = dataProvince._waterProvince.Count;
                        dataProvince._waterProvince.Add(new Province(waterProvinceId + 1, tilePosition));
                    }
                }
            }
        }

        Debug.Log($"Provinces: {dataProvince._provinces.Count}");
        Debug.Log($"Water Provinces: {dataProvince._waterProvince.Count}");
    }
}
