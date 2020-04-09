using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDictionary : MonoBehaviour
{
    private Dictionary<string, GameObject> _board;
    [SerializeField] private Transform _pointsHolder;
    [SerializeField] private int _sizex = 7, _sizey = 7;

    [SerializeField] private string _spawningPoint1;
    [SerializeField] private string _spawningPoint2;

    public int size
    {
        get { return _sizex; }
    }

    public Dictionary<string, GameObject> Board { get => _board; }
    public string SpawningPoint1 { get => _spawningPoint1; }
    public string SpawningPoint2 { get => _spawningPoint2; }

    // Start is called before the first frame update
    void Awake()
    {
        CreateDictionary();
        var temp = new GameObject("SpawningPoint1");
        temp.transform.position = _board[_spawningPoint1].transform.position;
        temp.transform.parent = transform;
        temp = new GameObject("SpawningPoint2");
        temp.transform.position = _board[_spawningPoint2].transform.position;
        temp.transform.parent = transform;
    }
    private void CreateDictionary()
    {
        GameObject temp;
        string position;
        _board = new Dictionary<string, GameObject>();
        for (int i = 0; i < _sizex; i++)
        {
            for (int j = 0; j < _sizey; j++)
            {
                position = (char)('A' + i) + "" + (j+1);
                temp = new GameObject(position);
                temp.transform.parent = _pointsHolder;
                temp.transform.localPosition = new Vector3(-i - 0.5f, 0.5f, j + 0.5f);
                temp.AddComponent<PointInfo>();
                BoxCollider bc= temp.AddComponent<BoxCollider>();
                bc.size = new Vector3 (1,1,1);
                bc.isTrigger = true;
                //Rigidbody rb = temp.AddComponent<Rigidbody>();
                //
                //temp.AddComponent<Rigidbody>();
                _board.Add(position, temp);
                
            }
        }
        
    }
}
