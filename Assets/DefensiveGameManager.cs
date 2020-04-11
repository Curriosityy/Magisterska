using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveGameManager : MonoBehaviour
{
    [SerializeField] DefensivePopulationGenerator _dpg;
    [SerializeField] float _gameTimer;
    public int maxSteps=20;
    float _timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _gameTimer || _dpg.AreAllAiDead())
        {
            _dpg.AssignPointsToNeat();
            Population.Instance.GenerateNextPopulation();
            _dpg.AssignNeatToAi();
            _timer = 0;
        }
    }
}
