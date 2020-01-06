
using UnityEngine;

class WalkSpell : Spell
{
    public override string SpellName => "walk";

    public override int ManaCost => 30;

    public override void Cast(Minion caster, string position)
    {
        var pos = Object.FindObjectOfType<BoardDictionary>().Board[position];
        Debug.Log(string.Format("WalkSpell Casted old value{0}, new value{1}", caster.transform.position, pos.transform.position));
        caster.transform.position = pos.transform.position;
        
    }
}

