using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FindButton : MonoBehaviour
{
    public static void FindButtonClick(GameObject findButton, DataProvince dataProvince, GameObject ProvinceInfo,Tilemap ProvinceMap,Tilemap ClickMap,Vector3Int defaultTilePosition,Tile clickTile,Tile defaultTile)
    {
        TMP_InputField findText = findButton.transform.Find("Find_Panel").Find("InputID").GetComponent<TMP_InputField>();
        int findTextId = int.Parse(findText.text);

        Debug.Log($"ID found: {findTextId}");

        bool isGroundProvince = dataProvince._provinces.Any(p => p.Id == findTextId);
        bool isWaterProvince = dataProvince._waterProvince.Any(w => w.Id == findTextId);

        if (isGroundProvince)
        {
            Vector3Int provincePos = dataProvince._provinces.FirstOrDefault(p => p.Id == findTextId).ProvincePosition;

            ProvinceInfo.SetActive(true);

            defaultTile = (Tile)ClickMap.GetTile(provincePos);
            ClickMap.SetTile(provincePos, clickTile);

            if (defaultTilePosition != provincePos)
            {
                if (ClickMap.HasTile(defaultTilePosition))
                {
                    ClickMap.SetTile(defaultTilePosition, defaultTile);
                }
            }

            defaultTilePosition = provincePos;
        }

        else if (isWaterProvince)
        {
            Vector3Int waterProvPos = dataProvince._waterProvince.FirstOrDefault(w => w.Id == findTextId).ProvincePosition;

            ProvinceInfo.SetActive(true);

            defaultTile = (Tile)ClickMap.GetTile(waterProvPos);
            ClickMap.SetTile(waterProvPos, clickTile);

            if (defaultTilePosition != waterProvPos)
            {
                if (ClickMap.HasTile(defaultTilePosition))
                {
                    ClickMap.SetTile(defaultTilePosition, defaultTile);
                }
            }

            defaultTilePosition = waterProvPos;
        }

        else
        {
            findText.text = "ID not found";
        }
    }
}
