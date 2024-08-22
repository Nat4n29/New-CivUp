using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class State
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Population { get; set; }
    public List<City> Cities { get; set; } = new List<City>();
    public List<Province> Provinces { get; set; } = new List<Province>();

    public State(int id, string name, List<City> cities, List<Province> provinces, int population)
    {
        Id = id;
        Name = name;
        Cities = cities;
        Provinces = provinces;
        Population = population;
    }
}
