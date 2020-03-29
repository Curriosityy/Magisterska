using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationGenerator : MonoBehaviour
{
    [SerializeField] GameObject _aiPrefab;
    [SerializeField] BoardDictionary _boardPrefab;


    //Obie listy mogą zostać wykożystane do ustawiania bitew między AI
    List<GameObject> _boardList = new List<GameObject>();
    List<GameObject> _aiList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        var aiHolder = new GameObject("AiHolder");
        var boardHolder = new GameObject("BoardHolder");
        var population  = Population.Instance;

        for(int i=0;i< NeatValues.populationSize; i++)
        {
            var ai = Instantiate(_aiPrefab, aiHolder.transform);
            population.Species[0].AddIndividual(ai.GetComponent<AIControler>());
            _aiList.Add(ai.gameObject);
        }
        for(int i=0;i< NeatValues.populationSize / 2; i++)
        {
            var board = Instantiate(_boardPrefab, new Vector3(i*10,0,0), Quaternion.identity, boardHolder.transform);
            _boardList.Add(board.gameObject);
        }
    }
}
