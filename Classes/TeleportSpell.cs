using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class TeleportSpell : Spell
{
    public override string SpellName => "Teleport";

    public override int ManaCost => 20;

    public override SpellType Type => SpellType.Defensive;

    public override void Cast(Minion caster, string position)
    {
        base.Cast(caster, position);
        var bd = caster.transform.parent.GetComponent<BoardDictionary>();
        if (!canCast || caster.Position==position || !bd.Board[position].GetComponent<PointInfo>().Walkable)
        {
            return;
        }
        caster.GetComponent<MinionMana>().BurnMana(ManaCost);
        caster.transform.position = bd.Board[position].transform.position;
        caster.Position = position;
    }
}

