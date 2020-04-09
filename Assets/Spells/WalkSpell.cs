
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class WalkSpell : Spell
{

    public override string SpellName => "walk";

    public override int ManaCost => 10;

    public override SpellType Type => SpellType.Defensive;

    public override void Cast(Minion caster, string position)
    {
        List<GameObject> path;
        if (caster.Position == position)
            return;
        base.Cast(caster, position);
        if (!canCast)
            return;
        var bd = caster.transform.parent.GetComponent<BoardDictionary>();
        path=PathFinder.FindPath(bd.Board[position], caster.getminionpos,bd);
        if (path.Count>0)
        {
            caster.GetComponent<MinionMana>().BurnMana(ManaCost);

            path.Reverse();
            caster.MoveTo(path);
        }
        else
        {
            int a = 3;
        }

        //caster.transform.position = pos.transform.position;

        
        
    }
    
}

