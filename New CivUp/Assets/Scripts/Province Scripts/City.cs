using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Vector3Int Position { get; set; }
    public int Population { get; set; }
    public List<Construction> Constructions { get; set; } = new List<Construction>();
    public List<Province> Provinces { get; set; } = new List<Province>();
    public State State { get; set; }

    public City(int id, string name, int population, Province province, State state, Construction construction)
    {
        Id = id;
        Name = name;
        Population = population;
        Provinces.Add(province);
        State = state;
        Constructions.Add(construction);
    }
    public City(int id, string name, int population, Province province, State state)
    {
        Id = id;
        Name = name;
        Population = population;
        Provinces.Add(province);
        State = state;
    }
    public City(int id, string name, int population, Province province)
    {
        Id = id;
        Name = name;
        Population = population;
        Provinces.Add(province);
    }
}
