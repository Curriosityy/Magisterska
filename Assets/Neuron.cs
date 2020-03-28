using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum NeuronType
{
    input,
    hidden,
    output
}
[Serializable]
public class Neuron
{
    int _level;
    float _bias=1;
    [SerializeField]
    int _neuronID;
    [SerializeField]
    NeuronType _neuronType;
    public int NeuronID { get => _neuronID; set => _neuronID = value; }
    public NeuronType Type { get => _neuronType; set => _neuronType = value; }
    public int Level { get => _level;}
    public float Bias { get => _bias; set => _bias = value; }

    public void LevelUp(ref int maxLevel)
    {
        _level++;
        if(maxLevel < _level)
        {
            maxLevel = _level;
        }
    }
    public Neuron(int neuronID, NeuronType neuronType,int level)
    {
        _neuronID = neuronID;
        _neuronType = neuronType;
        _level = level;
        _bias = 1;
    }

}

