using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class SpellFactory
{
    private static Dictionary<string, Type> _spellsByName;
    private static bool IsInitialized=>_spellsByName!=null;
    private static void InitializeFactory()
    {
        if(IsInitialized)
            return;

        var spellTypes = Assembly.GetAssembly(typeof(Spell)).GetTypes()
            .Where(myClass => myClass.IsClass && !myClass.IsAbstract && myClass.IsSubclassOf(typeof(Spell)));
        _spellsByName = new Dictionary<string, Type>();
        foreach(var type in spellTypes)
        {
            var tempSpell = Activator.CreateInstance(type) as Spell;
            _spellsByName.Add(tempSpell.SpellName, type);
        }
    }

    public static Spell GetSpell(string spellName)
    {
        InitializeFactory();
        if(_spellsByName.ContainsKey(spellName))
        {
            return Activator.CreateInstance(_spellsByName[spellName]) as Spell;
        }
        return null;
    }

    public static IEnumerable<string> GetSpellsName()
    {
        InitializeFactory();
        return _spellsByName.Keys;
    }
   
}
