using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Country : MonoBehaviour
{
    public int Id { get; set; }
    public string Name { get; set; }
    public State[] States { get; set; }
    public Province[] Provinces { get; set; }
    public int Population { get; set; }

    public Country(int id, string name, State[] states, Province[] provinces, int population)
    {
        Id = id;
        Name = name;
        States = states;
        Provinces = provinces;
        Population = population;
    }
}
