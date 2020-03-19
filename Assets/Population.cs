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

