using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControler : MonoBehaviour
{
    private NeuralNetwork _neuralNetwork;

    public NeuralNetwork NeuralNetwork { get => _neuralNetwork; }

    private void Awake()
    {
        _neuralNetwork = new NeuralNetwork();
    }
}
