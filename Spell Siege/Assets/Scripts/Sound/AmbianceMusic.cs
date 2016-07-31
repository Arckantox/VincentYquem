using UnityEngine;
using System.Collections;

public class AmbianceMusic : MonoBehaviour {

    private AudioSource _source;

    public AudioClip[] _Loop;
    public bool _multiLoop;
    private int _nb, _current;


    void Start ()
    {
        _source = GetComponent<AudioSource>();
        _nb = _Loop.Length;
        _current = 0;
    }

    void Update()
    {
        if (_multiLoop)
        {
            if(!_source.isPlaying)
            {
                _current++;
                if (_current > _nb - 1)
                {
                    _current = 0;
                }
                _source.clip = _Loop[_current];
                _source.Play();
            }
            
        }
    }
}
