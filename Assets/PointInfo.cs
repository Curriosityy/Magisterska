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
    private int _aiInfo = 0;
    public int G { get => _g; set => _g = value; }
    public float F { get => _f; set => _f = value; }
    public int X { get => _x; set => _x = value; }
    public int Y { get => _y; set => _y = value; }
    public float H { get => _h; set => _h = value; }
    public GameObject Camefrom { get => _camefrom; set => _camefrom = value; }
    public bool Walkable { get => _walkable; set => _walkable = value; }
    public int AiInfo { get => _aiInfo; set => _aiInfo = value; }
    private List<GameObject> _overlapedGo;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collided "+other.name);
        if (other.transform.tag == "Obstacle")
        {
            //Debug.Log("Obstacle in");
            _walkable = false;
            if (other.GetComponent<Minion>() != null)
            {
                _aiInfo += 2;
            } else if (other.GetComponent<DestroyBoxObj>() != null)
            {
                _aiInfo += 1;
            }
        } else
        if (other.transform.tag == "Attack")
        {
            _aiInfo +=3;
            _walkable = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.transform.tag == "Obstacle")
        {
            //Debug.Log("Obstacle out");
            _walkable = true;
            if (other.GetComponent<Minion>() != null)
            {
                _aiInfo -= 2;
            }
            else if (other.GetComponent<DestroyBoxObj>() != null)
            {
                _aiInfo -= 1;
            }
        }
        else
        if (other.transform.tag == "Attack")
        {
            _aiInfo -= 3;
        }
    }

    void Start()
    {
        _g = int.MaxValue;
        _f = int.MaxValue;
    }
    public void setDist(GameObject targetLocation)
    {
        Vector3 s = targetLocation.transform.position;
    }
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (!_walkable)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, new Vector3(0.7f, 0.7f, 0.7f));
        }
    }
}
