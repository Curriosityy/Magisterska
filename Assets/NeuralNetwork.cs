using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class NeuralNetwork
{
    private int _maxLevel = 1;
    int _neuronCounter = 0;
    [SerializeField] List<Neuron> _neurons;
    [SerializeField] List<Edge> _connections;
    public List<Neuron> Neurons { get => _neurons;}
    public List<Edge> Connection { get => _connections; }

    //Lista do przechowywania wszystkich istniejących edgów.
    public static List<Edge> allEdges = new List<Edge>();

    public void MutateNeuralNetwork()
    {
        MutateConnections();
        MutateNeurons();

    }

    private void MutateNeurons()
    {
        CreateNewNeurons();
    }

    private void CreateNewNeurons()
    {
        foreach (var edge in GetEnabledConnections())
        {
            if(GetRand()<= NeatValues.addNodeProbability)
            {
                edge.IsActivated = false;
                if (!IsLevelGapBetweenConnectedNeuronsHigherThanOne(edge))
                {
                    LevelUpAllneuronsHighterThanLevel(GetNeuronOfId(edge.ConnectedFrom).Level);
                }
                var newNeuronId = _neuronCounter++;
                var newNeuron = new Neuron(newNeuronId, NeuronType.hidden, GetNeuronOfId(edge.ConnectedFrom).Level + 1);
                _neurons.Add(newNeuron);
                AddNewConnection(GetNeuronOfId(edge.ConnectedFrom), newNeuron);
                AddNewConnection(newNeuron, GetNeuronOfId(edge.ConnectedTo));
            }
        }
    }

    private bool IsLevelGapBetweenConnectedNeuronsHigherThanOne(Edge edge)
    {
        return GetNeuronOfId(edge.ConnectedFrom).Level != GetNeuronOfId(edge.ConnectedTo).Level - 1;
    }

    private void MutateConnections()
    {
        DisableConnections();
        MakeConnections();
        MutateWeights();
    }

    private void MutateWeights()
    {
        var rand = GetRand();
        foreach (var connection in GetEnabledConnections())
        {
            if (rand <= NeatValues.weightMutationProbability)
            {
                MutateWeight(connection);
            }
            else if (rand <= NeatValues.weightMutationProbability + NeatValues.weightRandomMutationProbability)
            {
                RandomizeWeight(connection);
            }
        }
    }

    private static float GetRand(float x=0f, float y=1f)
    {
        return UnityEngine.Random.Range(x, y);
    }

    private void RandomizeWeight(Edge connection)
    {
        connection.Weight = GetRand(NeatValues.minWeight, NeatValues.maxWeight);
    }

    private void MutateWeight(Edge connection)
    {
        connection.Weight += (GetRand(NeatValues.minWeight, NeatValues.maxWeight)/3);
    }

    private void MakeConnections()
    {
        for (int i = 0; i <= _maxLevel; i++)
        {
            foreach (var fromNeuron in GetNeuronOfLevel(i))
            {
                foreach (var toNeuron in GetNeuronWithHigherLevel(i))
                {
                    if (GetRand() <= NeatValues.addConnProbability)
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
            if(GetRand() <= NeatValues.removeConnProbability)
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
                if(GetRand() <= NeatValues.addNodeProbability)
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

    private void LevelUpAllneuronsHighterThanLevel(int level)
    {
        foreach(var neuron in GetNeuronWithHigherLevel(level))
        {
            neuron.LevelUp(ref _maxLevel);
        }
    }

    private Neuron GetNeuronOfId(int id)
    {
        return _neurons.Where(n => n.NeuronID == id).First();
    }


}