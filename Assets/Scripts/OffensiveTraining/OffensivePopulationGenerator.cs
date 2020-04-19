using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OffensivePopulationGenerator : MonoBehaviour
{
    [SerializeField] GameObject _aiPrefab;
    [SerializeField] BoardDictionary _boardPrefab;
    [SerializeField] GameObject _turretPrefab;

    //Obie listy mogą zostać wykożystane do ustawiania bitew między AI
    List<GameObject> _boardList = new List<GameObject>();
    List<OffensiveAiControler> _aiList = new List<OffensiveAiControler>();
    List<JumpingTurret> _jumpingTurret = new List<JumpingTurret>();
    
    // Start is called before the first frame update
    void Start()
    {

        var aiHolder = new GameObject("AiHolder");
        var boardHolder = new GameObject("BoardHolder");
        var turretHolder = new GameObject("TurretHolder");
        var population = Population.Instance;
        GenerateBoards(boardHolder);
        for (int i = 0; i < NeatValues.populationSize; i++)
        {
            var ai = Instantiate(_aiPrefab, aiHolder.transform);
            _aiList.Add(ai.GetComponent<OffensiveAiControler>());
            _aiList[i].AssignToBoard(_boardList[i].transform);
            _aiList[i].Restart();

            ai = Instantiate(_turretPrefab, turretHolder.transform);
            _jumpingTurret.Add(ai.GetComponent<JumpingTurret>());
            _jumpingTurret[i].AssignToBoard(_boardList[i].transform);
            _jumpingTurret[i].Restart();

        }
        AssignNeatToAi();
    }

    private void GenerateBoards(GameObject boardHolder)
    {
        for (int i = 0; i < NeatValues.populationSize; i++)
        {
            var board = Instantiate(_boardPrefab, new Vector3(0, i*3, 0), Quaternion.identity, boardHolder.transform);
            _boardList.Add(board.gameObject);
        }
    }

    public void AssignNeatToAi()
    {
        var neats = Population.Instance.Generation;
        for (int i = 0; i < NeatValues.populationSize; i++)
        {
            _aiList[i].NeuralNetwork = neats[i];
            _aiList[i].Restart();
            _jumpingTurret[i].Restart();
        }
    }

    public void AssignPointsToNeat()
    {
        foreach (var ai in _aiList)
        {
            ai.NeuralNetwork.Fitness = ai.Points;
            //if (ai.IsAlive)
                //ai.NeuralNetwork.Fitness += 100;
            if (NeatValues.BestFitness < ai.NeuralNetwork.Fitness)
                NeatValues.BestFitness = ai.NeuralNetwork.Fitness;
        }
    }

    public bool AreAllAiDead()
    {
        return _aiList.Where(a => a.IsAlive).Count() == 0;
    }
}
