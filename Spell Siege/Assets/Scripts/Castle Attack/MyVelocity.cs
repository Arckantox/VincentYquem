using UnityEngine;
using System.Collections;

public class MyVelocity : MonoBehaviour {

    private Rigidbody2D _rb;
    public Vector2 _Vel;
	
    void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

	void FixedUpdate ()
    {
        _Vel = _rb.velocity;
	}
}
