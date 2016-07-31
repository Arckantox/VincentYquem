using UnityEngine;
using System.Collections;

public class BasicPhysicMinorGem : MinorGem
{
    public float _AddedMass = 0f;
    public float _MassMultiplicatorPercent = 0f;


    public override GameObject ApplyEffect(GameObject Spell)
    {
        Spell.GetComponent<Rigidbody2D>().mass = (1+ (_MassMultiplicatorPercent * 0.01f)) *( Spell.GetComponent<Rigidbody2D>().mass + _AddedMass);
        return Spell;
    }

}
