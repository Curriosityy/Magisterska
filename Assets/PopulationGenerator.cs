using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationGenerator : MonoBehaviour
{
    [SerializeField] GameObject _aiPrefab;
    [SerializeField] BoardDictionary _boardPrefab;
    [SerializeField] Transform _spawnPoint;

    public GameObject obstaclePrefab;

    [SerializeField] float _gameTimer = 15;
    float timer;
    //Obie listy mogą zostać wykożystane do ustawiania bitew między AI
    List<GameObject> _boardList = new List<GameObject>();
    List<AIControler> _aiList = new List<AIControler>();
    // Start is called before the first frame update
    void Start()
    {

        var aiHolder = new GameObject("AiHolder");
        var boardHolder = new GameObject("BoardHolder");
        var population = Population.Instance;
        for (int i = 0; i < NeatValues.populationSize; i++)
        {
            var ai = Instantiate(_aiPrefab, aiHolder.transform);
            ai.GetComponent<AIControler>().spawnPoint = _spawnPoint;
            _aiList.Add(ai.GetComponent<AIControler>());
        }
        GenerateMap(boardHolder);
        AssignNeatToAi();
    }

    private void GenerateBoards(GameObject boardHolder)
    {
        for (int i = 0; i < NeatValues.populationSize / 2; i++)
        {
            var board = Instantiate(_boardPrefab, new Vector3(i * 10, 0, 0), Quaternion.identity, boardHolder.transform);
            _boardList.Add(board.gameObject);
        }
    }

    private void GenerateMap(GameObject boardHolder)
    {
        var pos = _spawnPoint.position;
        for (int i=0;i<40;i++)
        {
            if (i % 2 == 0)
            {
                pos.x += 4;
            }
            pos.x += UnityEngine.Random.Range(7,8);
            Instantiate(obstaclePrefab, pos, Quaternion.identity, boardHolder.transform);

        }
    }

    void AssignNeatToAi()
    {
        var neats = Population.Instance.Generation;
        for(int i=0;i<NeatValues.populationSize;i++)
        {
            _aiList[i].NeuralNetwork = neats[i];
            _aiList[i].Restart();
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer>=_gameTimer || AreAllAiDead())
        {
            AssignPointsToNeat();
            Population.Instance.GenerateNextPopulation();
            AssignNeatToAi();
            timer = 0;
        }
    }

    private void AssignPointsToNeat()
    {
        foreach (var ai in _aiList)
        {
            ai.NeuralNetwork.Fitness = ai.Points;
            if (ai.IsAlive)
                ai.NeuralNetwork.Fitness += 100;
            if (NeatValues.BestFitness < ai.NeuralNetwork.Fitness)
                NeatValues.BestFitness = ai.NeuralNetwork.Fitness;
        }
    }

    private bool AreAllAiDead()
    {
        return _aiList.Where(a => a.IsAlive).Count() == 0;
    }
}
