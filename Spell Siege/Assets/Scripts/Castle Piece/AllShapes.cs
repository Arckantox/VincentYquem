using UnityEngine;
using System.Collections;

public class AllShapes : MonoBehaviour {

    public static AllShapes _AllShapes;

    //Boolean indiquant si les pièces on leur script de santé actif
    public static bool _atk;

    public GameObject[] _TabShapes;

	// Use this for initialization
	void Start()
    {
        _AllShapes = this;
        if(CastleAttack._CastleAttack)
        {
            _atk = true;
        }
        else
        {
            _atk = false;
        }
	}
	
}
