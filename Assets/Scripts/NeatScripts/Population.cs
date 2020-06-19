using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Serialization;
using System.IO;

public class Population
{
    private bool _isInitialized;
    private List<Species> _species;
    private List<NeuralNetwork> _generation;
    private List<NeuralNetwork> _oldGeneration;
    private SaveData populationSave = new SaveData();
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
        for(int i = 0; i < 200; i += 1)
        {
            OffensiveAiControler.roadplan.Add(NeatValues.rnd.Next(0, 49));
        }
        
       
    }
    public void RestartObstacles()
    {

    }

    public void GenerateNextPopulation()
    {

        //Debug.Log("GeneratingNewPopulation");
        
        NeatValues.IncreaseGeneration();
        CalculateAdjustedFitness();
        getDatatoXML();
        KillWorstIndividualsInAllSpecies();
        DeleteWorstSpecies();
        GenerateNewPopulation();
        AssignGeneration();
        DeleteOldGenrationFromSpecies();
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
            eliteCount = Mathf.CeilToInt(kidsCounter* NeatValues.elitismRate);
            if (eliteCount > species.Individuals.Count - 1)
            {
                eliteCount = species.Individuals.Count - 1;
            }
            for (int i = 0; i < kidsCounter; i++)
            {
                if (i < eliteCount)
                {
                    newGeneration.Add(new NeuralNetwork(species.Individuals[i], Color.red));
                }
                else
                {
                    if (NeatValues.rnd.NextDouble() < NeatValues.asexualReproductionProbability)
                    {
                        newGeneration.Add(species.AsexualReproduction());
                        //Debug.Log("asexual");
                    }
                    else
                    {
                        newGeneration.Add(species.Crossover());
                        //Debug.Log("sexual");
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
        _generation.RemoveAll(g => g.Generation != NeatValues.GenerationCount);
    }

    private void RemoveStaleSpecies()
    {
        Species.RemoveAll(s => s.StagnationCount > NeatValues.maxStagnation);
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
    public void getDatatoXML()
    {
        populationSave.AverageAdjFitness = SumAdjFittnes() / NeatValues.populationSize;
        populationSave.AverageFitness = getSumFitness() / NeatValues.populationSize;
        populationSave.TotalFitness = getSumFitness();
        populationSave.SpeciesCount = Species.Count;
        populationSave.getData();
    }
    public float getSumFitness()
    {
        float fitnessSum=0;
        foreach(var ind in _generation)
        {
            fitnessSum += ind.Fitness;
        }
        return fitnessSum;
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
    public void savePopulation()
    {
        string fileName = Application.streamingAssetsPath + "/XML/"+NeatValues.GenerationCount+".xml";
        XmlSerializer serializer = new XmlSerializer(typeof(NeuralNetwork));
        
        using (FileStream stream = new FileStream(fileName, FileMode.Create))
        {
            serializer.Serialize(stream, _generation[0]);
        }

    }
    public void loadPopulation(int gen=0)
    {
        List<NeuralNetwork> loadedPopulation = new List<NeuralNetwork>();
        XmlSerializer serializer = new XmlSerializer(typeof(NeuralNetwork));
        string fileName = Application.streamingAssetsPath + "/XML/"+gen+".xml";
        using (FileStream stream = new FileStream(fileName, FileMode.Open))
        {
            var neat = serializer.Deserialize(stream);
            NeuralNetwork loadedNeat = (NeuralNetwork)neat;
            foreach(var conn in loadedNeat.Connection)
            {
                NeuralNetwork.allEdges.Add(new Edge(conn));
            }
            
            for (int i = 0; i < NeatValues.populationSize; i += 1)
            {
                loadedPopulation.Add(new NeuralNetwork(loadedNeat));
            }
        }
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

