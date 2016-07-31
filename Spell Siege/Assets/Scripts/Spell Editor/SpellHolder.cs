using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SpellHolder : MonoBehaviour {

    public static SpellHolder _SpellHolder;

    public GameObject _BasicSpell;

    public List<GameObject> _Spellz;

	void Awake ()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        _SpellHolder = this;
        DontDestroyOnLoad(this.gameObject);
	}
	
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.Space))
        {
            SaveSpell(0, Socketing._socketing._MinorGems);
        }
	}

    public void SaveSpell(int index, MinorGem[] tabMinGems)
    {
        if(index > _Spellz.Count)
        {
            return;
        }

        if(index < _Spellz.Count)
        {
            _Spellz[index] = CreateSpell(tabMinGems);


        }
        if(index == _Spellz.Count)
        {
            _Spellz.Add(CreateSpell(tabMinGems));
        }
    }

    private GameObject CreateSpell(MinorGem[] tabMinGems)
    {
        GameObject _Created = Instantiate(_BasicSpell, Vector2.one * -1000, Quaternion.identity) as GameObject;
        foreach(MinorGem MG in tabMinGems)
        {
            if(MG != null)
            {
                _Created = MG.ApplyEffect(_Created);
                _Created.transform.parent = this.transform;
            }
        }

        Debug.Log("Spell Created");
        return _Created;
    }

}
