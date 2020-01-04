using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space clicked");
            var walkSpell = new WalkSpell();
            walkSpell.Cast(gameObject, "A1");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q clicked");
            var fireballSpell = new FireBallSpell();
            fireballSpell.Cast(gameObject, "G7");
        }
    }
}
