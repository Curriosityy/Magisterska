using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell
{
    public abstract string SpellName{get;}
    public abstract void Cast(Minion caster, string position);
}
