using System.Collections.Generic;

public class NeuralNetwork
{
    List<Neuron> _neurons;
    List<Edge> _connection;
    public List<Neuron> Neurons { get => _neurons; set => _neurons = value; }
    public List<Edge> Connection { get => _connection; set => _connection = value; }


    public NeuralNetwork()
    {
    }
    
}