using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ClickScript : MonoBehaviour
{
    public static Vector3Int OnClickMouse(Vector3Int tilePos, GameObject ProvinceInfo, DataProvince dataProvince, Tilemap ProvinceMap, Tilemap ClickMap, Tile clickTile, Tile defaultTile, Vector3Int defaultTilePosition)
    {
        if (Input.GetMouseButtonDown(0))
        {
            ProvinceInfo.SetActive(true);

            defaultTile = (Tile)ClickMap.GetTile(tilePos);
            ClickMap.SetTile(tilePos, clickTile);

            if (defaultTilePosition != tilePos)
            {
                if (ClickMap.HasTile(defaultTilePosition))
                {
                    ClickMap.SetTile(defaultTilePosition, defaultTile);
                }
            }

            defaultTilePosition = tilePos;

            var matchingProvince = dataProvince._provinces.FirstOrDefault(p => p.ProvincePosition == tilePos);
            var matchingWater = dataProvince._waterProvince.FirstOrDefault(p => p.ProvincePosition == tilePos);

            if (matchingProvince != null)
            {
                Debug.Log($"ID = {matchingProvince.Id} {matchingProvince.ProvincePosition}");
            }
            else if (matchingWater != null)
            {
                Debug.Log($"Water ID = {matchingWater.Id} {matchingWater.ProvincePosition}");
            }
            else
            {
                Debug.Log("Nothing tile has found");
            }
        }

        return defaultTilePosition;
    }

    ProvinceEditor provinceEditor;
    void Start()
    {
        provinceEditor = FindAnyObjectByType<ProvinceEditor>();
    }

    public void CloseClick()
    {
        provinceEditor.ClickMap.ClearAllTiles();
    }
}
