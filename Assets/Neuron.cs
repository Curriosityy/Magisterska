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

    private static int _maxLevel=0;
    int _level;
    float _bias;
    [SerializeField]
    int _neuronID;
    [SerializeField]
    NeuronType _neuronType;
    public int NeuronID { get => _neuronID; set => _neuronID = value; }
    public NeuronType Type { get => _neuronType; set => _neuronType = value; }
    public int Level { get => _level;}
    public static int MaxLevel { get => _maxLevel; }

    public void LevelUp()
    {
        _level++;
        if(MaxLevel<_level)
        {
            _maxLevel = _level;
        }
    }
    public Neuron(int neuronID, NeuronType neuronType,int level)
    {
        _neuronID = neuronID;
        _neuronType = neuronType;
        _level = level;
        if(MaxLevel<level)
        {
            _maxLevel = level;
        }
    }

}

