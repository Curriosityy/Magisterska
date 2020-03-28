using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class NeuralNetwork
{
    int _neuronCounter = 0;
    int _neuronInputSize = 20;
    int _neuronOutputSize = 2;
    [SerializeField] List<Neuron> _neurons;
    [SerializeField] List<Edge> _connections;
    public List<Neuron> Neurons { get => _neurons;}
    public List<Edge> Connection { get => _connections; }

    //Lista do przechowywania wszystkich istniejących edgów.
    public static List<Edge> allEdges = new List<Edge>();


    public void MutateConnections()
    {
        DisableConnections();
        MakeConnections();
        
    }

    private void MakeConnections()
    {
        for (int i = 0; i <= Neuron.MaxLevel; i++)
        {
            foreach (var fromNeuron in GetNeuronOfLevel(i))
            {
                foreach (var toNeuron in GetNeuronWithHigherLevel(i))
                {
                    if (UnityEngine.Random.Range(0f, 1f) <= NeatValues.addConnProbability)
                    {
                        AddNewConnection(fromNeuron, toNeuron);
                    }
                }
            }
        }
    }

    private void DisableConnections()
    {
        foreach(var connection in _connections)
        {
            if(UnityEngine.Random.Range(0f,1f) <= NeatValues.removeConnProbability)
            {
                connection.IsActivated = false;
            }
        }
    }

    public NeuralNetwork()
    {
        _neurons = new List<Neuron>();
        _connections = new List<Edge>();
        for(int i=0;i<NeatValues.inputNeutonSize;i++)
        {
            _neurons.Add(new Neuron(_neuronCounter++, NeuronType.input,0));
        }
        for(int i = 0; i < NeatValues.outputNeuronSize; i++)
        {
            _neurons.Add(new Neuron(_neuronCounter++, NeuronType.output,1));
        }

        foreach(var neuronFrom in GetNeurons(NeuronType.input)){
            foreach (var neuronTo in GetNeurons(NeuronType.output))
            {
                if(UnityEngine.Random.Range(0f,1f) <= NeatValues.addNodeProbability)
                    AddNewConnection(neuronFrom, neuronTo);
            }
        }
        
    }

    private void AddNewConnection(Neuron neuronFrom, Neuron neuronTo)
    {
        if (!IsEdgeExistInDatabase(neuronFrom.NeuronID, neuronTo.NeuronID))
        {
            var edge = new Edge(neuronFrom.NeuronID, neuronTo.NeuronID, Edge.innoNumber++, 1);
            _connections.Add(edge);
            allEdges.Add(new Edge(edge));
        }
        else if(!IsEdgeExistInThisNeuralNetwork(neuronFrom.NeuronID, neuronTo.NeuronID))
        {
            GetConnection(neuronFrom.NeuronID, neuronTo.NeuronID).IsActivated = true;
        }
        else
        {
            _connections.Add(new Edge(GetEdgeFromStaticList(neuronFrom.NeuronID, neuronTo.NeuronID)));
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

    private List<Neuron> GetRestNeurons(NeuronType type)
    {
        return _neurons.Where(n => n.Type != type).ToList();
    }

    private bool IsEdgeExistInDatabase(int neuronFromId,int neuronToId)
    {
        return allEdges.Where(e => e.ConnectedFrom == neuronFromId && e.ConnectedTo == neuronToId).Count() >= 1;
    }

    private bool IsEdgeExistInThisNeuralNetwork(int neuronFrom, int neuronTo)
    {
        return _connections.Where(e => e.ConnectedFrom == neuronFrom && e.ConnectedTo == neuronTo).Count() >= 1;
    }

    private Edge GetConnection(int neuronFrom, int neuronTo)
    {
        return _connections.Where(e => e.ConnectedFrom == neuronFrom && e.ConnectedTo == neuronTo).First();
    }

    private List<Neuron> GetNeuronOfLevel(int level)
    {
        return _neurons.Where(n => n.Level == level).ToList();
    }

    private List<Neuron> GetNeuronWithHigherLevel(int level)
    {
        return _neurons.Where(n => n.Level > level).ToList();
    }

    private List<Edge> GetEnabledConnections()
    {
        return _connections.Where(e => e.IsActivated == true).ToList();
    }


}