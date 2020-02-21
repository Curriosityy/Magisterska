using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum NeuronType
{
    input,
    hidden,
    output
}
public class Neuron
{
    int _neuronID;
    NeuronType _neuronType;
    public int NeuronID { get => _neuronID; set => _neuronID = value; }
    public NeuronType Type { get => _neuronType; set => _neuronType = value; }
   
    public Neuron(int neuronID, NeuronType neuronType)
    {
        _neuronID = neuronID;
        _neuronType = neuronType;
    }

}

