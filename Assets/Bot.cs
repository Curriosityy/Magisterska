using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot
{
    private int _position = 200;
    private int _distance = 200;
    private bool _isAlive = true;
    private int _fitness = 0;
    private NeuralNetwork _brain;

    public Bot()
    {
        _position = 200;
        _distance = 200;
        _isAlive = true;
        _fitness = 0;
    }

    public bool IsAlive { get => _isAlive; set => _isAlive = value; }
    public int Distance { get => _distance; set => _distance = value; }
    public int Position { get => _position; set => _position = value; }
    public int Fitness { get => _fitness; set => _fitness = value; }
    public NeuralNetwork Brain { get => _brain; set => _brain = value; }

    // Start is called before the first frame update

 
    public void CheckPos()
    {
        
        if (Position>400 || Position < 0)
        {
            IsAlive = false;
            
            if (NeatValues.BestFitness < Brain.Fitness)
            {
                NeatValues.BestFitness = Brain.Fitness;
            }
        }
        else
        {
             Fitness += 1;
             Position += 1;
             Distance = 400 - Position;
             float neatres=Brain.CalculateNeuralNetworkValue(22);
           
            Debug.Log("Nearest: "+neatres);
            if (neatres == 1)
            {
                Jump();
            }
        }

    }
    public void Jump()
    {
        Position -= 50;
    }
    // Update is called once per frame
    
    
}
