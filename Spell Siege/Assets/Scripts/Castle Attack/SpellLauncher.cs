using UnityEngine;
using System.Collections;

public class SpellLauncher : MonoBehaviour {

    public int _maxForce = 10;
    public float _multiplier = 50;
    private Vector2 _direction;
    private Vector2 _startPoint;
    private Vector2 _releasePoint;
    private float _scale;
    private Vector3 _ScreenPos;
    private Vector2 _ScreenPos2D;

    public Transform _shootOrigin;
    public Transform _spriteDirection;

    public GameObject _spell;

    private Camera _MCam;

    private bool _screenDrag;
    public Vector2 LimitX;
    private Vector2 _initialMousePos;
    private Vector3 _initialCamPos;

    private Transform _trackedSpell;
    private Vector3 _initialPos;

    void Start ()
    {
        _MCam = Camera.main;
        _screenDrag = false;
        _trackedSpell = null;
        _initialPos = _MCam.transform.position;
	}
	
	public void AllowScreenDrag()
    {
        _screenDrag = !_screenDrag;
    }

    void LaunchSpell()
    {
        Vector3 _start = _MCam.ScreenToWorldPoint(_startPoint);
        Vector3 _release = _MCam.ScreenToWorldPoint(_releasePoint);
        _direction = _start - _release;
        _direction = _direction.normalized;
        float force = Vector2.Distance(_start, _release);
        force = Mathf.Clamp(force, -_maxForce, _maxForce);

        if (SpellHolder._SpellHolder)
        {
            GameObject _SP = GameObject.Instantiate(SpellHolder._SpellHolder._Spellz[0], _shootOrigin.position, Quaternion.identity) as GameObject;
            _SP.GetComponent<Rigidbody2D>().velocity = (_direction * force * _multiplier);
            _trackedSpell = _SP.transform;

        }
        else
        {
            GameObject _SP = GameObject.Instantiate(_spell, _shootOrigin.position, Quaternion.identity) as GameObject;
            _SP.GetComponent<Rigidbody2D>().velocity = (_direction * force * _multiplier);
            _trackedSpell = _SP.transform;
        }
       
        if(AudioManager._AudioManager)
        {
            AudioManager._AudioManager.PlaySpell(1);
        }

    }

    void FixedUpdate()
    {
        if(_trackedSpell)
        {
            Vector3 _spellPos;
            _spellPos = _MCam.WorldToScreenPoint(_trackedSpell.transform.position);
            if(_spellPos.x > Screen.width*0.5f)
            {
                _MCam.transform.position = Vector3.Lerp(_MCam.transform.position, new Vector3 (_trackedSpell.position.x, _MCam.transform.position.y, _MCam.transform.position.z ), 0.1f);
                float _X = _MCam.transform.position.x;
                _X = Mathf.Clamp(_X, LimitX.x, LimitX.y);

                _MCam.transform.position = new Vector3(_X, _MCam.transform.position.y, _MCam.transform.position.z);
            }
            else
            {
                _MCam.transform.position = Vector3.Lerp(_MCam.transform.position, _initialPos, 0.1f);
            }
        }
        else
        {
            if(!_screenDrag)
            {
                //_MCam.transform.position = Vector3.Lerp(_MCam.transform.position, _initialPos, 0.1f);
            }
        }
        if (!_screenDrag)
        {
            Vector3 _startpos = _MCam.ScreenToWorldPoint(_startPoint);
            _spriteDirection.position = new Vector3(_startpos.x, _startpos.y, 0f);

            if (Input.GetMouseButtonDown(0))
            {
                if (_trackedSpell)
                {
                    _trackedSpell = _shootOrigin;
                }

                _startPoint = Input.mousePosition;
                
            }

            if (Input.GetMouseButton(0))
            {
                //Vector3 _startpos = _MCam.ScreenToWorldPoint(_startPoint);
                //_spriteDirection.position = new Vector3(_startpos.x, _startpos.y, 0f);

                _ScreenPos = _MCam.ScreenToWorldPoint(Input.mousePosition);

                Vector3 diff = _ScreenPos - _spriteDirection.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                _spriteDirection.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

                _scale = Vector2.Distance(_MCam.ScreenToWorldPoint(_startPoint), _ScreenPos);
                _scale = Mathf.Clamp(_scale, 0, _maxForce);

                _spriteDirection.localScale = new Vector3(1, _scale, 1);

            }

            if (Input.GetMouseButtonUp(0))
            {

                _ScreenPos2D = Input.mousePosition;

                _releasePoint =  _ScreenPos2D;

                LaunchSpell();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                _initialCamPos = _MCam.transform.position;
                _initialMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 Mouse2D = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                float _X = _initialCamPos.x - ((Mouse2D.x - _initialMousePos.x) * _MCam.orthographicSize*2f / Screen.height);
                _X = Mathf.Clamp(_X, LimitX.x, LimitX.y);

                _MCam.transform.position = new Vector3(_X, _MCam.transform.position.y, _MCam.transform.position.z);
            }

        }
    }

}
