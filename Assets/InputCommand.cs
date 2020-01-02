using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InputCommand : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI _output;
    /*
     * To consider if output window is needed.
     */
    private TMP_InputField _inputField;

    private void Start()
    {
        _inputField = GetComponent<TMP_InputField>();
    }

    public string GetInputValue()
    {
        /*
         *                              TODO:
         * some kind of parser, depends on what we want to do with commands.
         */
        string inputToReturn = _inputField.text;
        _inputField.text = "";
        return _inputField.text;
    }
}
