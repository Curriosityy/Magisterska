using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population
{
    private bool _isInitialized;
    private List<Species> _species;
    public List<Species> Species { get => _species;}
    private static Population _instance;
    private Population()
    {
        //_isInitialized = false;
        _species = new List<Species>();
        _species.Add(new Species());
    }
    public bool assignToSpecie(NeuralNetwork candidate)
    {
        foreach(var specie in Species)
        {
            if (specie.CompareWithFirst(candidate))
            {
               /* specie.AddIndividual(candidate);*/
                return true;
            }
            
        }
        CreateSpecie(candidate);
        return false;
    }
    private void CreateSpecie(NeuralNetwork candidate)
    {
        _species.Add(new Species(/*candidate*/));
    }
    public static Population Instance {
        get
        {
            if(_instance==null)
            {
                _instance = new Population();
            }
            return _instance;
        }

    }



}

