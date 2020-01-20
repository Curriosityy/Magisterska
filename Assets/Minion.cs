using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MinionMana),typeof(MinionHealth))]
public class Minion : MonoBehaviour
{
    float speed = 5f;
    private IEnumerator coroutine;
    public string position;
    private GameObject minionposition;
    public GameObject getminionpos
    {
        get { return minionposition; }
    }
    public void Start()
    {
        minionposition = GameObject.FindObjectOfType<BoardDictionary>().Board["A3"];
        position = "A3";
        
    }
    public void MoveTo(List<GameObject> path)
    {

        coroutine = walk(path);
        StartCoroutine(coroutine);
    }
    void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log(collision.collider.name);
        if (collision.gameObject.GetComponent<PointInfo>()!=null)
        {
            position = collision.collider.name;
            minionposition = GameObject.FindObjectOfType<BoardDictionary>().Board[position];
        }
        
    }
    private IEnumerator walk(List<GameObject> path)
    {
        int i = 0;
        Debug.Log(path.Count);
        while (i <path.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position,path[i].transform.position,speed*Time.deltaTime);
            if (Vector3.Distance(transform.position, path[i].transform.position)<0.01f)
            {
                i += 1;
            }
            yield return null;
        }
        position = path[i-1].name;
        minionposition = GameObject.FindObjectOfType<BoardDictionary>().Board[position];
    }

}
