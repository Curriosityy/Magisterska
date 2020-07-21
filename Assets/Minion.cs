using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MinionMana),typeof(MinionHealth))]
public class Minion : MonoBehaviour
{
    float speed = 5f;
    private List<int> hitat=new List<int>();
    private IEnumerator coroutine;
    private string position;
    private GameObject minionposition;
    private bool _isDoingSomething = false;
    public GameObject getminionpos
    {
        get { return minionposition; }
    }
    private float _timer;
    private int _spellCasted = 1;
    public int steps = 0;
    public bool IsDoingSomething { get => _isDoingSomething; }
    public float Points {
        get {
            if(steps>0)
            {
                return GetComponent<MinionHealth>().Statistics*100/steps;
                
            }
            return 0;
        }
        set => _timer = value; }
    public bool IsAlive { get =>GetComponent<MinionHealth>().Statistics>0; }
    public string Position { get => position; set => position = value; }
    public float Timer { get => _timer; set => _timer = value; }
    public int SpellCasted { get => _spellCasted; set => _spellCasted = value; }
    public List<int> Hitat { get => hitat; set => hitat = value; }

    //
    public void Start()
    {
        //Restart(1);
    }
    public bool CheckItHitat(int pos2)
    {
        foreach(var hit in hitat)
        {
            if (hit == pos2)
            {
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        if(IsAlive)
        {

            _timer += Time.deltaTime;
           /* if(steps>DefensiveGameManager.Instance.maxSteps)
            _timer += Time.deltaTime;
            if(steps>OffensiveGameManager.Instance.maxSteps)
            {
                GetComponent<MinionHealth>().DealDamage(100);
                StopAllCoroutines();
            }*/
        }
    }
    public void Restart(int restartPoint)
    {
        hitat = new List<int>();
        var bd = FindObjectOfType<BoardDictionary>();
        switch (restartPoint)
        {
            case 1:
                position = bd.SpawningPoint1;
                break;
            case 2:
                position = bd.SpawningPoint2;
                break;
        }
        StopAllCoroutines();
        minionposition = bd.Board[position];
        transform.position = minionposition.transform.position;
        GetComponent<MinionMana>().Restart();
        GetComponent<MinionHealth>().Restart();
        _timer = 0;
        SpellCasted = 0;
        steps = 0;
        _isDoingSomething = false;
    }
    public void MoveTo(List<GameObject> path)
    {
        //SpellCasted++;
        coroutine = Walk(path);
        StartCoroutine(coroutine);
    }
    void OnCollisionEnter(Collision collision)
    {
        
        //Debug.Log(collision.collider.name);
        if (collision.gameObject.GetComponent<PointInfo>()!=null)
        {
            position = collision.collider.name;
            minionposition = GameObject.FindObjectOfType<BoardDictionary>().Board[position];
        }
        
    }
    private IEnumerator Walk(List<GameObject> path)
    {
        //steps += 1;
        _isDoingSomething = true;
        int i = 0;
        //Debug.Log(path.Count);
        while (i <path.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position,path[i].transform.position,speed*Time.deltaTime);
            if (Vector3.Distance(transform.position, path[i].transform.position)<0.01f)
            {
                position = path[i].GetComponent<PointInfo>().name;
                i += 1;
                //steps++;
            }
            yield return null;
        }
        position = path[i-1].name;
        minionposition = GameObject.FindObjectOfType<BoardDictionary>().Board[position];
        _isDoingSomething = false;
    }

}
