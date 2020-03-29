using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class NeuralNetwork
{
    private int _maxLevel = 1;
    private float _fitness=0;
    private float _adjustedFitness=0;
    int _neuronCounter = 0;
    private int _generation = 0;
    [SerializeField] List<Neuron> _neurons;
    [SerializeField] List<Edge> _connections;
    public List<Neuron> Neurons { get => _neurons;}

    
    public int NeuronCounter { get => _neuronCounter; set => _neuronCounter = value; }

    public List<Edge> Connection { get => _connections; }
    public float AdjustedFitness { get => _adjustedFitness; set => _adjustedFitness = value; }
    public float Fitness { get => _fitness; set => _fitness = value; }
    public int Generation { get => _generation; set => _generation = value; }


    //Lista do przechowywania wszystkich istniejących edgów.
    public static List<Edge> allEdges = new List<Edge>();
 

    public void CalculateNeuralNetworkValue(int []inputs,out float[] outputs)
    {
        ClearNeuronValues();
        List<Edge> edges;
        List<Neuron> neurons;
        SetInputValues(inputs);
        for(int i=0;i<_maxLevel;i++)
        {
            neurons = GetNeuronOfLevel(i);
            if(i>0)
            {
                neurons.ForEach(n => n.UseActivationFunction());
            }
            foreach(var neuron in neurons)
            {
                edges = GetAllEnabledConnectionFromNeuron(neuron.NeuronID);
                foreach(var edge in edges)
                {
                    GetNeuronOfId(edge.ConnectedTo).SumValue(neuron.Value,edge.Weight);
                }
            }
        }
        SetOutputValues(out outputs);

    }

    private void ClearNeuronValues()
    {
        _neurons.ForEach(n => n.Value = 0);
    }

    private void SetOutputValues(out float[] outputs)
    {
        outputs = new float[NeatValues.outputNeuronSize];
        var neurons = GetNeurons(NeuronType.output);
        neurons.ForEach(n => n.UseActivationFunction());
        for(int i=0;i< outputs.Length; i++)
        {
            outputs[i] = neurons[i].Value;
        }
    }

    private void SetInputValues(int[] inputs)
    {
        GetNeurons(NeuronType.input).ForEach(n => n.Value = inputs[n.NeuronID]);
    }

    public void MutateNeuralNetwork()
    {
        MutateConnections();
        MutateNeurons();
    }

    private void MutateNeurons()
    {
        DeleteNeurons();
        CreateNewNeurons();
        MutateBias();
    }
    private void MutateConnections()
    {
        DisableConnections();
        MakeConnections();
        MutateWeights();
    }

    private void MutateBias()
    {
        var rand = GetRand();
        foreach (var neuron in _neurons)
        {
            if (rand <= NeatValues.biasMutationProbability)
            {
                MutateBias(neuron);
            }
            else if (rand <= NeatValues.biasMutationProbability + NeatValues.biasRandomMutationProbability)
            {
                RandomizeBias(neuron);
            }
        }
    }

    private void RandomizeBias(Neuron neuron)
    {
        neuron.Bias = GetRand(NeatValues.minBias, NeatValues.maxBias);
    }

    private void MutateBias(Neuron neuron)
    {
        neuron.Bias += GetRand(NeatValues.minBias, NeatValues.maxBias)/3;
    }

    private void DeleteNeurons()
    {
        var hiddenNeuronList = GetNeurons(NeuronType.hidden);
        for(int i = 0;i<hiddenNeuronList.Count;i++)
        {
            if (GetRand() <= NeatValues.removeNodeProbability)
            {
                RemoveNeuronOfId(hiddenNeuronList[i].NeuronID);
            }
        }
    }

    private void RemoveNeuronOfId(int id)
    {
        _neurons.Remove(GetNeuronOfId(id));
        _connections.RemoveAll(e => e.ConnectedTo == id);
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
        var rand = new System.Random();
        foreach(var neuronFrom in GetNeurons(NeuronType.input)){
            foreach (var neuronTo in GetNeurons(NeuronType.output))
            {
                if(rand.Next(0,100) <= NeatValues.addNodeProbability*100)
                    AddNewConnection(neuronFrom, neuronTo);
            }
        }
        
    }
    public NeuralNetwork(NeuralNetwork networkToCopy)
    {
        _neurons = new List<Neuron>();
        _connections = new List<Edge>();
        _neuronCounter = networkToCopy.NeuronCounter;
        foreach (var neuron in networkToCopy.Neurons)
        {
            _neurons.Add(new Neuron(neuron));
        }
        foreach (var connection in networkToCopy.Connection)
        {
            _connections.Add(new Edge(connection));
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
        else if(IsEdgeExistInThisNeuralNetwork(neuronFrom.NeuronID, neuronTo.NeuronID))
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
        return allEdges.FirstOrDefault(e => e.ConnectedFrom == neuronFromId && e.ConnectedTo == neuronToId);
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
            if (neat2.DoesInnovNumberExist(Connection[i].Id))
            {
                excessJoin = false;
                int connID = neat2.getEgdeId(Connection[i].Id);
                weightDiff += Math.Abs(neat1.Connection[i].Weight-neat2.Connection[connID].Weight);
            }
            else
            {
                if (excessJoin)
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
            if (!neat1.DoesInnovNumberExist(diffCon.Id))
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
            if (Connection[i].Id == innovNum)
            {
                return i;
            }
        }
        return 0;
    }
    public bool DoesInnovNumberExist(int innovNum)
    {
        foreach (var edge in Connection)
        {
            if (edge.Id == innovNum)
            {
                return true;
            }
        }
        return false;
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
        return _connections.FirstOrDefault(e => e.ConnectedFrom == neuronFrom && e.ConnectedTo == neuronTo);
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
        return _neurons.FirstOrDefault(n => n.NeuronID == id);
    }

    private List<Edge> GetAllConnectionFromNeuron(int id)
    {
        return _connections.Where(e => e.ConnectedFrom == id).ToList();
    }
    private List<Edge> GetAllEnabledConnectionFromNeuron(int id)
    {
        return _connections.Where(e => e.ConnectedFrom == id && e.IsActivated==true).ToList();
    }



}