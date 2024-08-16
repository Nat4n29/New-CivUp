using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public GameObject BaseMap;
    public Tilemap WaterMap;
    public Tile WaterTile;
    public int Height;
    public int Width;

    [SerializeField] private float noiseFrequency;
    [SerializeField] private float noiseTreshold;

    public void Start()
    {
        GenerateMap();
    }

    // PerlinNoise Map Generator
    public void GenerateMap()
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

                Vector3Int tilePosition = WaterMap.WorldToCell(new Vector3(posX, posY, 0));
                WaterMap.SetTile(tilePosition, WaterTile);
            }
        }
    }
}
