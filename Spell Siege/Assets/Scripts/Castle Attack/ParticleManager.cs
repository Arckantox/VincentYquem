using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour {

    public static ParticleManager _ParticleManager;

    public ParticleSystem _Wood;
    public ParticleSystem _Stone;
    public ParticleSystem _Paper;
    public ParticleSystem _Glass;
    public ParticleSystem _Metal;

    public int _number;

    //Material Table
    /*
        Wood = 1
        Stone = 2
        Paper = 3
        Glass = 4
        Metal = 5

    */

    void Awake()
    {
        _ParticleManager = this;
    }

    public void EmitParticle(int material, Vector2 pos, int amount)
    {
        amount = _number;
        switch(material)
        {
            case 0:
                return;
            case 1:
                _Wood.transform.position = pos;
                _Wood.Emit(amount);
                break;
            case 2:
                _Stone.transform.position = pos;
                _Stone.Emit(amount);
                break;
            case 3:
                _Paper.transform.position = pos;
                _Paper.Emit(amount);
                break;
            case 4:
                _Glass.transform.position = pos;
                _Glass.Emit(amount);
                break;
            case 5:
                _Metal.transform.position = pos;
                _Metal.Emit(amount);
                break;
        }
    }

}
