using System;
using System.Collections.Generic;
using System.Linq;
public class NeuralNetwork
{
    int _neuronCounter = 0;
    int _neuronInputSize = 20;
    int _neuronOutputSize = 2;
    List<Neuron> _neurons;
    List<Edge> _connection;
    public List<Neuron> Neurons { get => _neurons;}
    public List<Edge> Connection { get => _connection; }
    public static List<Edge> allEdges = new List<Edge>();

    public NeuralNetwork()
    {
        _neurons = new List<Neuron>();
        _connection = new List<Edge>();
        for(int i=0;i<_neuronInputSize;i++)
        {
            _neurons.Add(new Neuron(_neuronCounter++, NeuronType.input));
        }
        for(int i = 0; i < _neuronOutputSize; i++)
        {
            _neurons.Add(new Neuron(_neuronCounter++, NeuronType.output));
        }

        foreach(var neuronFrom in GetNeurons(NeuronType.input)){
            foreach (var neuronTo in GetNeurons(NeuronType.output)){
                if(!IsEdgeExist(neuronFrom.NeuronID, neuronTo.NeuronID))
                {
                    var edge = new Edge(neuronFrom.NeuronID, neuronTo.NeuronID, Edge.innoNumber++,1);
                    _connection.Add(edge);
                    allEdges.Add(edge);
                }
                else
                {
                    _connection.Add(GetEdgeFromStaticList(neuronFrom.NeuronID, neuronTo.NeuronID));
                }
            }
        }
    }

    private Edge GetEdgeFromStaticList(int neuronFromId, int neuronToId)
    {
        return allEdges.Where(e => e.ConnectedFrom == neuronFromId && e.ConnectedTo == neuronToId).First();
    }

    private List<Neuron> GetNeurons(NeuronType type)
    {
        return _neurons.Where(n => n.Type == type).ToList();
    }

    private bool IsEdgeExist(int neuronFromId,int neuronToId)
    {
        return allEdges.Where(e => e.ConnectedFrom == neuronFromId && e.ConnectedTo == neuronToId).Count() == 1;
    }

}