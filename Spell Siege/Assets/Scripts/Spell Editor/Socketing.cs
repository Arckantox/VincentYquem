using UnityEngine;
using System.Collections;

public class Socketing : MonoBehaviour {

    public static Socketing _socketing;

    public Transform _MajorSocket;

    public Transform[] _MinorSockets;
    public SpriteRenderer[] _Indicators;

    public MinorGem[] _MinorGems;

    private bool _dragGem;
    private bool _dragInterface;
    private bool _Major;
    private bool _Minor;
    private GameObject _gem;
    private Transform _Interface;
    private float _interfaceY;
    private float _mouseY;

    private Vector3 _ScreenPos;
    private Vector2 _ScreenPos2D;
    private Camera _MCam;

    private Color _HoverColor = new Color(0f, 1f, 0f, 0.5f);
    private Color _RelatedSocket = new Color(1f, 1f, 1f, 0.7f);
    private Transform _hovered;

    // Use this for initialization
    void Start ()
    {
        _socketing = this;
        _Minor = false;
        _Major = false;
        _dragGem = false;
        _dragInterface = false;
        _MCam = Camera.main;
        _MinorGems = new MinorGem[8];
        for(int i = 0; i<8; i++)
        {
            _MinorGems[i] = null;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetMouseButtonDown(0))
        {
            Select();
        }

        if (Input.GetMouseButton(0))
        {
            if(_dragInterface) //Script pour drag une interface en Y
            {
                _ScreenPos = _MCam.ScreenToWorldPoint(Input.mousePosition);
                float _y = _interfaceY + (_ScreenPos.y - _mouseY);
                if(_Interface.GetComponent<InterfaceClamping>())
                {
                    InterfaceClamping _clamp = _Interface.GetComponent<InterfaceClamping>();
                    _y = Mathf.Clamp(_y, _clamp._minY, _clamp._maxY);
                    _Interface.position = new Vector2(_Interface.position.x, _y);
                }
            }

            if(_dragGem)
            {
                _ScreenPos = _MCam.ScreenToWorldPoint(Input.mousePosition);
                _ScreenPos2D = new Vector2(_ScreenPos.x, _ScreenPos.y);
                _gem.transform.position = _ScreenPos2D;

                if(_Minor)
                {
                    RaycastHit2D hit;
                    _ScreenPos = _MCam.ScreenToWorldPoint(Input.mousePosition);
                    _ScreenPos2D = new Vector2(_ScreenPos.x, _ScreenPos.y);

                    if (hit = Physics2D.Raycast(_ScreenPos2D, Vector2.zero, 50f,1<<8))
                    {
                        hit.transform.GetComponent<SpriteRenderer>().color = _HoverColor;
                        _hovered = hit.transform;
                    }
                    else
                    {
                        foreach (SpriteRenderer rend in _Indicators)
                        {
                            rend.color = _RelatedSocket;
                        }
                        _hovered = null;
                    }
                }
            }
        }


        if (Input.GetMouseButtonUp(0))
        {
            _Interface = null;
            _dragInterface = false;
            _dragGem = false;
            if(_Minor)
            {
                foreach (SpriteRenderer rend in _Indicators)
                {
                    rend.color = new Color (0f, 0f, 0f, 0f);
                }

                PositionGem();
            }
            _Minor = false;
            _Major = false;
            
            _gem = null;

        }

    }

    void Select()
    {
        RaycastHit2D hit;
        _ScreenPos = _MCam.ScreenToWorldPoint(Input.mousePosition);
        _ScreenPos2D = new Vector2(_ScreenPos.x, _ScreenPos.y);

        int layermask = 1 << 5;
        layermask = ~layermask;

        if (hit = Physics2D.Raycast(_ScreenPos2D, Vector2.zero, 50f, layermask))
        {
            if(hit.transform.gameObject.layer == 8)
            {
                if(hit.transform.GetComponent<SocketedScript>()._socketed)
                {
                    Destroy(hit.transform.GetChild(0).gameObject);
                    hit.transform.GetComponent<SocketedScript>()._socketed = false;
                    hit.transform.parent.GetComponent<SpriteRenderer>().enabled = true;
                }
            }

            if (hit.transform.tag == "MinorGem")
            {
                _gem = Instantiate(hit.transform.gameObject, _ScreenPos2D, Quaternion.identity) as GameObject;
                _dragGem = true;
                _dragInterface = false;
                _Minor = true;
                _Major = false;
                foreach (SpriteRenderer rend in _Indicators)
                {
                    rend.color = _RelatedSocket;
                }


            }
            if (hit.transform.tag == "MajorGem")
            {
                _gem = Instantiate(hit.transform.gameObject, _ScreenPos2D, Quaternion.identity) as GameObject;
                _dragGem = true;
                _dragInterface = false;
                _Minor = false;
                _Major = true;
            }
        }
        else
        {
            if((hit = Physics2D.Raycast(_ScreenPos2D, Vector2.zero, 50f)))
            {
                if (hit.transform.tag == "Interface")
                {
                    _dragGem = false;
                    _dragInterface = true;
                    _Minor = false;
                    _Major = false;
                    _Interface = hit.transform;
                    _mouseY = _ScreenPos.y;
                    _interfaceY = _Interface.position.y;
                }
            }
            
        }
        


    }



    void PositionGem()
    {
        if(_hovered)
        {
            SocketedScript socket = _hovered.GetComponent<SocketedScript>();
            _hovered.parent.GetComponent<SpriteRenderer>().enabled = false;
            if (socket._socketed)
            {
                Destroy(_hovered.GetChild(0).gameObject);
                socket._socketed = false;
            }
            _gem.transform.position = _hovered.position;
            _gem.transform.parent = _hovered;
            _gem.GetComponent<Collider2D>().enabled = false;
            _hovered.GetComponent<SocketedScript>()._socketed = true;
            _MinorGems[socket._slot] = _gem.GetComponent<MinorGem>();

        }
        else
        {
            Destroy(_gem);
        }
        _gem = null;
        
    }

}
