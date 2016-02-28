using UnityEngine;
using System.Collections;
using System;

public interface ISpell {

    ISpell fuseWith(ISpell other);

    SpellEffect effect();
	
}

public struct SpellEffect
{
    public float damage;
    public float radius;
    public Element element;

    public override string ToString()
    {
        return "damage: " + damage + ", radius: " + radius + ", element: " + element;
    }
}

public enum Element
{
    Fire,
    Ice,
    Lightning,
    Adam,
    White
}

public class PerfectSpell : ISpell
{
    private SpellEffect _effect = new SpellEffect { damage = 100.0f, radius = 10.0f, element = Element.Adam };

    public SpellEffect effect()
    {
        return _effect;
    }

    public ISpell fuseWith(ISpell other)
    {
        // fuck other spells
        return this;
    }
}
