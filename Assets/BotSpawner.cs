using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] JumpingTurret _offensiveAiControler;
    [SerializeField] Transform _board;
    // Start is called before the first frame update

    void Start()
    {
        var pop = Population.Instance;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
