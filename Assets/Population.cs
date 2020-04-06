using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Population
{
    private bool _isInitialized;
    private List<Species> _species;
    private List<NeuralNetwork> _generation;
    private List<NeuralNetwork> _oldGeneration;

    public List<NeuralNetwork> Generation { get => _generation; }
    public List<Species> Species { get => _species; }
    private static Population _instance;
    private Population()
    {
        _species = new List<Species>();
        _generation = new List<NeuralNetwork>();
        _oldGeneration = new List<NeuralNetwork>();
        CreateRandomPopulation();
        AssignGeneration();
    }

    public void GenerateNextPopulation()
    {
        Debug.Log("GeneratingNewPopulation");
        NeatValues.IncreaseGeneration();
        CalculateAdjustedFitness();
        KillWorstIndividualsInAllSpecies();
        DeleteWorstSpecies();
        GenerateNewPopulation();
       // MutateEveryone();
        AssignGeneration();
        DeleteOldGenrationFromSpecies();
        ConnectSpieciesWithOneIndividual();

       // _generation.AddRange(_oldGeneration);
    }

    private void ConnectSpieciesWithOneIndividual()
    {
        var list = _species.Where(s => s.Individuals.Count == 1).ToList();
        for(int i=0;i<list.Count;i+=2)
        {
            if(list.Count>i+1)
            {
                list[i].AddIndividual(list[i + 1].Individuals[0]);
                list[i + 1].Individuals.RemoveAt(0);
            }
        }
        DeleteEmptySpecies();
    }

    private void DeleteOldGenrationFromSpecies()
    {
        foreach(var species in Species)
        {
            for(int i=species.Individuals.Count-1;i>=0;i--)
            {
                if(species.Individuals[i].Generation!=NeatValues.GenerationCount)
                {
                    species.Individuals.RemoveAt(i);
                }
            }
        }
        DeleteEmptySpecies();
    }

    private void DeleteEmptySpecies()
    {
        Species.RemoveAll(s => s.Individuals.Count == 0);
    }

    private void MutateEveryone()
    {

        foreach (var neat in _generation)
        {
            neat.MutateNeuralNetwork();
        }

    }

    private void GenerateNewPopulation()
    { 
        var sum = SumAdjFittnes();
        List<NeuralNetwork> newGeneration = new List<NeuralNetwork>();
        int kidsCounter;
        int eliteCount;
        foreach (var species in _species)
        {

            kidsCounter = Mathf.RoundToInt((species.AdjFitness / sum) * NeatValues.populationSize);
            eliteCount = Mathf.CeilToInt(species.Individuals.Count* NeatValues.elitismRate);
            for (int i = 0; i < kidsCounter; i++)
            {
                if (i < eliteCount)
                {
                    newGeneration.Add(species.GetIndividualOfId(i));
                    Debug.Log("adding elite");
                }
                else
                {
                    if (NeatValues.rnd.NextDouble() < NeatValues.asexualReproductionProbability)
                    {
                        newGeneration.Add(species.AsexualReproduction());
                        Debug.Log("asexual");
                    }
                    else
                    {
                        newGeneration.Add(species.Crossover());
                        Debug.Log("sexual");
                    }
                }
               
                
            }
        }
        
        if (newGeneration.Count < NeatValues.populationSize)
        {
            for(int i=0;i< NeatValues.populationSize- newGeneration.Count; i += 1)
            {
                newGeneration.Add(new NeuralNetwork(newGeneration[NeatValues.rnd.Next(0, newGeneration.Count-1)]));
            }
        }
        _generation.Clear();
        _generation.AddRange(newGeneration);
    }



    //private void GenerateNewPopulation()
    //{
    //    var sum = SumAdjFittnes();
    //    List<NeuralNetwork> newGeneration = new List<NeuralNetwork>();
    //    int kidsCounter;
    //    foreach (var species in _species)
    //    {
    //        kidsCounter = Mathf.RoundToInt(( species.AdjFitness / sum) * NeatValues.populationSize);
    //        for (int i = 0; i < kidsCounter; i++)
    //        {
    //            newGeneration.Add(species.Crossover());
    //        }
    //    }
    //    _oldGeneration.Clear();

    //    foreach (var species in _species)
    //    {
    //        _oldGeneration.AddRange(species.Individuals);
    //    }
    //    _generation.Clear();
    //    NeuralNetwork neat;
    //    Debug.Log("old count:" + _oldGeneration.Count + " new count:" + newGeneration.Count);
    //    for (int i = 0; i < NeatValues.populationSize - _oldGeneration.Count; i++)
    //    {
    //        neat = newGeneration[UnityEngine.Random.Range(0, newGeneration.Count)];
    //        _generation.Add(neat);
    //        newGeneration.Remove(neat);
    //    }

    //}

    private float SumAdjFittnes()
    {
        float sum = 0;
        foreach (var species in _species)
        {
            species.SetSpecieAdjustedFitness();
            sum += species.AdjFitness;
        }
        return sum;
    }


    public void run()
    {

        //speciate generation
        //remove old generation
        //add networks to controllers
        //evaluate - puszczenie w grze i zbieranie punkciorów
        //calculate genomes fitness and adjusted fitness - tu metoda CalculateAdjustedFitness()
        //culling weak networks from species - KillWorstIndividuals()
        //check stagnation and delete weak species (if memebers < 2 or stagnation > 15)
        //Calculate species adjusted fitness
        //crossover -> mutation -> add offsping to population
    }

    private void KillWorstIndividualsInAllSpecies()
    {
        foreach (var species in _species)
        { 
            species.KillWorstIndividuals();
            species.GetAverageFitness();
        }
    }

    private void DeleteWorstSpecies()
    {

        _species.RemoveAll(s => s.Individuals.Count < NeatValues.minSpieceSize || s.StagnationCount > NeatValues.maxStagnation);
        //for (int i = _species.Count - 1; i >= 0; i--)
        //{
        //    if (_species[i].Individuals.Count < NeatValues.minSpieceSize || _species[i].StagnationCount > NeatValues.maxStagnation)
        //    {
        //        _species.Remove(_species[i]);
        //    }
        //}
    }



    private void CreateRandomPopulation()
    {
        for (int i = 0; i < NeatValues.populationSize; i += 1)
        {
            _generation.Add(new NeuralNetwork());
        }
    }
    private void AssignGeneration()
    {
        foreach (var generationMemeber in Generation)
        {
            AssignToSpecie(generationMemeber);
        }
    }

    private void RemoveOldGenerationMembers()
    {
        for (int i = 0; i < _generation.Count; i += 1)
        {
            if (_generation[i].Generation != NeatValues.GenerationCount)
            {
                _generation.RemoveAt(i);
            }
        }
    }

    private void RemoveStaleSpecies()
    {
        for (int i = 0; i < Species.Count; i += 1)
        {
            if (Species[i].StagnationCount > NeatValues.maxStagnation)
            {
                Species.RemoveAt(i);
            }
        }
    }
    private void CalculateAdjustedFitness()
    {
        for (int i = 0; i < Generation.Count; i += 1)
        {
            int sh = 0;
            for (int j = 0; j < Generation.Count; j += 1)
            {
                if (Generation[j].Compare(Generation[i]))
                {
                    sh += 1;
                }
            }
            Generation[i].AdjustedFitness = Generation[i].Fitness / sh;
        }
    }

    private bool AssignToSpecie(NeuralNetwork candidate)
    {
        foreach (var specie in Species)
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
    public static Population Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Population();
            }
            return _instance;
        }

    }

}

