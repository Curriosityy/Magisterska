using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {

        
    }
    public static void movetowards()
    {

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
