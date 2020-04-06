﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SpellType{
    Defensive,
    Offensive
}
public abstract class Spell
{
    protected bool canCast;
    public abstract string SpellName{get;}
    public abstract int ManaCost { get; }
    public abstract SpellType Type { get; }
    public virtual void Cast(Minion caster, string position)
    {
        canCast = false;
        if (caster.GetComponent<MinionMana>().Statistics < ManaCost)
            return;
        caster.GetComponent<MinionMana>().BurnMana(ManaCost);
        canCast = true;
    }
}
