using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControler : MonoBehaviour
{
    [SerializeField] private NeuralNetwork _neuralNetwork;
    [SerializeField] private GameObject _minionPrefab;
    [SerializeField] private SimpleMinionBehaviour _controledMinion;
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
    public bool IsAlive {
        get
        {
            if (_controledMinion != null)
                return _controledMinion.IsAlive;
            return false;

        }
    }

    public void Restart()
    {
        Debug.Log("Restart");
        StopAllCoroutines();
        if(_controledMinion!=null)
        {
            Destroy(_controledMinion.gameObject);
        }
        _controledMinion = Instantiate(_minionPrefab, spawnPoint.position,Quaternion.identity,null).GetComponent<SimpleMinionBehaviour>();
    }

    private void Update()
    {
        if(IsAlive && !_controledMinion.IsJumping)
        {
            CalculateMove();
        }

    }

    private void CalculateMove()
    {
        var value = _neuralNetwork.CalculateNeuralNetworkValue(_controledMinion.GetDistanceToNextObstacle());
        Debug.Log(value);
        if (value >= 1)
            _controledMinion.Jump();
    }

}
