using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
        if(player.Minion && Input.GetKeyDown(KeyCode.Return))
        {
            var inputValue = ic.GetInputValue().Split(' ');
            Debug.Log(string.Format("Getted input value {0}",inputValue));
            Spell spell = SpellFactory.GetSpell(inputValue[0].ToLower());
            if(inputValue.Length>=2)
                spell?.Cast(player.Minion, inputValue[1].ToUpper());
        }
    }
}
