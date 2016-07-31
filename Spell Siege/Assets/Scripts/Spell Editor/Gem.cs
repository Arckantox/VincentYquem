using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public abstract class Gem : MonoBehaviour    // Class mère des Gems Mineures et Majeures
{

    public abstract GameObject ApplyEffect(GameObject Spell);

}

[System.Serializable]
public class MinorGem : Gem    // Class mère des Gems Mineures et Majeures
{
    public override GameObject ApplyEffect(GameObject Spell)
    {
        return Spell;
    }
}


[System.Serializable]
public class MajorGem : Gem    // Class mère des Gems Mineures et Majeures
{
    public override GameObject ApplyEffect(GameObject Spell)
    {
        return Spell;
    }

}