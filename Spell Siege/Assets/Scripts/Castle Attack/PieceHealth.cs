using UnityEngine;
using System.Collections;

public class PieceHealth : MonoBehaviour {

    private float _maxHealth;
    public float _health;
    public Sprite[] _states;
    private SpriteRenderer _spriteR;
    private Rigidbody2D _rb;
    public int _mat;

    void OnEnable()
    {
        if(!AllShapes._atk)
        {
            Destroy(this);
        }

        _maxHealth = _health;
        _spriteR = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();

    }

    public void TakeDamage(float power, bool IsSpell = false)
    {
        _health -= (power * 1);
        //Debug.Log(gameObject.name + "  |  " + _health + "/" + _maxHealth);
        if(_states.Length > 0)
        {
            if (_health < (_maxHealth * 0.666f))
            {
                _spriteR.sprite = _states[0];
            }
            if (_health < (_maxHealth * 0.333f))
            {
                _spriteR.sprite = _states[1];
            }
        }
        

        if (_health <= 0f)
        {
            if(ParticleManager._ParticleManager) //Envoi des particules lors de la destruction
            {
                ParticleManager._ParticleManager.EmitParticle(_mat, transform.position, 10);

            }
            if(AudioManager._AudioManager)
            {
                AudioManager._AudioManager.PlayDestruction(_mat, 1);
            }
            //Debug.Log(gameObject.name + "  |  " + _health + "/"+ _maxHealth);
            Destroy(gameObject);
            return;
        }
        else
        {
            if(!IsSpell)
            {
                if (AudioManager._AudioManager)
                {
                    AudioManager._AudioManager.PlayImpact(1);
                }
            }
            
        }

        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {

        Vector2 v1 = GetComponent<MyVelocity>()._Vel;
        Vector2 v2 = Vector2.zero;
        float m1 = _rb.mass;
        float m2 = 1;

        if (coll.rigidbody)
        {
            v2 = coll.gameObject.GetComponent<MyVelocity>()._Vel;
            m2 = coll.rigidbody.mass;
        }

        Vector3 _project = Vector3.Project(new Vector3(v2.x, v2.y, 0f), new Vector3(v1.x, v1.y, 0f));
        v2 = new Vector2(_project.x, _project.y);
        float velo = (v1 - v2).magnitude;
        float power = ((v1 * m1) - (v2 * m2)).magnitude;


        if (velo >3f)
        {
            if(coll.transform.tag != "Spell")
            {
                TakeDamage(power);
            }
            else
            {
                TakeDamage(power, true);
            }   
        }

        /*        
        float mass = 1;
        if(coll.rigidbody)
        {
            mass = coll.rigidbody.mass;
        }
        float _diffVelo = coll.relativeVelocity.magnitude;
        if(_diffVelo > 3)
        {
            TakeDamage(_diffVelo, mass);
        }
        */
    }

}
