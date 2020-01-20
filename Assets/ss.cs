using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointInfo : MonoBehaviour
{
   
    int _g;
    float _f;
    private int _x;
    private int _y;
    private float _h;
    private GameObject _camefrom;
    private bool _walkable = true;

    public int G { get => _g; set => _g = value; }
    public float F { get => _f; set => _f = value; }
    public int X { get => _x; set => _x = value; }
    public int Y { get => _y; set => _y = value; }
    public float H { get => _h; set => _h = value; }
    public GameObject Camefrom { get => _camefrom; set => _camefrom = value; }
    public bool Walkable { get => _walkable; set => _walkable = value; }

    public void Calcg()
    {

    }
    void Start()
    {
        _g = int.MaxValue;
        _f = int.MaxValue;
    }
    public void setDist(GameObject targetLocation)
    {
        Vector3 s = targetLocation.transform.position;
        _h =Vector3.Distance(gameObject.transform.position, s);
        //Debug.Log(h);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
