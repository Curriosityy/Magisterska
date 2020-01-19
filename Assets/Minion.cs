using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(MinionMana),typeof(MinionHealth))]
public class Minion : MonoBehaviour
{

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
   
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
        if (position.Length == 2)
        {
            position = collision.collider.name;
            minionposition = GameObject.FindObjectOfType<BoardDictionary>().Board[position];
        }
        
    }


}
