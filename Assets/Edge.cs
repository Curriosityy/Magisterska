using System;
using UnityEngine;

[Serializable]
public class Edge
{
    [SerializeField]
    int _connectedFrom;
    [SerializeField]
    int _connectedTo;
    [SerializeField]
    int _id;
    public static int innoNumber = 0;
    [SerializeField]
    double _weight;
    [SerializeField]
    private bool _isActivated;

    public Edge(int connectedFrom, int connectedTo, int id, double weight)
    {
        _connectedFrom = connectedFrom;
        _connectedTo = connectedTo;
        _id = id;
        _weight = weight;
        _isActivated = true;
    }

    public Edge(Edge edge)
    {
        _connectedFrom = edge.ConnectedFrom;
        _connectedTo = edge.ConnectedTo;
        _id = edge.Id;
        _weight = edge.Weight;
        _isActivated = true;
    }

    public int ConnectedFrom { get => _connectedFrom; set => _connectedFrom = value; }
    public int ConnectedTo { get => _connectedTo; set => _connectedTo = value; }
    public int Id { get => _id; set => _id = value; }
    public double Weight { get => _weight; set => _weight = value; }
    public bool IsActivated { get => _isActivated; set => _isActivated = value; }
}
