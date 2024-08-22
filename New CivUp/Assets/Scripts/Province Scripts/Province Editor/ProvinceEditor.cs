using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class ProvinceEditor : MonoBehaviour
{
    ProvinceGenerator provinceGenerator;
    Tilemap ProvinceMap;
    Tilemap CityMap;
    public Tile cityTile;

    public GameObject ProvinceInfo;

    public Camera mainCamera;

    Vector3Int tilePos;
    public Tile clickTile;
    private Tile defaultTile;
    private Vector3Int defaultTilePosition;
    void Start()
    {
        provinceGenerator = FindAnyObjectByType<ProvinceGenerator>();
        //ProvinceInfo = GameObject.Find("Province Info").GetComponent<GameObject>();
        ProvinceMap = provinceGenerator.ProvinceMap;
        CityMap = provinceGenerator.CityMap;
    }

    void Update()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        tilePos = ProvinceMap.WorldToCell(mouseWorldPos);

        ProvincePanelInfo();

        if (IsPointerOverUIElement() == false)
        {
            OnClickMouse(tilePos);
        }
    }

    public void ProvincePanelInfo()
    {
        Text text = ProvinceInfo.transform.Find("Id text").GetComponent<Text>();
        bool isActived = ProvinceInfo.activeSelf;

        if (isActived)
        {
            text.text = $"Province ID: {provinceGenerator._provinces.FirstOrDefault(p => p.ProvincePosition == defaultTilePosition).Id}";
        }

    }

    // Função que verifica se o cursor está sobre algum elemento UI
    private bool IsPointerOverUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void OnClickMouse(Vector3Int tilePos)
    {
        if (ProvinceMap.HasTile(tilePos))
        {
            if (Input.GetMouseButtonDown(0))
            {
                ProvinceInfo.SetActive(true);

                defaultTile = (Tile)ProvinceMap.GetTile(tilePos);
                ProvinceMap.SetTile(tilePos, clickTile);

                if (defaultTilePosition != tilePos)
                {
                    if (ProvinceMap.HasTile(defaultTilePosition))
                    {
                        ProvinceMap.SetTile(defaultTilePosition, defaultTile);
                    }
                }

                defaultTilePosition = tilePos;

                var matchingProvince = provinceGenerator._provinces.FirstOrDefault(p => p.ProvincePosition == tilePos);

                if (matchingProvince != null)
                {
                    Debug.Log($"ID = {matchingProvince.Id}");
                }
                else
                {
                    Debug.Log("Nothing tile has found");
                }
            }

        }
        else
        {
            ProvinceInfo.SetActive(false);
        }
    }

    public void CreateCity()
    {
        
        
        CityMap.SetTile(defaultTilePosition, cityTile);

        var _province = provinceGenerator._provinces.FirstOrDefault(p => p.ProvincePosition == defaultTilePosition);
    
        provinceGenerator._cities.Add(new City(provinceGenerator._cities.Count + 1, "Nome Cidade", 50, _province));

        //Debug.Log($"City Id: {provinceGenerator._cities.FirstOrDefault(c => c.Position == defaultTilePosition).Id}");
    }
}
