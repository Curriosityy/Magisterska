﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NeatValues {
    private static int _generationCount = 0;
    private static int _specieCount=0;
    private static float _bestFitness = 0;
    public static System.Random rnd = new System.Random();
    public static int SpecieCount { get => _specieCount; }
    public static float BestFitness { get => _bestFitness; set => _bestFitness = value; }
    public static int GenerationCount { get => _generationCount;}

    public static void IncreaseGeneration()
    {
        _generationCount += 1;
    }
    public static void IncreaseSpecie()
    {
        _specieCount += 1;
    }

    public const int inputNeutonSize = 2;
    public const int outputNeuronSize = 1;
    //Population 
    /// <summary>
    /// Population size powinno być parzyste
    /// </summary>
    public const int populationSize=50;
    public const int generationTreshhold=9999;
    public const int fitnessTreshold=9999;
    public const int activationFunction=1; // 1=ReLu
    //public const float linearActivFunValue = 1f;
    //Speciation
    public const int maxStagnation=15;
    public const float weightCoefficiant=0.5f;
    public const float disjoinsCoefficiant=1f;
    public const float excessjoinsCoefficiant=1f;
    public const float survivingRate=0.3f;
    public const float elitismRate = 0.1f;
    public const float asexualReproductionProbability = 0.2f;
    public const int minSpieceSize=2;
    public const float simularityTreshhold=2f;
    //Genome
    public const float addConnProbability=0.4f;
    public const float removeConnProbability=0.2f;
    public const float changeConnStatusProbability=0.1f;
    public const float addNodeProbability=0.2f;
    public const float removeNodeProbability=0.2f;
    //Node
    public const float minBias=-5f;
    public const float maxBias= 5f;
    public const float biasMutationProbability=0.5f;
    public const float biasRandomMutationProbability=0.1f;

    //Connection
    public const float minWeight=-5f;
    public const float maxWeight=5f;
    public const float weightMutationProbability=0.5f;
    public const float weightRandomMutationProbability=0.1f;

   
}
