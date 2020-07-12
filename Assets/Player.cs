using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //TODO swap gameobject with minion script.
    private Minion _controledMinion;
    public Minion Minion { get => _controledMinion; }
    [SerializeField] private Minion _minionPrefab;
    private BoardDictionary _dictionary;
    // Start is called before the first frame update
    void Start()
    {
        //Replace it with somekind of injection for multiboard purpose, while train ai.
        _dictionary = FindObjectOfType<BoardDictionary>();
    }

    private void SpawnMinion()
    {
        var minion = Instantiate(_minionPrefab);
        minion.transform.position = _dictionary.Board[_dictionary.SpawningPoint1].transform.position;
        minion.transform.SetParent(_dictionary.transform);
        minion.Restart(1);
        _controledMinion = minion;
    }
    // Update is called once per frame
    void Update()
    {
        if(!_controledMinion)
        {
            SpawnMinion();
        }
    }
}
