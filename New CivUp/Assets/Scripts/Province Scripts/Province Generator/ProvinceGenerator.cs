using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProvinceGenerator : MonoBehaviour
{
    GameObject BaseMap;
    private int Width;
    private int Height;

    GroundMapGenerator _groundMapGenerator;
    List<Country> _countries = new List<Country>();
    List<State> _states = new List<State>();
    [SerializeField]
    public List<City> _cities = new List<City>();
    [SerializeField]
    public List<Province> _provinces = new List<Province>();
    [SerializeField]
    public List<Province> _waterProvince = new List<Province>();

    public Tilemap ProvinceMap;
    public Tilemap WaterMap;
    public Tile tileProv;

    public Tilemap CityMap;
    public Tilemap StateMap;
    public Tilemap CountryMap;

    public void Start()
    {
        _groundMapGenerator = FindAnyObjectByType<GroundMapGenerator>();
        BaseMap = _groundMapGenerator.BaseMap;
        Width = _groundMapGenerator.Width;
        Height = _groundMapGenerator.Height;

        GenerateProvinceMap();

        Debug.Log($"Provinces: {_provinces.Count}");
        Debug.Log($"Water Provinces: {_waterProvince.Count}");
    }

    public void GenerateProvinceMap()
    {
        ProvinceMap.ClearAllTiles();
        WaterMap.ClearAllTiles();
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

                bool isProvince = _groundMapGenerator.groundValue > _groundMapGenerator.groundNoiseThreshold;

                Vector3Int tilePosition = ProvinceMap.WorldToCell(new Vector3(posX, posY, 0));

                //Province
                if (isProvince)
                {
                    ProvinceMap.SetTile(tilePosition, tileProv);
                    int provinceId = _provinces.Count;
                    _provinces.Add(new Province(provinceId + 1, tilePosition));
                    // Remove o tile de água, se existir
                    WaterMap.SetTile(tilePosition, null);
                    _waterProvince.RemoveAll(p => p.ProvincePosition == tilePosition);
                }
                else
                {
                    // Água
                    if (!ProvinceMap.HasTile(tilePosition))
                    {
                        WaterMap.SetTile(tilePosition, tileProv);
                        int waterProvinceId = _waterProvince.Count;
                        _waterProvince.Add(new Province(waterProvinceId + 1, tilePosition));
                    }
                }
            }
        }
    }

    public void AddCity(City city)
    {
        _cities.Add(city);
    }
}
