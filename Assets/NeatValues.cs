using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NeatValues {
    private static int _generationCount = 0;
    private static int _specieCount=0;
    private static float _bestFitness = 0;
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


    //Population
    const int populationSize=50;
    const int generationTreshhold=9999;
    const int fitnessTreshold=9999;
    const int activationFunction=1; // 1=ReLu
    //Speciation
    const int maxStagnation=15;
    const float weightCoefficiant=0.4f;
    const float disjoinsCoefficiant=1f;
    const float excessjoinsCoefficiant=1f;
    const float survivingRate=0.8f;
    const int minSpieceSize=2;
    const float simularityTreshhold=2f;
    //Genome
    const float addConnProbability=0.8f;
    const float removeConnProbability=0.3f;
    const float changeConnStatusProbability=0.3f;
    const float addNodeProbability=0.5f;
    const float removeNodeProbability=0.2f;
    //Node
    const float minBias=-1f;
    const float maxBias=1f;
    const float biasMutationProbability=0.5f;
    const float biasRandomMutationProbability=0.1f;

    //Connection
    const float minWeight=-1f;
    const float maxWeight=1f;
    const float weightMutationProbability=0.5f;
    const float weightRandomMutationProbability=0.1f;

   
}
