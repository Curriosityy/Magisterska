using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class SpellFactory
{
    private static Dictionary<string, Type> spellsByName;
    private static bool IsInitialized=>spellsByName!=null;
    public static void InitializeFactory()
    {
        if(IsInitialized)
            return;

        var spellTypes = Assembly.GetAssembly(typeof(Spell)).GetTypes()
            .Where(myClass => myClass.IsClass && !myClass.IsAbstract && !myClass.IsSubclassOf(typeof(Spell)));
        spellsByName = new Dictionary<string, Type>();
        foreach(var type in spellTypes)
        {
            var tempSpell = Activator.CreateInstance(type) as Spell;
            spellsByName.Add(tempSpell.SpellName, type);
        }
    }

    public static Spell GetSpell(string spellName)
    {
        InitializeFactory();
        if(spellsByName.ContainsKey(spellName))
        {
            return Activator.CreateInstance(spellsByName[spellName]) as Spell;
        }
        return null;
    }

    public static IEnumerable<string> GetSpellsName()
    {
        return spellsByName.Keys;
    }
   
}
