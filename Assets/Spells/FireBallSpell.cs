﻿using UnityEngine;

class FireBallSpell : Spell
{
    public override string SpellName => "fireball";

    public override int ManaCost => 30;

    public override void Cast(Minion caster, string position)
    {
        //TODO in the future use assetbundle
        base.Cast(caster, position);
        if (!canCast)
            return;
        var prefab = Resources.Load("Prefabs/FireBall");
        var fireball = Object.Instantiate(prefab, caster.transform.position, Quaternion.identity) as GameObject;
        Physics.IgnoreCollision(caster.GetComponentInChildren<Collider>(), fireball.GetComponentInChildren<Collider>());
        /*
         *          TODOD
         * scorch mana and play come cool particles
         */
        fireball.GetComponent<FireBall>().Initialize(Object.FindObjectOfType<BoardDictionary>().Board[position].transform.position);
    }
}
