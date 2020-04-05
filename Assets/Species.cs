using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Species
{
    private int _specieID;
    private List<NeuralNetwork> _individuals = new List<NeuralNetwork>();
    private float _avgFitness;
    private float _maxfitness;
    private float _adjFitness;
    private float _stagnationCount=0;

    System.Random rnd = new System.Random();
    public float AvgFitness { get => _avgFitness;}
    public List<NeuralNetwork> Individuals { get => _individuals; }
    public int SpecieID { get => _specieID; set => _specieID = value; }
    public float AdjFitness { get => _adjFitness; }
    public float StagnationCount { get => _stagnationCount;}
    public float Maxfitness { get => _maxfitness; set => _maxfitness = value; }

    public Species()
    {
        
        _specieID = NeatValues.SpecieCount;
        _individuals = new List<NeuralNetwork>();
        _maxfitness = 0;
        for(int i = 0; i < NeatValues.populationSize; i += 1)
        {
            _individuals.Add(new NeuralNetwork());
            /// <summary>
            /// Nie możesz dodawać ai controlera przez new, to jest monobehaviour czyli obiekt w świecie unity.
            /// AiControler posiada w sobie sieć neat, która jest tworzona podczas tworzenia Ai w świecie.
            /// Poprzez AiControler.NeuralNetwork możesz się dostać do sieci neuronowej.
            /// Lista wszystkich AiControlerów jest w PopulationGenerator, później (jeżeli chcemy pisać ze sztuką unity)
            /// dostęp powinien być przeniesiony do klasy AIManager.
            /// Manager jest to klasa, która powinna dawać dostęp do obiektów.
            /// IMO, species powinieneś tworzyć puste, a potem w population czy gdzieś przypisywać do species dany obiekt, lub
            /// tworzysz tutaj początkową populacje NeuralNetworków, którą później musisz przypisać do AiControlera tak jak gadaliśmy ostatnio.
            /// Wtedy listą będzie NeuralNetwork.
            /// </summary>
        }
    }
    public Species(NeuralNetwork existing)
    {
        _maxfitness = 0;
        _specieID = NeatValues.SpecieCount;
        _individuals.Add(existing);
        
    }
    
    public NeuralNetwork Crossover()
    {
     
         NeuralNetwork parent1 =_individuals[rnd.Next(0,_individuals.Count-1)];
         NeuralNetwork parent2 = _individuals[rnd.Next(0, _individuals.Count - 1)];
         NeuralNetwork temp;
         if (parent1.Fitness < parent2.Fitness)
         {
             temp=parent1;
             parent1=parent2;
             parent2=temp;
         }

         
        var child = new NeuralNetwork(parent1);
        for (int i = 0; i < child.Connection.Count; i += 1)
        {
            if (parent2.DoesInnovNumberExist(child.Connection[i].Id))
            {
                if (rnd.Next(3) < 2)
                { 
                    
                    int edgePos=parent2.getEgdeId(child.Connection[i].Id);
                    
                    child.Connection[i] = new Edge(parent2.Connection[edgePos]);
                }
            }
        }
        
        return child;
    }
    public int CompareWithAll(NeuralNetwork testSubject)
    {
        int matchesFound = 0;
        for(int i = 0; i < _individuals.Count; i += 1)
        {
            if (_individuals[i].Compare(testSubject))
            {
                matchesFound += 1;
            }
        }
        return matchesFound;
    }

    public bool CompareWithFirst(NeuralNetwork testSubject)
    {
        return _individuals[0].Compare(testSubject);
    }

    public void AddIndividual(NeuralNetwork individual)
    {
        _individuals.Add(individual);
    }
    public void RemoveIndividual(int numberOfIndividual)
    {
        _individuals.RemoveAt(numberOfIndividual);
    }

    public void RemoveFromPreviosGeneration()
    {
        for(int i = 0; i < _individuals.Count; i += 1)
        {
            if (_individuals[i].Generation != NeatValues.GenerationCount)
            {
                _individuals.RemoveAt(i);
            }
        }
    }

    public NeuralNetwork GetFirstIndividual()
    {
        return _individuals[0];
    }

    public void GetAverageFitness()
    {
        float fitness = 0f;
        foreach (var individual in _individuals)
        {
            fitness += individual.Fitness;
        
        }
        fitness = fitness / _individuals.Count;
        CheckStagnation(fitness);
        _avgFitness = fitness; 
    }
    public void SetSpecieAdjustedFitness()
    {
        float adjustedFitness = 0f;
        foreach (var individual in _individuals)
        {
            adjustedFitness += individual.AdjustedFitness;
        }
       
        _adjFitness = adjustedFitness;
    }
    private void CheckStagnation(float newfitness)
    {
        if (_maxfitness > newfitness)
        {
            _stagnationCount += 1;
        }
        else
        {
            _maxfitness = newfitness;
        }
    }

    //Trzeba zrobic w populacji, policzyc dystans od kazdego osobnika nie tylko tych w gatunku.

    /*public void SetIndividualAdjustedFitness()
    {
        int specieCount = _individuals.Count;
        foreach (var individual in _individuals)
        {
            //individual.SetAdjustedFitness(individual.GetFitness()/specieCount);
        }
    }*/

    public void KillWorstIndividuals()
    {
        SortIndividuals();
        int remaining=Mathf.CeilToInt((_individuals.Count-1)*NeatValues.survivingRate);
        _individuals = _individuals.GetRange(0, remaining);

    }

    public NeuralNetwork GetBestIndividual()
    {
        SortIndividuals();
        return _individuals[0];
    }

    public void SortIndividuals()
    {
        _individuals.Sort((p1, p2) => p1.Fitness.CompareTo(p2.Fitness));
        _individuals.Reverse();
    }

    public int GetSpecieCount()
    {
        return _individuals.Count;
    }

    public float GetIndividualFitness(int id)
    {
        return _individuals[id].Fitness;
    }


}
