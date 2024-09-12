//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundMapGenerator : MonoBehaviour
{
    public static void GenerateGroundMap(GameObject baseMap, Tilemap groundMap, Tilemap subGroundMap, int height, int width, int scale, int octaves, float persistence, float lacunarity, float seedX, float seedY, float fallOffStart, float fallOffEnd, Tile groundTile, Tile subGroundTile, float groundNoiseThreshold, TypeTerrain[] typeTerrain)
    {
        // Limpa a Tilemap antes de gerar um novo mapa
        groundMap.ClearAllTiles();
        subGroundMap.ClearAllTiles();

        // Obtenha a escala e posição do baseMap
        Vector3 baseMapScale = baseMap.transform.localScale;
        Vector3 baseMapPosition = baseMap.transform.position;

        // Calcule o tamanho do tilemap em tiles
        float tileSizeX = baseMapScale.x / width;
        float tileSizeY = baseMapScale.y / height;

        // Ajuste o offset para começar a gerar os tiles de forma alinhada ao baseMap
        Vector3 offset = new Vector3(baseMapPosition.x - baseMapScale.x / 2, baseMapPosition.y - baseMapScale.y / 2, 0);

        //Gera o Falloff Map
        float[,] fallOffMap = FallOffGenerator.GenerateFallOff(new Vector3Int(width, height, 0), fallOffStart, fallOffEnd);

        // Variáveis para armazenar as posições dos tiles mais altos
        List<Vector3Int> highestPoints = new List<Vector3Int>();

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

                // Garante que o valor de groundValue esteja entre 0 e 1
                groundValue = Mathf.Clamp01((groundValue + 1f) / 2f);

                //Aplica o Falloff
                groundValue *= fallOffMap[j, i];

                bool isGround = groundValue > groundNoiseThreshold;

                Vector3Int tilePosition = groundMap.WorldToCell(new Vector3(posX, posY, 0));

                if (isGround)
                {
                    groundMap.SetTile(tilePosition, groundTile);

                    subGroundMap.SetTile(tilePosition, subGroundTile);
                }

                for (int x = 0; x < typeTerrain.Length; x++)
                {
                    if (groundValue > typeTerrain[x].Altitude)
                    {
                        groundMap.SetTile(tilePosition, typeTerrain[x].TerrainTile);
                    }
                }

                // Identifica os pontos mais altos
                if (groundValue > 0.9f)
                {
                    highestPoints.Add(tilePosition);
                }
            }
        }

        // Geração dos rios
        foreach (Vector3Int startPoint in highestPoints)
        {
            //GenerateRiver(startPoint);
        }
    }

    /*static void GenerateRiver(Vector3Int startPoint)
    {
        Vector3Int currentPoint = startPoint;
        for (int i = 0; i < RiverLength; i++)
        {
            RiverMap.SetTile(currentPoint, RiverTile);

            // Encontra o vizinho mais baixo em uma cruz (+)
            Vector3Int nextPoint = currentPoint;
            float lowestValue = groundValue;

            List<Vector3Int> neighbors = new List<Vector3Int>
            {
                new Vector3Int(currentPoint.x + 1, currentPoint.y, currentPoint.z),
                new Vector3Int(currentPoint.x - 1, currentPoint.y, currentPoint.z),
                new Vector3Int(currentPoint.x, currentPoint.y + 1, currentPoint.z),
                new Vector3Int(currentPoint.x, currentPoint.y - 1, currentPoint.z),
                new Vector3Int(currentPoint.x + 1, currentPoint.y - 1, currentPoint.z),
                new Vector3Int(currentPoint.x - 1, currentPoint.y + 1, currentPoint.z)
            };

            foreach (Vector3Int neighbor in neighbors)
            {
                float neighborValue = GroundMap.GetTile(neighbor) == null ? 0 : groundValue;

                if (neighborValue < lowestValue)
                {
                    lowestValue = neighborValue;
                    nextPoint = neighbor;
                }
            }

            currentPoint = nextPoint;

            // Se o próximo ponto é um tile de água, pare de gerar o rio
            if (GroundMap.GetTile(currentPoint) == null)
            {
                break;
            }
        }
    }*/
}
