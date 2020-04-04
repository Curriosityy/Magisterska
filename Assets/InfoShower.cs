using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoShower : MonoBehaviour
{
    [SerializeField] Text generacjaText;
    [SerializeField] Text fitnessText;
    [SerializeField] Text gatunkiText;
    [SerializeField] Text gatunki2Text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        generacjaText.text = NeatValues.GenerationCount.ToString();
        fitnessText.text = NeatValues.BestFitness.ToString();
        gatunkiText.text = Population.Instance.Species.Count.ToString();
        gatunki2Text.text = "";
        foreach (var species in Population.Instance.Species)
        {
            gatunki2Text.text += " " + species.Individuals.Count;
        }


    }
}
