using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell
{
    public string SpellName{get;set;}
    public abstract void Cast();
}
