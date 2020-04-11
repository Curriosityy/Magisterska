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
    float _value=0;
    [SerializeField]
    int _level;
    [SerializeField]
    float _bias=1;
    [SerializeField]
    int _neuronID;
    [SerializeField]
    NeuronType _neuronType;
    public int NeuronID { get => _neuronID; set => _neuronID = value; }
    public NeuronType Type { get => _neuronType; set => _neuronType = value; }
    public int Level { get => _level; set => _level = value; }
    public float Bias { get => _bias; set => _bias = value; }
    public float Value { get => _value; set => _value = value; }
    
    public void SumValue(float value,float weight)
    {
        _value += (value * weight);
    }

    public void UseActivationFunction()
    {
        _value = Mathf.Max(0, _value + _bias);
        //switch (_neuronType)
        //{
        //    case NeuronType.hidden:
                
        //        break;
        //    case NeuronType.output:
        //        _value = (_value + _bias) * NeatValues.linearActivFunValue;
        //        break;
        //}

    }

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
    public Neuron(int neuronID, NeuronType neuronType, int level,float bias)
    {
        _neuronID = neuronID;
        _neuronType = neuronType;
        _level = level;
        _bias = bias;
    }
    public Neuron(Neuron neuronToCopy)
    {
        _neuronID = neuronToCopy.NeuronID;
        _neuronType = neuronToCopy.Type;
        _level = neuronToCopy.Level;
        _bias = neuronToCopy.Bias;
    }
    public Neuron()
    {
       
    }


}

