using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterMapGenerator : MonoBehaviour
{
    public static void GenerateWaterMap(GameObject baseMap, Tilemap waterMap, int height, int width, int scale, int octaves, float persistence, float lacunarity, float seedX, float seedY, float fallOffStart, float fallOffEnd, float waterAltitude, Tile waterTile, TypeWater[] typeWater)
    {
        waterMap.ClearAllTiles();
        
        // Obtenha a escala e posição do BaseMap
        Vector3 baseMapScale = baseMap.transform.localScale;
        Vector3 baseMapPosition = baseMap.transform.position;

        // Calcule o tamanho do tilemap em tiles
        float tileSizeX = baseMapScale.x / width;
        float tileSizeY = baseMapScale.y / height;

        // Ajuste o offset para começar a gerar os tiles de forma alinhada ao BaseMap
        Vector3 offset = new Vector3(baseMapPosition.x - baseMapScale.x / 2, baseMapPosition.y - baseMapScale.y / 2, 0);

        //Gera o Falloff Map
        float[,] fallOffMap = FallOffGenerator.GenerateFallOff(new Vector3Int(width, height, 0), fallOffStart, fallOffEnd);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                // Calcula a posição do tile levando em consideração o tamanho do tile e o offset
                float posX = j * tileSizeX + offset.x;
                float posY = i * tileSizeY + offset.y;

                float xCoord = (float)j / width * scale + seedX;
                float yCoord = (float)i / height * scale + seedY;

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

                // Garante que o valor de _groundMapGenerator.groundValue esteja entre 0 e 1
                groundValue = Mathf.Clamp01((groundValue + 1f) / 2f);

                //Aplica o Falloff
                groundValue *= fallOffMap[j, i];

                bool isWater = groundValue > waterAltitude;

                Vector3Int tilePosition = waterMap.WorldToCell(new Vector3(posX, posY, 0));

                if (isWater)
                {
                    waterMap.SetTile(tilePosition, waterTile);
                }

                for (int x = 0; x < typeWater.Length; x++)
                {
                    if (groundValue > typeWater[x].Altitude)
                    {
                        waterMap.SetTile(tilePosition, typeWater[x].WaterType);
                    }
                }
            }
        }
    }
}