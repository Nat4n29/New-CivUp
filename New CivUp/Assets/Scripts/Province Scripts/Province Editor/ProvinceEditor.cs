using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using TMPro;

public class ProvinceEditor : MonoBehaviour
{
    ProvinceGenerator provinceGenerator;
    Tilemap ProvinceMap;
    Tilemap WaterMap;
    Tilemap CityMap;
    public Tile cityTile;

    public GameObject ProvinceInfo;
    public GameObject FindButton;

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
        WaterMap = provinceGenerator.WaterMap;
    }

    public void Update()
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
        TextMeshProUGUI IdText = ProvinceInfo.transform.Find("Id text").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI CityIdText = ProvinceInfo.transform.Find("City_ID text").GetComponent<TextMeshProUGUI>();
        bool isActived = ProvinceInfo.activeSelf;

        if (isActived)
        {
            if (WaterMap.HasTile(defaultTilePosition))
            {
                IdText.text = $"Water ID: {provinceGenerator._waterProvince.FirstOrDefault(p => p.ProvincePosition == defaultTilePosition).Id}";
            }
            else if (ProvinceMap.HasTile(defaultTilePosition))
            {
                IdText.text = $"Province ID: {provinceGenerator._provinces.FirstOrDefault(p => p.ProvincePosition == defaultTilePosition).Id}";
            }
            else
            {
                IdText.text = "Not has waterprovince";
            }

            if (CityMap.HasTile(defaultTilePosition))
            {
                CityIdText.text = $"City ID: {provinceGenerator._cities.FirstOrDefault(c => c.Position == defaultTilePosition).Id}";
            }
            else
            {
                CityIdText.text = "Not has city";
            }
        }

    }

    // Função que verifica se o cursor está sobre algum elemento UI
    private bool IsPointerOverUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void OnClickMouse(Vector3Int tilePos)
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
            var matchingWater = provinceGenerator._waterProvince.FirstOrDefault(p => p.ProvincePosition == tilePos);

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
    }

    public void CreateCity()
    {
        if (!WaterMap.HasTile(defaultTilePosition))
        {
            CityMap.SetTile(defaultTilePosition, cityTile);

            var _province = provinceGenerator._provinces.FirstOrDefault(p => p.ProvincePosition == defaultTilePosition);

            int cityId = provinceGenerator._cities.Count;

            City newCity = new City(cityId + 1, "Nome Cidade", defaultTilePosition, 50, _province);

            provinceGenerator.AddCity(newCity);

            Debug.Log($"City Id: {newCity.Id}");
        }
    }

    //Find Button System_______________________________________________________________________________________________________
    public void FindButtonClick()
    {
        TMP_InputField findText = FindButton.transform.Find("Find_Panel").Find("InputID").GetComponent<TMP_InputField>();
        int findTextId = int.Parse(findText.text);

        Debug.Log($"ID found: {findTextId}");

        bool isGroundProvince = provinceGenerator._provinces.Any(p => p.Id == findTextId);
        bool isWaterProvince = provinceGenerator._waterProvince.Any(w => w.Id == findTextId);

        if (isGroundProvince)
        {
            Vector3Int provincePos = provinceGenerator._provinces.FirstOrDefault(p => p.Id == findTextId).ProvincePosition;

            ProvinceInfo.SetActive(true);

            defaultTile = (Tile)ProvinceMap.GetTile(provincePos);
            ProvinceMap.SetTile(provincePos, clickTile);

            if (defaultTilePosition != provincePos)
            {
                if (ProvinceMap.HasTile(defaultTilePosition))
                {
                    ProvinceMap.SetTile(defaultTilePosition, defaultTile);
                }
            }

            defaultTilePosition = provincePos;
        }

        else if (isWaterProvince)
        {
            Vector3Int waterProvPos = provinceGenerator._waterProvince.FirstOrDefault(w => w.Id == findTextId).ProvincePosition;

            ProvinceInfo.SetActive(true);

            defaultTile = (Tile)ProvinceMap.GetTile(waterProvPos);
            ProvinceMap.SetTile(waterProvPos, clickTile);

            if (defaultTilePosition != waterProvPos)
            {
                if (ProvinceMap.HasTile(defaultTilePosition))
                {
                    ProvinceMap.SetTile(defaultTilePosition, defaultTile);
                }
            }

            defaultTilePosition = waterProvPos;
        }

        else
        {
            findText.text = "ID not found";
        }
    }

    public void ToggleFindPanel()
    {
        Transform FindPanel = FindButton.transform.Find("Find_Panel");

        FindPanel.gameObject.SetActive(!FindPanel.gameObject.activeSelf);
    }
}
