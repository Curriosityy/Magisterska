using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population
{
    private bool _isInitialized;
    private List<Species> _species;
    private List<NeuralNetwork> _generation;
    private List<NeuralNetwork> _newGeneration;
    private List<Bot> _bots;
    public List<Species> Species { get => _species;}
    private static Population _instance;
    private Population()
    {
        
        //_isInitialized = false;
        _species = new List<Species>();
        _generation = new List<NeuralNetwork>();
        _newGeneration = new List<NeuralNetwork>();
        _bots = new List<Bot>();
        CreateRandomPopulation();
        AssignOldGeneration();
        AssignNeats();
        run();
        //_species.Add(new Species());
    }
    public void run()
    {
      //  for (int j = 0; j < 500; j += 1)
       // {

            for (int i = 0; i < Bots.Count; i += 1)
            {
                Bots[i].CheckPos();
           
            
        }
       // }
        for (int i = 0; i < Bots.Count; i += 1)
        {
           Debug.Log(Bots[i].Fitness);
           Debug.Log(Bots[i].Brain.Connection.Count);
           
        }


    }

    public void CreateRandomPopulation()
    {
        for(int i = 0; i < NeatValues.populationSize; i += 1)
        {
            _generation.Add(new NeuralNetwork());
            _bots.Add(new Bot());
        }
        _newGeneration = _generation;
        //Debug.Log(Generation.Count);
       
    }
    public void AssignOldGeneration()
    {
        foreach (var generationMemeber in Generation)
        {
            assignToSpecie(generationMemeber);
            //Debug.Log("ASSIGNING");
        }
        Debug.Log(Species.Count);
        Debug.Log(Species[0].Individuals.Count);
        
       

    }
    public void AssignNeats()
    {
        for (int i=0;i<Bots.Count;i+=1)
        {
            Bots[i].Brain = Generation[i];
            //Debug.Log("ASSIGNING");
        }
        Debug.Log(Species.Count);
        Debug.Log(Species[0].Individuals.Count);

    }
    public bool CheckGenerationStatus()
    {
        foreach(var bot in Bots)
        {
            if (bot.IsAlive)
            {
                return true;
            }
        }
        return false;
    }

    public void removeStaleSpecies()
    {
        for(int i = 0; i < Species.Count; i += 1)
        {
            if (Species[i].StagnationCount > NeatValues.maxStagnation)
            {
                Species.RemoveAt(i);
            }
        }
    }
    public void CalculateAdjustedFitness()
    {
        for(int i = 0; i < Generation.Count; i += 1)
        {
            int sh = 0;
            for(int j = 0; j < Generation.Count; j += 1)
            {
                if (Generation[j].Compare(Generation[i]))
                {
                    sh += 1;
                }
            }
            Generation[i].AdjustedFitness = Generation[i].Fitness / sh;
        }
    }

    public bool assignToSpecie(NeuralNetwork candidate)
    {
        foreach(var specie in Species)
        {
            if (specie.CompareWithFirst(candidate))
            {
                specie.AddIndividual(candidate);
                return true;
            }
            
        }
        CreateSpecie(candidate);
        return false;
    }

    private void CreateSpecie(NeuralNetwork candidate)
    {
        NeatValues.IncreaseSpecie();
        _species.Add(new Species(candidate));
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

    
    public List<NeuralNetwork> Generation { get => _generation; set => _generation = value; }
    public List<NeuralNetwork> NewGeneration { get => _newGeneration; set => _newGeneration = value; }
    public List<Bot> Bots { get => _bots; set => _bots = value; }
}

