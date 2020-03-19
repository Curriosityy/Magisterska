using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationGenerator : MonoBehaviour
{
    [SerializeField] int _boards;
    [SerializeField] AIControler _aiPrefab;
    [SerializeField] BoardDictionary _boardPrefab;
    List<GameObject> boardList = new List<GameObject>();
    List<GameObject> aiList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        var aiHolder = new GameObject("AiHolder");
        var boardHolder = new GameObject("BoardHolder");
        var population  = Population.Instance;

        population.Initialize();
        for(int i=0;i<_boards*2;i++)
        {
            var ai = Instantiate(_aiPrefab, aiHolder.transform);
            population.Species[0].AddIndividual(ai);
            aiList.Add(ai.gameObject);
        }
        for(int i=0;i<_boards;i++)
        {
            var board = Instantiate(_boardPrefab, new Vector3(i*10,0,0), Quaternion.identity, boardHolder.transform);
            boardList.Add(board.gameObject);
        }
    }
}
