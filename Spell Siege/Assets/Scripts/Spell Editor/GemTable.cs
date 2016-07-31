using UnityEngine;
using System.Collections;

public class GemTable : MonoBehaviour {

    public static GemTable _GemTable;

    public MinorGem[] _MinorGemTable;

	void Awake ()
    {
        _GemTable = this;
	}
	
}
