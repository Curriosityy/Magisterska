using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationGenerator : MonoBehaviour
{
    [SerializeField] GameObject _aiPrefab;
    [SerializeField] BoardDictionary _boardPrefab;

    float gameTimer = 30;
    float timer;
    //Obie listy mogą zostać wykożystane do ustawiania bitew między AI
    List<GameObject> _boardList = new List<GameObject>();
    List<AIControler> _aiList = new List<AIControler>();
    // Start is called before the first frame update
    void Start()
    {
        var aiHolder = new GameObject("AiHolder");
        var boardHolder = new GameObject("BoardHolder");
        var population  = Population.Instance;
        for(int i=0;i< NeatValues.populationSize; i++)
        {
            var ai = Instantiate(_aiPrefab, aiHolder.transform);
            //population.Species[0].AddIndividual(ai.GetComponent<AIControler>());
            _aiList.Add(ai.GetComponent<AIControler>());
        }
        for(int i=0;i< NeatValues.populationSize / 2; i++)
        {
            var board = Instantiate(_boardPrefab, new Vector3(i*10,0,0), Quaternion.identity, boardHolder.transform);
            _boardList.Add(board.gameObject);
        }
        AssignNeatToAi();
    }

    void AssignNeatToAi()
    {
        Debug.Log("AssignNeatToAi");
        var neats = Population.Instance.Generation;
        for(int i=0;i<neats.Count;i++)
        {
            _aiList[i].Restart();
            _aiList[i].NeuralNetwork = neats[i];
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer>=gameTimer || AreAllAiDead())
        {
            Debug.Log("GameEnd");
            AssignPointsToNeat();
            Population.Instance.GenerateNextPopulation();
            AssignNeatToAi();
            timer = 0;
        }
    }

    private void AssignPointsToNeat()
    {
        Debug.Log("AssignPoints");
        foreach (var ai in _aiList)
        {
            ai.NeuralNetwork.Fitness = ai.Points;
        }
    }

    private bool AreAllAiDead()
    {
        return _aiList.Where(a => a.IsAlive).Count() == 0;
    }
}
