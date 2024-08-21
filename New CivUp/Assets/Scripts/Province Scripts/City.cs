using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Population { get; set; }
    public Construction[] Constructions { get; set; }
    public Province[] Provinces { get; set; }
    public State State { get; set; }

    public City(int id, string name, int population, Construction[] constructions, Province[] provinces, State state)
    {
        Id = id;
        Name = name;
        Population = population;
        Constructions = constructions;
        Provinces = provinces;
        State = state;
    }
}
