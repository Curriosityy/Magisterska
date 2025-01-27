﻿using System;
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
    float _weight;
    [SerializeField]
    private bool _isActivated;

    public Edge(int connectedFrom, int connectedTo, int id, float weight)
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
        _isActivated = edge._isActivated;
    }

    public Edge(Edge edge, float weight)
    {
        _connectedFrom = edge.ConnectedFrom;
        _connectedTo = edge.ConnectedTo;
        _id = edge.Id;
        _weight = weight;
        _isActivated = edge._isActivated;
    }

    public int ConnectedFrom { get => _connectedFrom;}
    public int ConnectedTo { get => _connectedTo;}
    public int Id { get => _id;}
    public float Weight { get => _weight; set => _weight = value; }
    public bool IsActivated { get => _isActivated; set => _isActivated = value; }
}
