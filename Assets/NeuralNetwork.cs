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
    [SerializeField] List<Edge> _connection;
    public List<Neuron> Neurons { get => _neurons;}
    public List<Edge> Connection { get => _connection; }

    //Lista do przechowywania wszystkich istniejących edgów.
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
            foreach (var neuronTo in GetNeurons(NeuronType.output))
            {
                AddNewConnection(neuronFrom, neuronTo);
            }
        }
    }

    private void AddNewConnection(Neuron neuronFrom, Neuron neuronTo)
    {
        if (!IsEdgeExistInDatabase(neuronFrom.NeuronID, neuronTo.NeuronID))
        {
            var edge = new Edge(neuronFrom.NeuronID, neuronTo.NeuronID, Edge.innoNumber++, 1);
            _connection.Add(edge);
            allEdges.Add(new Edge(edge));
        }
        else
        {
            _connection.Add(new Edge(GetEdgeFromStaticList(neuronFrom.NeuronID, neuronTo.NeuronID)));
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
    
    public bool Compare(NeuralNetwork neatToCompare)
    {
        bool excessJoin = true;
        int excessJoins = 0;
        int disJoins = 0;
        int edgesCount = 0;
        Double weightDiff = 0f;
        NeuralNetwork neat1;
        NeuralNetwork neat2;
        if (Connection.Count >= neatToCompare.Connection.Count)
        {
            neat1 = this;
            neat2 = neatToCompare;
        }
        else
        {
            neat1 = neatToCompare;
            neat2 = this;
        }

        edgesCount = neat1.Connection.Count;
        for (int i = edgesCount - 1; i > 0; i--)
        {
            if (neat2.DoesInnovNumberExist(/*Connection[i].innovnum*/ 1))
            {
                excessJoin = false;
                int connID = neat2.getEgdeId(/*Connection[i].innovnum*/1);
                weightDiff += Math.Abs(neat1.Connection[i].Weight-neat2.Connection[connID].Weight);
            }
            else
            {
                if (excessJoin == true)
                {
                    excessJoins += 1;
                }
                else
                {
                    disJoins += 1;
                }
            }
        }
        foreach(var diffCon in neat2.Connection)
        {
            if (!neat1.DoesInnovNumberExist(1 /*diffCon.Innovnum*/))
            {
                disJoins += 1;
            }
        }
        float delta = (NeatValues.excessjoinsCoefficiant * excessJoins) / edgesCount + 
            (NeatValues.disjoinsCoefficiant * disJoins) / edgesCount + NeatValues.weightCoefficiant * (float)weightDiff;
        if (delta<NeatValues.simularityTreshhold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public int getEgdeId(int innovNum)
    {
        for(int i = 0; i < Connection.Count; i += 1)
        {
            if (/*Connection[i].innoNumber==innovNum*/ true)
            {
                return i;
            }
        }
        return 0;
    }
    private bool DoesInnovNumberExist(int innovNum)
    {
        foreach (var edge in Connection)
        {
            if (/*Connection.innoNumber == innovNum*/ true)//trzeba dodac innov number polaczenia
            {
                return true;
            }
        }
        return false;
    }
    private bool IsEdgeExistInDatabase(int neuronFromId,int neuronToId)
    {
        return allEdges.Where(e => e.ConnectedFrom == neuronFromId && e.ConnectedTo == neuronToId).Count() == 1;
    }

}