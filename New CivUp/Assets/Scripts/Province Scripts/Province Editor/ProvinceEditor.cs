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
using System;

public class ProvinceEditor : MonoBehaviour
{
    GeneratorManeger generatorManeger;
    DataProvince dataProvince;

    public GameObject ProvinceInfo;
    public GameObject findButton;

    [NonSerialized]
    public Tilemap ProvinceMap;
    public Tilemap ClickMap;

    public Camera mainCamera;

    Vector3Int tilePos;
    public Tile clickTile;
    [NonSerialized]
    public Tile defaultTile;
    [NonSerialized]
    public Vector3Int defaultTilePosition;

    void Start()
    {
        generatorManeger = FindAnyObjectByType<GeneratorManeger>();
        dataProvince = FindAnyObjectByType<DataProvince>();

        ProvinceMap = generatorManeger.ProvinceMap;
    }

    public void Update()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        tilePos = ProvinceMap.WorldToCell(mouseWorldPos);

        if (IsPointerOverUIElement() == false)
        {
            defaultTilePosition = ClickScript.OnClickMouse(tilePos, ProvinceInfo, dataProvince, ProvinceMap, ClickMap, clickTile, defaultTile, defaultTilePosition);
            InfoPanel.ProvincePanelInfo(generatorManeger, dataProvince, ProvinceInfo, defaultTilePosition);
        }
    }

    // Função que verifica se o cursor está sobre algum elemento UI
    private bool IsPointerOverUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    //Find System_________________________________________________________________________________________________________
    public void OnFindButtonClick()
    {
        FindButton.FindButtonClick(findButton,dataProvince, ProvinceInfo,ProvinceMap,ClickMap,defaultTilePosition,clickTile,defaultTile);
    }
    public void ToggleFindPanel()
    {
        Transform FindPanel = findButton.transform.Find("Find_Panel");

        FindPanel.gameObject.SetActive(!FindPanel.gameObject.activeSelf);
    }
}
