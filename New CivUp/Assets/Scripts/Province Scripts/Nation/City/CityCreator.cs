using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CityCreator : MonoBehaviour
{
    ProvinceEditor provinceEditor;
    GeneratorManeger generatorManeger;
    DataProvince dataProvince;
    public Tile cityTile;

    void Start()
    {
        provinceEditor = FindAnyObjectByType<ProvinceEditor>();
        generatorManeger = FindAnyObjectByType<GeneratorManeger>();
        dataProvince = FindAnyObjectByType<DataProvince>();
    }

    public void CreateCity()
    {
        if (!generatorManeger.WaterMap.HasTile(provinceEditor.defaultTilePosition))
        {
            generatorManeger.CityMap.SetTile(provinceEditor.defaultTilePosition, cityTile);

            var _province = dataProvince._provinces.FirstOrDefault(p => p.ProvincePosition == provinceEditor.defaultTilePosition);

            int cityId = dataProvince._cities.Count;

            City newCity = new City(cityId + 1, "Nome Cidade", provinceEditor.defaultTilePosition, 50, _province);

            dataProvince._cities.Add(newCity);

            Debug.Log($"City Id: {newCity.Id}");
        }
    }
}
