using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] OffensiveAiControler _offensiveAiControler;
    [SerializeField] Transform _board;
    // Start is called before the first frame update

    void Start()
    {
        var pop = Population.Instance;
        _offensiveAiControler.NeuralNetwork = pop.loadPopulation();
        _offensiveAiControler.Restart(2, _board);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
