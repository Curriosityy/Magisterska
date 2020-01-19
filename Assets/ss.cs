using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ss : MonoBehaviour
{
   
    public int g;
    public float f;
    public int x;
    public int y;
    public float h;
    public GameObject camefrom;
    public bool walkable = true;
    public void calcg()
    {

    }
    void Start()
    {
        g = int.MaxValue;
        f = int.MaxValue;
    }
    public void setDist(GameObject targetLocation)
    {
        Vector3 s = targetLocation.transform.position;
        h =Vector3.Distance(gameObject.transform.position, s);
        //Debug.Log(h);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
