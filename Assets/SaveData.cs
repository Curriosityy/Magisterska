using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveData
{
    public List<float> allGens = new List<float>();
    public int GenerationNumber;
    public float TotalFitness;
    public float AverageFitness;
    public float AverageAdjFitness;
    public int PopulationSize;
    public int SpeciesCount;
    public float dc,ec,wc;
    public float SurvivingRate;
    public float simularityTreshhold;
    public float addConnProbability;
    public float removeConnProbability;
    public float addNodeProbability;
    public float removeNodeProbability;
    public float minWeight;
    public float minBias;
    public float weightMutationProbability;
    private void saveData()
    {
        string fileName = Application.streamingAssetsPath + "/XML/" + NeatValues.GenerationCount + ".xml";
        XmlSerializer serializer = new XmlSerializer(typeof(SaveData));

        using (FileStream stream = new FileStream(fileName, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }

    }
   public void getData()
    {
        GenerationNumber = NeatValues.GenerationCount;
        PopulationSize = NeatValues.populationSize;
        minBias = NeatValues.minBias;
        minWeight = NeatValues.minWeight;
        weightMutationProbability = NeatValues.weightMutationProbability;
        removeNodeProbability=NeatValues.removeNodeProbability;
        addNodeProbability=NeatValues.addNodeProbability;
        removeConnProbability = NeatValues.changeConnStatusProbability;
        addConnProbability = NeatValues.addConnProbability;
        simularityTreshhold = NeatValues.simularityTreshhold;
        SurvivingRate = NeatValues.survivingRate;
        dc = NeatValues.disjoinsCoefficiant;
        ec = NeatValues.excessjoinsCoefficiant;
        wc = NeatValues.weightCoefficiant;
        allGens.Add(AverageFitness);
        saveData();
    }
}
