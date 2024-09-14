using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundMapGenerator : MonoBehaviour
{
    public static void GenerateGroundMap(GameObject baseMap, Tilemap groundMap, Tilemap subGroundMap, int height, int width, int scale, int octaves, float persistence, float lacunarity, float seedX, float seedY, float fallOffStart, float fallOffEnd, Tile groundTile, Tile subGroundTile, float groundNoiseThreshold, TypeTerrain[] typeTerrain, float RiverHeight, int RiverLength, Tilemap RiverMap, RuleTile RiverTile)
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
        List<Tuple<Vector3Int, float>> altituePoints = new List<Tuple<Vector3Int, float>>();
        List<Tuple<Vector3Int, float>> highestPoints = new List<Tuple<Vector3Int, float>>();
        List<Tuple<Vector3Int, float>> waterPoints = new List<Tuple<Vector3Int, float>>();

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

                    altituePoints.Add(new Tuple<Vector3Int, float>(tilePosition, groundValue));
                }

                for (int x = 0; x < typeTerrain.Length; x++)
                {
                    if (groundValue > typeTerrain[x].Altitude)
                    {
                        groundMap.SetTile(tilePosition, typeTerrain[x].TerrainTile);

                        altituePoints.Add(new Tuple<Vector3Int, float>(tilePosition, groundValue));
                    }
                }

                // Identifica os pontos mais altos
                if (groundValue >= RiverHeight)
                {
                    highestPoints.Add(altituePoints.FirstOrDefault(a => a.Item2 == groundValue));
                }

                if (groundValue <= groundNoiseThreshold)
                {
                    waterPoints.Add(new Tuple<Vector3Int, float>(tilePosition, 0f));
                }
            }
        }

        //Elimina metade dos pontos altos
        if (highestPoints.Count > 2)
        {
            int halfHighest = (int)(highestPoints.Count - (highestPoints.Count * ((float) 5/100)));

            // Remover metade dos elementos de forma aleatória
            for (int i = 0; i < halfHighest; i++)
            {
                // Obtenha um índice aleatório dentro do intervalo atual da lista
                int randomIndex = UnityEngine.Random.Range(0, highestPoints.Count);
                
                // Remove o item no índice aleatório gerado
                highestPoints.RemoveAt(randomIndex);
            }
        }
        Debug.Log(highestPoints.Count);

        // Geração dos rios
        foreach (var startPoint in highestPoints)
        {
            Vector3Int position = startPoint.Item1;
            float altitude = startPoint.Item2;

            GenerateRiver(position, altitude, altituePoints, RiverLength, RiverMap, RiverTile, groundMap, waterPoints);
        }
    }

    static void GenerateRiver(Vector3Int startPoint, float altitude, List<Tuple<Vector3Int, float>> altituePoints, int RiverLength, Tilemap RiverMap, RuleTile RiverTile, Tilemap GroundMap, List<Tuple<Vector3Int, float>> waterPoints)
    {
        Vector3Int currentPoint = startPoint;

        for (int i = 0; i < RiverLength; i++)
        {
            RiverMap.SetTile(currentPoint, RiverTile);
            
            // Encontra o vizinho mais baixo
            Vector3Int nextPoint = currentPoint;
            float lowestValue = altitude;

            List<Vector3Int> neighbors = new List<Vector3Int>
            {
                new Vector3Int(currentPoint.x + 1, currentPoint.y, currentPoint.z),
                new Vector3Int(currentPoint.x - 1, currentPoint.y, currentPoint.z),
                new Vector3Int(currentPoint.x, currentPoint.y + 1, currentPoint.z),
                new Vector3Int(currentPoint.x, currentPoint.y - 1, currentPoint.z),
                new Vector3Int(currentPoint.x - 1, currentPoint.y + 1, currentPoint.z),
                new Vector3Int(currentPoint.x - 1, currentPoint.y - 1, currentPoint.z)
            };

            foreach (Vector3Int neighbor in neighbors)
            {
                var neighborTuple = altituePoints.FirstOrDefault(a => a.Item1 == neighbor);
                var neighborWater = waterPoints.FirstOrDefault(a => a.Item1 == neighbor);

                if (neighborTuple != null)
                {
                    float neighborValue = neighborTuple.Item2;
                    if (neighborValue < lowestValue)
                    {
                        lowestValue = neighborValue;
                        nextPoint = neighbor;
                    }
                }

                else if (neighborWater != null)
                {
                    nextPoint = neighbor;
                    break;
                }
            }

            if (nextPoint != null)
            {
                currentPoint = nextPoint;
            }


            // Se o próximo ponto é um tile de água, pare de gerar o rio
            if (GroundMap.GetTile(currentPoint) == null)
            {
                RiverMap.SetTile(currentPoint, RiverTile);
                break;
            }
            
        }
    }
}
