using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class NeuralNetwork
{
    int a = 0;
    static System.Random _rand = new System.Random();
    public int _maxLevel = 1;
    private float _fitness=0;
    private float _adjustedFitness=0;
    int _neuronCounter = 0;
    private int _generation = 0;
    [SerializeField] List<Neuron> _neurons;
    [SerializeField] List<Edge> _connections;
    Color _color;
    //trzeba dodac polaczenia poczatkowe
    public List<Neuron> Neurons { get => _neurons;}
    
    
    public int NeuronCounter { get => _neuronCounter; set => _neuronCounter = value; }
    public int MaxLevel { get => _maxLevel; }
    public List<Edge> Connection { get => _connections; }
    public float AdjustedFitness { get => _adjustedFitness; set => _adjustedFitness = value; }
    public float Fitness { get => _fitness; set => _fitness = value; }
    public int Generation { get => _generation; set => _generation = value; }
    public Color Color { get => _color;}
    



    //Lista do przechowywania wszystkich istniejących edgów.
    public static List<Edge> allEdges = new List<Edge>();
 

    public float[] CalculateNeuralNetworkValue(float[] input)
    {
        ClearNeuronValues();
        List<Edge> edges;
        List<Neuron> neurons;
        SetInputValues(input);
        for(int i=0;i<_maxLevel;i++)
        {
            neurons = GetNeuronOfLevel(i);
            if(i>0 && neurons.Count>0)
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
        return SetOutputValues();

    }

    private void ClearNeuronValues()
    {
        _neurons.ForEach(n => n.Value = 0);
    }
    //zmienilem
    private float[] SetOutputValues()
    {
        float[] output = new float[NeatValues.outputNeuronSize];
        var neurons = GetNeurons(NeuronType.output);
        //Debug.Log("used activation on=" + neurons[0].Type);
        neurons.ForEach(n => n.UseActivationFunction());
        for(int i=0;i<NeatValues.outputNeuronSize; i++)
        {
            output[i] = neurons[i].Value;
        }
        return output;
    }

    private void SetInputValues(float[] input)
    {
        GetNeurons(NeuronType.input).ForEach(n => n.Value = input[n.NeuronID]);
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
        float rand;
        foreach (var neuron in _neurons)
        {
            rand = (float)_rand.NextDouble();
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
        neuron.Bias = Random(NeatValues.minBias, NeatValues.maxBias);
    }

    private void MutateBias(Neuron neuron)
    {
        neuron.Bias += Random(NeatValues.minBias, NeatValues.maxBias)/10;
    }

    private void DeleteNeurons()
    {
        
        if (Random() <= NeatValues.removeNodeProbability)
        {
            var hiddenNeuronList = GetNeurons(NeuronType.hidden);
            if(hiddenNeuronList.Count>0)
            {
                RemoveNeuronOfId(hiddenNeuronList[UnityEngine.Random.Range(0, hiddenNeuronList.Count)].NeuronID);
            }
            
        }
       
       
    }

    private void RemoveNeuronOfId(int id)
    {
        _neurons.Remove(GetNeuronOfId(id));
        _connections.RemoveAll(e => e.ConnectedTo == id);
        _connections.RemoveAll(e => e.ConnectedFrom == id);
    }

    private void CreateNewNeurons()
    {
        if (Random() <= NeatValues.addNodeProbability)
        {
            var enabledConn = GetEnabledConnections();
            if(enabledConn.Count>0)
            {
                var edge = enabledConn[UnityEngine.Random.Range(0, enabledConn.Count)];
                edge.IsActivated = false;
                if (!IsLevelGapBetweenConnectedNeuronsHigherThanOne(edge))
                {
                    LevelUpAllneuronsHighterThanLevel(GetNeuronOfId(edge.ConnectedFrom).Level);
                }
                var newNeuronId = _neuronCounter++;
                var newNeuron = new Neuron(newNeuronId, NeuronType.hidden, GetNeuronOfId(edge.ConnectedFrom).Level + 1,Random(NeatValues.minBias,NeatValues.maxBias));
                _neurons.Add(newNeuron);
                AddNewConnection(GetNeuronOfId(edge.ConnectedFrom), newNeuron);
                AddNewConnection(newNeuron, GetNeuronOfId(edge.ConnectedTo));
            }
            
        }
    }

    private bool IsLevelGapBetweenConnectedNeuronsHigherThanOne(Edge edge)
    {
        if(GetNeuronOfId(edge.ConnectedFrom)==null)
        {
            GetNeuronOfId(edge.ConnectedFrom);
        }
        if(GetNeuronOfId(edge.ConnectedTo)==null)
        {
            GetNeuronOfId(edge.ConnectedTo);
        }


        return GetNeuronOfId(edge.ConnectedFrom).Level != GetNeuronOfId(edge.ConnectedTo).Level - 1;
    }


    private void MutateWeights()
    {
        float rand;
        foreach (var connection in GetEnabledConnections())
        {
            rand = Random();
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


    private void RandomizeWeight(Edge connection)
    {
        connection.Weight = Random(NeatValues.minWeight, NeatValues.maxWeight);
    }

    private void MutateWeight(Edge connection)
    {
        connection.Weight += (Random(NeatValues.minWeight, NeatValues.maxWeight)/10);
    }

    //private void MakeConnections()
    //{
    //    for (int i = 0; i <= _maxLevel; i++)
    //    {
    //        foreach (var fromNeuron in GetNeuronOfLevel(i))
    //        {
    //            foreach (var toNeuron in GetNeuronWithHigherLevel(i))
    //            {
    //                if (Random() <= NeatValues.addConnProbability)
    //                {
    //                    AddNewConnection(fromNeuron, toNeuron);
    //                }
    //            }
    //        }
    //    }
    //}

    private void MakeConnections()
    {
        if (Random() <= NeatValues.addConnProbability)
        {
            var nodes = GetNeuronsWithOutType(NeuronType.output);
            var neuronFrom = nodes[UnityEngine.Random.Range(0, nodes.Count)];
            nodes = GetNeuronWithHigherLevel(neuronFrom.Level);
            var neuronTo = nodes[UnityEngine.Random.Range(0, nodes.Count)];
            AddNewConnection(neuronFrom,neuronTo);
        }
    }

    private void DisableConnections()
    {
        int rndConn = NeatValues.rnd.Next(_connections.Count);
        if (Random() < NeatValues.changeConnStatusProbability)
        {
            if(_connections[rndConn].IsActivated == true)
            {
                _connections[rndConn].IsActivated = false;
            }
            else
            {
                _connections[rndConn].IsActivated = true;
            }
        }
    }

    public NeuralNetwork()
    {
        _generation = NeatValues.GenerationCount;
        _neurons = new List<Neuron>();
        _connections = new List<Edge>();
        for(int i=0;i<NeatValues.inputNeuronSize;i++)
        {
            _neurons.Add(new Neuron(_neuronCounter++, NeuronType.input,0));
        }
        for(int i = 0; i < NeatValues.outputNeuronSize; i++)
        {
            _neurons.Add(new Neuron(_neuronCounter++, NeuronType.output,1));
        }
        foreach (var neuronFrom in GetNeurons(NeuronType.input)){
            foreach (var neuronTo in GetNeurons(NeuronType.output))
            {
                //if(_rand.Next(0,100) <= NeatValues.addNodeProbability*100)
                    AddNewConnection(neuronFrom, neuronTo);
            }
        }
        _color = Color.blue;
    }

    public NeuralNetwork(NeuralNetwork networkToCopy)
    {
        _generation = NeatValues.GenerationCount;
        _neurons = new List<Neuron>();
        _connections = new List<Edge>();
        _neuronCounter = networkToCopy.NeuronCounter;
        _maxLevel = networkToCopy._maxLevel;
        foreach (var neuron in networkToCopy.Neurons)
        {
            _neurons.Add(new Neuron(neuron));
        }
        foreach (var connection in networkToCopy.Connection)
        {
            _connections.Add(new Edge(connection));
        }
        _color = Color.white;
    }

    public NeuralNetwork(NeuralNetwork networkToCopy,Color color)
    {
        _generation = NeatValues.GenerationCount;
        _neurons = new List<Neuron>();
        _connections = new List<Edge>();
        _neuronCounter = networkToCopy.NeuronCounter;
        _maxLevel = networkToCopy._maxLevel;
        foreach (var neuron in networkToCopy.Neurons)
        {
            _neurons.Add(new Neuron(neuron));
        }
        foreach (var connection in networkToCopy.Connection)
        {
            _connections.Add(new Edge(connection));
        }
        _color = Color.red;
    }





    private void AddNewConnection(Neuron neuronFrom, Neuron neuronTo)
    {
        if (!IsEdgeExistInDatabase(neuronFrom.NeuronID, neuronTo.NeuronID))
        {
            var edge = new Edge(neuronFrom.NeuronID, neuronTo.NeuronID, Edge.innoNumber++, Random(NeatValues.minWeight, NeatValues.maxWeight));
            _connections.Add(edge);
            allEdges.Add(new Edge(edge));
        }
        else if(IsEdgeExistInThisNeuralNetwork(neuronFrom.NeuronID, neuronTo.NeuronID))
        {
            GetConnection(neuronFrom.NeuronID, neuronTo.NeuronID).IsActivated = true;
        }
        else
        {
            _connections.Add(new Edge(GetEdgeFromStaticList(neuronFrom.NeuronID, neuronTo.NeuronID), Random(NeatValues.minWeight, NeatValues.maxWeight)));
        }
    }

    private static float Random(float min=0,float max=1)
    {
        return min + (float)_rand.NextDouble() * (max - min);
    }

    private Edge GetEdgeFromStaticList(int neuronFromId, int neuronToId)
    {
        return allEdges.FirstOrDefault(e => e.ConnectedFrom == neuronFromId && e.ConnectedTo == neuronToId);
    }

    private List<Neuron> GetNeurons(NeuronType type)
    {
        return _neurons.Where(n => n.Type == type).ToList();
    }
    public int GetInnovRange()
    {
        int range = 0;
        foreach(var conn in Connection)
        {
            if (conn.Id > range)
            {
                range = conn.Id;
            }
        }
        return range;
    }
    
    public int GetExcessJoins(NeuralNetwork neatToCompare)
    {
        int range = neatToCompare.GetInnovRange();
        int excessJoins = 0;
        foreach(var conn in Connection)
        {
            if (conn.Id > range)
            {
                excessJoins += 1;
            }
        }
        return excessJoins;
    }
    public int GetDisJoins(NeuralNetwork neatToCompare)
    {
        int range = neatToCompare.GetInnovRange();
        int disJoins = 0;
        foreach (var conn in Connection)
        {
            if (!neatToCompare.DoesInnovNumberExist(conn.Id))
            {
                if (conn.Id <= range)
                {
                    disJoins += 1;
                }
            }
           
        }
        return disJoins;
    }
    public float GetWeightDiff(NeuralNetwork neatToCompare)
    {
        float weightDiff = 0;
        foreach (var conn in Connection)
        {
            if (neatToCompare.DoesInnovNumberExist(conn.Id))
            {
                int connID = neatToCompare.getEgdeId(conn.Id);
                weightDiff += Math.Abs(conn.Weight - neatToCompare.Connection[connID].Weight);
            }
        }
        return weightDiff;
    }
    public int getMathingEdges(NeuralNetwork neatToCompare)
    {
        int edges = 0;
        foreach (var conn in Connection)
        {
            if (neatToCompare.DoesInnovNumberExist(conn.Id))
            {
                edges += 1;
            }
        }
        return edges;
    }
    public bool Compare(NeuralNetwork neatToCompare)
    {
       
        int excessJoins = 0;
        int disJoins = 0;
        int edgesCount = 0;
        Double weightDiff = 0f;
        if (GetInnovRange() >= neatToCompare.GetInnovRange())
        {
            excessJoins= GetExcessJoins(neatToCompare);
        }
        else
        {
            excessJoins = neatToCompare.GetExcessJoins(this);
        }
        disJoins += neatToCompare.GetDisJoins(this);
        disJoins += GetDisJoins(neatToCompare);
        if (neatToCompare.Connection.Count >= Connection.Count)
        {
            edgesCount = neatToCompare.getMathingEdges(this);
            weightDiff = neatToCompare.GetWeightDiff(this);
        }
        else
        {
            edgesCount = getMathingEdges(neatToCompare);
            weightDiff = GetWeightDiff(neatToCompare);
        }
       
       /* NeuralNetwork neat1;
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
            if (neat2.DoesInnovNumberExist(neat1.Connection[i].Id))
            {
                excessJoin = false;
                int connID = neat2.getEgdeId(neat1.Connection[i].Id);
                weightDiff += Math.Abs(neat1.Connection[i].Weight - neat2.Connection[connID].Weight);
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
        foreach (var diffCon in neat2.Connection)
        {
            if (!neat1.DoesInnovNumberExist(diffCon.Id))
            {
                disJoins += 1;
            }
        }*/
        float delta = (NeatValues.excessjoinsCoefficiant * excessJoins) / edgesCount +
            (NeatValues.disjoinsCoefficiant * disJoins) / edgesCount + NeatValues.weightCoefficiant * (float)weightDiff;
        //Debug.Log("delta" + delta +"edges "+edgesCount+ " excess joins "+excessJoins+ " wynik "+ (NeatValues.excessjoinsCoefficiant * excessJoins) / edgesCount + " disJoins "+ disJoins+ " wynik " + (NeatValues.disjoinsCoefficiant * disJoins) / edgesCount+ " weight diff "+weightDiff+" wynik " + NeatValues.weightCoefficiant * (float)weightDiff);
        if (delta < NeatValues.simularityTreshhold)
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

    private List<Neuron> GetNeuronsWithOutType(NeuronType type)
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