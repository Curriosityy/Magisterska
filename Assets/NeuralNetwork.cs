﻿using System;
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
        throw new NotImplementedException();
    }

    private void DisableConnections()
    {
        foreach(var connection in _connections)
        {
            if(UnityEngine.Random.Range(0f,1f)>NeatValues.removeConnProbability)
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
                if(UnityEngine.Random.Range(0f,1f)> NeatValues.addNodeProbability)
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
        return allEdges.Where(e => e.ConnectedFrom == neuronFromId && e.ConnectedTo == neuronToId).Count() == 1;
    }



}