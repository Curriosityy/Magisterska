using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    float speed = 5f;
    private IEnumerator coroutine;
    // Start is called before the first frame update
    void Start()
    {

        //coroutine = walk(path,player);
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    private IEnumerator walk(List<GameObject> path,Minion player)
    {
        int i = 0;
        while (i<=path.Count)
        {

            yield return null;
        }
    }
}
