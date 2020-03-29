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
    [SerializeField]
    int _neuronID;
    [SerializeField]
    NeuronType _neuronType;
    public int NeuronID { get => _neuronID; set => _neuronID = value; }
    public NeuronType Type { get => _neuronType; set => _neuronType = value; }
    public Neuron(Neuron neuronToCopy)
    {
        _neuronID = neuronToCopy.NeuronID;
        _neuronType = neuronToCopy.Type;
    }
   
    public Neuron(int neuronID, NeuronType neuronType)
    {
        _neuronID = neuronID;
        _neuronType = neuronType;
    }

}

