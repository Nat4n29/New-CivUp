using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundMapGenerator : MonoBehaviour
{
    public GameObject BaseMap;
    public Tilemap GroundMap;
    public Tile GroundTile;
    public int Height;
    public int Width;
    public float noiseFrequency = 100f;
    public float noiseThreshold = 0.5f;

    public void Start()
    {
        GenerateGroundMap();
    }

    // PerlinNoise Map Generator
    public void GenerateGroundMap()
    {
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

                float xCoord = (float) j / Width;
                float yCoord = (float) i / Height;

                float groundValue = Mathf.PerlinNoise(xCoord / noiseFrequency, yCoord / noiseFrequency);

                bool isGround = groundValue < noiseThreshold;

                if (isGround) continue;

                Vector3Int tilePosition = GroundMap.WorldToCell(new Vector3(posX, posY, 0));
                GroundMap.SetTile(tilePosition, GroundTile);
            }
        }
    }
}
