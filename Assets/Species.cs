using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Species
{
    List<AIControler> _individuals;

    public Species()
    {
        _individuals = new List<AIControler>();
    }

    public void AddIndividual(AIControler individual)
    {
        _individuals.Add(individual);
    }
}
