using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{


    public static AudioManager _AudioManager;
    private AudioSource _source;
    public AudioClip _spell;

    public AudioClip _impact;

    public AudioClip[] _Breaksounds;

    void Awake()
    {
        _AudioManager = this;
        _source = GetComponent<AudioSource>();
    }

    public void PlaySpell(float vol)
    {
        _source.PlayOneShot(_spell, vol);
    }

    public void PlayDestruction(int mat, float vol)
    {
        if (_Breaksounds[mat - 1] != null)
        {
            _source.PlayOneShot(_Breaksounds[mat - 1], vol);
        }
    }

    public void PlayImpact(float vol)
    {
        _source.PlayOneShot(_impact, vol);
    }
}
