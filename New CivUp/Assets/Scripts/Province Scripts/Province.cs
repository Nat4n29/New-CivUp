using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Province
{
    public int Id { get; set; }
    public Vector3Int ProvincePosition { get; set; }
    public District DistrictType { get; set; }
    public int Population { get; set; }

    public Province(int id, Vector3Int provincePosition, District districtType, int population)
    {
        Id = id;
        ProvincePosition = provincePosition;
        DistrictType = districtType;
        Population = population;
    }
    public Province(int id, Vector3Int provincePosition)
    {
        Id = id;
        ProvincePosition = provincePosition;
    }
}
