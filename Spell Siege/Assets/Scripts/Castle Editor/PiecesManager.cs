using UnityEngine;
using System.Collections;

public class PiecesManager : MonoBehaviour
{
    public static PiecesManager _PiecesManager;

    private GameObject _selectedPiece;
    private Camera _MCam;
    private RaycastHit2D hit;
    private Rigidbody2D _targetRB;
    private Collider2D _targetCol;
    private bool _draging;
    private bool _rotate;
    private Transform _rotator;

    private Vector3 _ScreenPos;
    private Vector2 _ScreenPos2D;
    private float _dist;
    private Vector3 _Mpos;
    private Vector3 _eulerStartRot;

    private bool _DragScreen;
    private Vector2 _initialMousePos;
    private Vector3 _initialCamPos;
    public Vector2 LimitX;
    public Vector2 LimitY;



    void Awake()
    {
        _PiecesManager = this;
    }

    void Start ()
    {
        _MCam = Camera.main;
        _rotator = GameObject.FindGameObjectWithTag("Rotator").transform;
        _selectedPiece = null;
        _draging = false;
        _rotate = false;
        _DragScreen = false;
	}

    public void SelectPiece(GameObject Go)
    {
        if (_selectedPiece) // Si une pièce est déjà sélectionnée alors on lui rend sa physique
        {
            _targetRB.gravityScale = 1;
            _targetCol.enabled = true;
            _selectedPiece.GetComponent<SpriteRenderer>().sortingOrder = 0;
            _selectedPiece = null;
            _draging = false;
        }
        // On définie la variable _selectedPiece avec la pièce touchée et lui enlève sa physique
        _selectedPiece = Go;
        _targetRB = _selectedPiece.GetComponent<Rigidbody2D>();
        _targetCol = _selectedPiece.GetComponent<Collider2D>();

        _targetRB.gravityScale = 0;
        _targetRB.velocity = Vector3.zero;
        _targetRB.angularVelocity = 0f;
        _targetCol.enabled = false;
        _rotator.position = _selectedPiece.transform.position;
        _draging = true;
        _selectedPiece.GetComponent<SpriteRenderer>().sortingOrder = 50;
    }

    void Update ()
    {
        if(_selectedPiece && !_draging)
        {
            _rotator.position = _selectedPiece.transform.position;
        }
        else
        {
            _rotator.position = Vector3.one * 1000;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _ScreenPos = _MCam.ScreenToWorldPoint(Input.mousePosition);
            _ScreenPos2D = new Vector2(_ScreenPos.x, _ScreenPos.y);
            if (_selectedPiece) //Si une pièce est déja sélectionnée
            {
                _ScreenPos = _MCam.ScreenToWorldPoint(Input.mousePosition);
                _dist = Vector3.Distance(new Vector3(_ScreenPos.x, _ScreenPos.y, 0), _selectedPiece.transform.position);
                if (_dist < 2) //Si le clique est à proxymitée de la pièce alors ça la drag
                {
                    _draging = true;
                }
                if (_dist > 5) // Si le clique est éloigné  de la pièce
                {
                    _targetRB.gravityScale = 1;
                    _targetCol.enabled = true;
                    _selectedPiece.GetComponent<SpriteRenderer>().sortingOrder = 0;
                    _selectedPiece = null;
                    _draging = false;
                }
            }
            if (hit = Physics2D.Raycast(_ScreenPos2D, Vector2.zero, 100)) //Raycast 2D à la position de la souris
            {
                if (hit.transform.tag == "CastlePiece" && _selectedPiece == null) //Si touche une pièce alors qu'aucune n'est sélectionné
                {
                    SelectPiece(hit.transform.gameObject);
                }
                else //Si raycast ne touche pas une pièce
                {
                    if (hit.transform.tag == "Rotator")
                    {
                        Vector3 diff = _MCam.ScreenToWorldPoint(Input.mousePosition) - _rotator.position;
                        diff.Normalize();

                        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                        _rotator.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

                        _rotate = true;
                        _eulerStartRot = _selectedPiece.transform.eulerAngles;
                        _selectedPiece.transform.eulerAngles = new Vector3(0f, 0f, _rotator.eulerAngles.z - _eulerStartRot.z);
                    }
                }
            }
            else
            {
                _DragScreen = true;
                _initialCamPos = _MCam.transform.position;
                _initialMousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
            }
        }

        if(Input.GetMouseButton(0))
        {
            if(_selectedPiece)
            {
                if (_draging)
                {
                    _ScreenPos = _MCam.ScreenToWorldPoint(Input.mousePosition);
                    _Mpos = new Vector3(_ScreenPos.x, _ScreenPos.y, 0);
                    _selectedPiece.transform.position = _Mpos;
                }

                if (_rotate)
                {
                    Vector3 diff = _MCam.ScreenToWorldPoint(Input.mousePosition) - _rotator.position;
                    diff.Normalize();

                    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    _rotator.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                    _selectedPiece.transform.eulerAngles = new Vector3(0f, 0f, _rotator.eulerAngles.z - _eulerStartRot.z);
                }
            }
            else
            {
                if (_DragScreen)
                {
                    Vector2 Mouse2D = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    float _X = _initialCamPos.x - ((Mouse2D.x - _initialMousePos.x) / Screen.height * _MCam.orthographicSize * 2f);
                    _X = Mathf.Clamp(_X, LimitX.x, LimitX.y);
                    float _Y = _initialCamPos.y - ((Mouse2D.y - _initialMousePos.y) / Screen.height * _MCam.orthographicSize * 2f);
                    _Y = Mathf.Clamp(_Y, LimitY.x, LimitY.y);

                    _MCam.transform.position = new Vector3(_X, _Y, _MCam.transform.position.z);
                }
            }
            
            
               
        }

        if(Input.GetMouseButtonUp(0))
        {
            if (_selectedPiece)
            {
                if (_draging)
                {
                    _draging = false;
                }

                if (_rotate)
                {
                    //_selectedPiece.transform.parent = GameObject.FindGameObjectWithTag("EditorScripts").transform;
                    _rotate = false;
                }
            }
            _DragScreen = false;
        }

	}
}
