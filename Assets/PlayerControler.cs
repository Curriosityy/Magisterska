using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    Player player;
    InputCommand ic;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        ic = FindObjectOfType<InputCommand>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
