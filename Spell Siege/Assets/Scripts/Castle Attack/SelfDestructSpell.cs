using UnityEngine;
using System.Collections;

public class SelfDestructSpell : MonoBehaviour {

    public float _delay;
    private float _timer;
    private bool _collided = false;
    [SerializeField]
    private ParticleSystem _ball;
    [SerializeField]
    private ParticleSystem _boom;

	// Use this for initialization
	void Start ()
    {
        _collided = false;
        _timer = 0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(GetComponent<Rigidbody2D>().velocity.magnitude);
	    if(_collided)
        {
            if(_timer >= _delay)
            {
                Destroy(gameObject);
            }
            _timer += Time.deltaTime;
        }
	}

    void OnCollisionEnter2D(Collision2D _c)
    {
        _collided = true;
        GetComponent<CircleCollider2D>().enabled = false;
        //Rigidbody2D _rb = GetComponent<Rigidbody2D>();
        //_rb.velocity = -_rb.velocity;
        Destroy(_ball);
        _boom.Emit(10);
    }
}
