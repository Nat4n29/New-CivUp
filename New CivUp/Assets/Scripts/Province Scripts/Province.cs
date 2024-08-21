using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Province : MonoBehaviour
{
    public int Id { get; set; }
    public District DistrictType { get; set; }
    public int Population { get; set; }

    public Province(int id, District districtType, int population)
    {
        Id = id;
        DistrictType = districtType;
        Population = population;
    }
}
