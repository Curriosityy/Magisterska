using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JumpingAIControler : MonoBehaviour
{
    [SerializeField] private NeuralNetwork _neuralNetwork;
    [SerializeField] private GameObject _minionPrefab;
    private SimpleMinionBehaviour _controledMinion;
    public Transform spawnPoint;

    bool canJump = true;
    public NeuralNetwork NeuralNetwork { get => _neuralNetwork; set => _neuralNetwork = value; }
    public float Points
    {
        get
        {
            if (_controledMinion != null)
                return _controledMinion.Points;
            return 0;
        }
    }
    public bool IsAlive
    {
        get
        {
            if (_controledMinion != null)
                return _controledMinion.IsAlive;
            return false;

        }
    }

    public void Restart()
    {
        StopAllCoroutines();
        if (_controledMinion == null)
        {
            _controledMinion = Instantiate(_minionPrefab, spawnPoint.position, Quaternion.identity, null).GetComponent<SimpleMinionBehaviour>();
            _controledMinion.aiControling = this;
        }
        else
        {
            _controledMinion.aiControling = this;
            _controledMinion.Restart();
            _controledMinion.GetComponent<Renderer>().material.color = _neuralNetwork.Color;
        }

    }

    private void Update()
    {
        if (IsAlive && !_controledMinion.IsJumping)
        {
            CalculateMove();
        }

    }

    private void CalculateMove()
    {
        float[] inputValue;

        _controledMinion.GetDistanceToNextObstacle(out inputValue);
        var value = _neuralNetwork.CalculateNeuralNetworkValue(inputValue);


        if (Mathf.RoundToInt(value[0]) >= 1)
            _controledMinion.Jump();
    }

}
