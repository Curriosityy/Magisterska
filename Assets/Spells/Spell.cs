using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell
{
    public abstract string SpellName{get;}
    public abstract int ManaCost { get; }
    public virtual void Cast(Minion caster, string position)
    {
        if (caster.GetComponent<MinionMana>().Statistics < ManaCost)
            return;
        caster.GetComponent<MinionMana>().BurnMana(ManaCost);
    }
}
