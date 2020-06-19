using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OffensivePopulationGenerator : MonoBehaviour
{
    [SerializeField] GameObject _aiPrefab;
    [SerializeField] BoardDictionary _boardPrefab;
    [SerializeField] GameObject _turretPrefab;
    [SerializeField] Text _current;
    //Obie listy mogą zostać wykożystane do ustawiania bitew między AI
    List<GameObject> _boardList = new List<GameObject>();
    List<OffensiveAiControler> _aiList = new List<OffensiveAiControler>();
    List<JumpingTurret> _jumpingTurret = new List<JumpingTurret>();


    public List<GameObject> BoardList { get => _boardList; set => _boardList = value; }
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
            _aiList[i].AssignToBoard(BoardList[i].transform);
            _aiList[i].Restart();

            ai = Instantiate(_turretPrefab, turretHolder.transform);
            _jumpingTurret.Add(ai.GetComponent<JumpingTurret>());
            _jumpingTurret[i].AssignToBoard(BoardList[i].transform);
            _jumpingTurret[i].Restart();

        }
        AssignNeatToAi();
    }

    private void Update()
    {
        int max = 0;
        for (int i = 0; i < _aiList.Count; i++)
        {
            if (_aiList[max].Points < _aiList[i].Points)
            {
                max = i;
            }
        }
        var t = Camera.main.transform.position;
        t.y = _aiList[max].ControledMinion.transform.position.y;
        Camera.main.transform.position = t;
        _current.text = _aiList[max].Points.ToString();
    }

    private void GenerateBoards(GameObject boardHolder)
    {
        for (int i = 0; i < NeatValues.populationSize; i++)
        {

            var board = Instantiate(_boardPrefab, new Vector3(0, i * 3, 0), Quaternion.identity, boardHolder.transform);
            _boardList.Add(board.gameObject);
        }
    }

    public void AssignNeatToAi()
    {
        var neats = Population.Instance.Generation;
        for (int i = 0; i < neats.Count; i++)
        {
            if (i < _aiList.Count)
            {
                _aiList[i].NeuralNetwork = neats[i];
                _aiList[i].Restart();
                _jumpingTurret[i].Restart();
            }

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
