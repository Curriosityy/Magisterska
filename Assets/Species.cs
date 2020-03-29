using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Species
{
    private int _specieID;
    private List<AIControler> _individuals;
    private float _avgFitness;
    private float _adjFitness;
    private float _stagnationCount=0;
    System.Random rnd = new System.Random();
    public float AvgFitness { get => _avgFitness;}
    public List<AIControler> Individuals { get => _individuals; }
    public int SpecieID { get => _specieID; set => _specieID = value; }
    public float AdjFitness { get => _adjFitness; }
    public float StagnationCount { get => _stagnationCount;}

    public Species()
    {
        NeatValues.IncreaseSpecie();
        _specieID = NeatValues.SpecieCount;
        _individuals = new List<AIControler>();
        for(int i = 0; i < NeatValues.populationSize; i += 1)
        {
            _individuals.Add(new AIControler());
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
    public Species(AIControler existing)
    {
        NeatValues.IncreaseSpecie();
        _specieID = NeatValues.SpecieCount;
        _individuals = new List<AIControler>();
        _individuals.Add(existing);
    }
    
    public NeuralNetwork Crossover(NeuralNetwork child)
    {
      
        /*
         NeuralNetwork parent1 =_individuals[rnd.Next(0,_individuals.Count-1)];
         NeuralNetwork parent2 = _individuals[rnd.Next(0, _individuals.Count - 1)];
         NeuralNetwork temp;
         if (parent1.GetFitness() < parent2.GetFitness())
         {
             temp=parent1;
             parent1=parent2;
             parent2=temp;
         }

         
        child = new NeuralNetwork(parent1);
        for (int i = 0; i < child.Connection.Count; i += 1)
        {
            if (parent2.DoesInnovNumberExist(child.Connection[i].Id))
            {
                if (rnd.Next(3) < 2)
                {
                    int edgePos=parent2.getEgdeId(child.Connection[i].Id);
                    child.Connection[i] = new Edge(child.Connection[edgePos]);
                }
            }
        }
        */
        return child;
    }
    public int CompareWithAll(AIControler testSubject)
    {
        int matchesFound = 0;
        for(int i = 0; i < _individuals.Count; i += 1)
        {
           /* if (_individuals[i].Compare(testSubject))
            {
                matchesFound += 1;
            }*/
        }
        return matchesFound;
    }

    public bool CompareWithFirst(NeuralNetwork testSubject)
    {

        /*if (_individuals[0].Compare(testSubject))
        {
            return true;
        }*/
        return false;
    }

    public void AddIndividual(AIControler individual)
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
            if (/*_individuals[i].generation != NeatValues.GenerationCount*/true)
            {
                _individuals.RemoveAt(i);
            }
        }
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
        fitness = fitness / _individuals.Count;
        CheckStagnation(fitness);
        _avgFitness = fitness; 
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
    private void CheckStagnation(float newfitness)
    {
        if (_adjFitness > newfitness)
        {
            _stagnationCount += 1;
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
        int remaining=Mathf.CeilToInt(_individuals.Count-1*NeatValues.survivingRate);
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
