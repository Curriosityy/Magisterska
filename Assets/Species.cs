using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Species
{
    private int _specieID;
    private List<AIControler> _individuals;
    private float _avgFitness;
    private float _adjFitness;

    public float AvgFitness { get => _avgFitness;}
    public List<AIControler> Individuals { get => _individuals; }
    public int SpecieID { get => _specieID; set => _specieID = value; }
    public float AdjFitness { get => _adjFitness; }

    public Species(/*NeatValues nv*/)
    {
        //_specieID = nv.GetSpecieID();
        //nv.IncreaseSpecieID();
        _individuals = new List<AIControler>();
    }
    

    public void AddIndividual(AIControler individual)
    {
        _individuals.Add(individual);
    }

    public AIControler GetFirstIndividual()
    {
        return _individuals[0];
    }

    public void GetAverageFitness()
    {
        float fitness = 0f;
        foreach (var individual in _individuals)
        {
            //fitness += individual.GetFitness();
        }
        _avgFitness = fitness/_individuals.Count; 
    }
    public void GetTotalAdjustedFitness()
    {
        float adjustedFitness = 0f;
        foreach (var individual in _individuals)
        {
            //adjustedFitness += individual.GetAdjFitness();
        }
        _adjFitness = adjustedFitness;
    }
    public void SetIndividualAdjustedFitness()
    {
        int specieCount = _individuals.Count;
        foreach (var individual in _individuals)
        {
            //individual.SetAdjustedFitness(individual.GetFitness()/specieCount);
        }
    }

    public void KillWorstIndividuals(float cullingPercentage)
    {
        SortIndividuals();
        int remaining=Mathf.CeilToInt(_individuals.Count*cullingPercentage);
        _individuals = _individuals.GetRange(0, remaining);

    }

    public AIControler GetBestIndividual()
    {
        SortIndividuals();
        return _individuals[0];
    }

    public void SortIndividuals()
    {
        //_individuals.Sort((p1, p2) => p1.GetFitess().CompareTo(p2.GetFitess()));
        _individuals.Reverse();
    }

    public int GetSpecieCount()
    {
        return _individuals.Count;
    }

    public float GetIndividualFitness(int id)
    {
        return 0f;//_individuals[id].GetFitness();
    }


}
