using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffensiveGameManager : MonoBehaviour
{
    private static OffensiveGameManager _instance;

    [SerializeField] OffensivePopulationGenerator _dpg;
    [SerializeField] float _gameTimer;
    public int maxSteps=20;
    public int maxShoots = 15;
    float _timer;

    public static OffensiveGameManager Instance { get => _instance; }
    public float GameTimer { get => _gameTimer; set => _gameTimer = value; }

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
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
