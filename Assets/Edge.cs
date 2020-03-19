public class Edge
{
    int _connectedFrom;
    int _connectedTo;
    int _id;
    public static int innoNumber = 0;
    double _weight;
    public Edge(int connectedFrom, int connectedTo, int id, double weight)
    {
        _connectedFrom = connectedFrom;
        _connectedTo = connectedTo;
        _id = id;
        _weight = weight;
    }

    public int ConnectedFrom { get => _connectedFrom; set => _connectedFrom = value; }
    public int ConnectedTo { get => _connectedTo; set => _connectedTo = value; }
    public int Id { get => _id; set => _id = value; }
    public double Weight { get => _weight; set => _weight = value; }
}
