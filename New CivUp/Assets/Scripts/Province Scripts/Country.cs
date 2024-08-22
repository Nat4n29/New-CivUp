using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Country
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<State> States { get; set; } = new List<State>();
    public List<Province> Provinces { get; set; } = new List<Province>();
    public int Population { get; set; }

    public Country(int id, string name, List<State> states, List<Province> provinces, int population)
    {
        Id = id;
        Name = name;
        States = states;
        Provinces = provinces;
        Population = population;
    }
}
