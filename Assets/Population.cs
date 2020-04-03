using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population
{
    private bool _isInitialized;
    private List<Species> _species;
    private List<NeuralNetwork> _generation;

    private List<Bot> _bots;
    public List<Species> Species { get => _species;}
    private static Population _instance;
    private Population()
    {
        
        //_isInitialized = false;
        _species = new List<Species>();
        _generation = new List<NeuralNetwork>();
        _bots = new List<Bot>();
        CreateRandomPopulation();
        
        run();
        //_species.Add(new Species());
    }
    public void run()
    {
        while (NeatValues.GenerationCount<NeatValues.generationTreshhold)
        {

            AssignGeneration();
            Debug.Log(NeatValues.GenerationCount);
            RemoveOldGenerationMembers();
            Debug.Log(Generation.Count);
            AssignNeats();
            Debug.Log(NeatValues.BestFitness);
            EvaluateGen();
            CalculateFitness();
            CalculateAdjustedFitness();
            CullingSpecies();
            CalculateSpeciesFitness();
            RemoveWeakSpecies();
            CalculateSpecieAdjustedFitness();
            CalculateSpecieOffspring();
            Debug.Log(NeatValues.BestFitness);
        }
      
        //speciate generation +
        //remove old generation +
        //add networks to controllers +
        //evaluate +
        //calculate genomes fitness and adjusted fitness +
        //culling weak networks from species +
        //check stagnation and delete weak species (if memebers < 2 or stagnation > 15)+
        //Calculate specie adjusted fitness +
        //crossover -> mutation -> add offsping to population +

        


    }
    public void CalculateSpecieAdjustedFitness()
    {
        for (int i = 0; i < Species.Count; i += 1)
        {
            Species[i].CalculateSpecieAdjustedFitness();
        }
    }
    public void RemoveWeakSpecies()
    {
        for(int i = 0; i < Species.Count; i += 1)
        {
            if(Species[i].StagnationCount>NeatValues.maxStagnation || Species[i].Individuals.Count < NeatValues.minSpieceSize)
            {
                Species.RemoveAt(i);
            }
        }
    }
    public void CalculateSpecieOffspring()
    {
        int added = 0;
        float allSpecieAdjFitness = 0;
        for (int i = 0; i < Species.Count; i += 1)
        {
            allSpecieAdjFitness += Species[i].AdjFitness;
        }
        for (int i = 0; i < Species.Count; i += 1)
        {
            
            int offspringNumber = Mathf.RoundToInt(Species[i].AdjFitness/allSpecieAdjFitness*50);
            for (int j = 0; j < offspringNumber; j++)
            {
                _generation.Add(Species[i].Crossover(new NeuralNetwork()));
                added += 1;
            }
        }
        //Debug.Log("ADDED" + added);

    }
    public void CalculateSpeciesFitness()
    {
        for (int i = 0; i < Species.Count; i += 1)
        {
            Species[i].CalculateAverageFitness();
        }
    }
    public void CullingSpecies()
    {
        foreach(var specie in Species)
        {
            specie.KillWorstIndividuals();
        }
    }
    public void EvaluateGen()
    {
        for (int j = 0; j < 500; j += 1)
        {
            for (int i = 0; i < Bots.Count; i += 1)
            {
                Bots[i].CheckPos();
            }
        }
    }
    public void CreateRandomPopulation()
    {
        for(int i = 0; i < NeatValues.populationSize; i += 1)
        {
            _generation.Add(new NeuralNetwork());
            _bots.Add(new Bot());
        }
       
        //Debug.Log(Generation.Count);
       
    }
    public void AssignGeneration()
    {
         
        foreach (var generationMemeber in Generation)
        {
            if(generationMemeber.Generation==NeatValues.GenerationCount)
            assignToSpecie(generationMemeber);
            //Debug.Log("ASSIGNING");
        }

        //Debug.Log(Species.Count);
        //Debug.Log(Species[0].Individuals.Count);
       
    }
    public void AssignNeats()
    {
        for (int i=0;i<Bots.Count;i+=1)
        {
            //Debug.Log(Bots.Count+" "+Generation.Count);
            Bots[i].Brain = Generation[i];
            //Debug.Log("ASSIGNING");
        }
        //Debug.Log(Species.Count);
        //Debug.Log(Species[0].Individuals.Count);

    }
    public void RemoveOldGenerationMembers()
    {

        for (int i = 0; i < _generation.Count; i += 1)
        {
            if (_generation[i].Generation != NeatValues.GenerationCount)
            {
                _generation.RemoveAt(i);
            }
        }
        NeatValues.IncreaseGeneration();

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
    public void CalculateFitness()
    {
        for (int i = 0; i < Bots.Count; i += 1)
        {
            Bots[i].Brain.Fitness = Bots[i].Fitness;
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
 
    public List<Bot> Bots { get => _bots; set => _bots = value; }
}

