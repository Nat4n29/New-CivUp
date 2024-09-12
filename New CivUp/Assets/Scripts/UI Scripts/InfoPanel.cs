using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    public static void ProvincePanelInfo(GeneratorManeger generatorManeger, DataProvince dataProvince, GameObject ProvinceInfo, Vector3Int defaultTilePosition)
    {
        // Verifique se ProvinceInfo não é nulo
        if (ProvinceInfo == null)
        {
            Debug.LogError("ProvinceInfo não foi atribuído!");
            return;
        }

        // Tente encontrar os textos. Se não encontrar, log de erro.
        TextMeshProUGUI IdText = ProvinceInfo.transform.Find("Id text")?.GetComponent<TextMeshProUGUI>();
        if (IdText == null)
        {
            Debug.LogError("Não foi encontrado o componente 'Id text' no ProvinceInfo!");
            return;
        }

        TextMeshProUGUI CityIdText = ProvinceInfo.transform.Find("City_ID text")?.GetComponent<TextMeshProUGUI>();
        if (CityIdText == null)
        {
            Debug.LogError("Não foi encontrado o componente 'City_ID text' no ProvinceInfo!");
            return;
        }

        bool isActived = ProvinceInfo.activeSelf;

        if (isActived)
        {            
            // Verificação de `null` para o `Province`
            if(generatorManeger.ProvinceMap.HasTile(defaultTilePosition))
            {
                var province = dataProvince._provinces.FirstOrDefault(p => p.ProvincePosition == defaultTilePosition);
                if (province != null)
                {
                    IdText.text = $"Province ID: {province.Id}";
                }
                else
                {
                    IdText.text = "Province não encontrada.";
                }
            }

            // Verificação de `null` para o `WaterProvince`
            else if (generatorManeger.WaterMap.HasTile(defaultTilePosition))
            {
                var waterProvince = dataProvince._waterProvince.FirstOrDefault(p => p.ProvincePosition == defaultTilePosition);
                if (waterProvince != null)
                {
                    IdText.text = $"Water ID: {waterProvince.Id}";
                }
                else
                {
                    IdText.text = "Water province não encontrada.";
                }
            }

            // Verificação de `null` para o `City`
            if (generatorManeger.CityMap.HasTile(defaultTilePosition))
            {
                var city = dataProvince._cities.FirstOrDefault(c => c.Position == defaultTilePosition);
                if (city != null)
                {
                    CityIdText.text = $"City ID: {city.Id}";
                }
                else
                {
                    CityIdText.text = "City não encontrada.";
                }
            }
            else
            {
                CityIdText.text = "Not has city";
            }
        }
    }
}
