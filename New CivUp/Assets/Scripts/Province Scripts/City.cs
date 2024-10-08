using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class City
{
    public int Id;
    public string Name { get; set; }
    public Vector3Int Position;
    public int Population { get; set; }
    public List<Construction> Constructions { get; set; } = new List<Construction>();
    public List<Province> Provinces = new List<Province>();
    public State State { get; set; }

    public City(int id, string name, Vector3Int position, int population, Province province, State state, Construction construction)
    {
        Id = id;
        Name = name;
        Position = position;
        Population = population;
        Provinces.Add(province);
        State = state;
        Constructions.Add(construction);
    }
    public City(int id, string name, Vector3Int position, int population, Province province, State state)
    {
        Id = id;
        Name = name;
        Position = position;
        Population = population;
        Provinces.Add(province);
        State = state;
    }
    public City(int id, string name, Vector3Int position, int population, Province province)
    {
        Id = id;
        Name = name;
        Position = position;
        Population = population;
        Provinces.Add(province);
    }
}
