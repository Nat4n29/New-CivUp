using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Population { get; set; }
    public City[] Cities { get; set; }
    public Province[] Provinces { get; set; }

    public State(int id, string name, City[] cities, Province[] provinces, int population)
    {
        Id = id;
        Name = name;
        Cities = cities;
        Provinces = provinces;
        Population = population;
    }
}
