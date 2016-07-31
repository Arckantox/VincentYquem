
using UnityEngine;
using System.Collections;

public class UnlockRotation : MonoBehaviour {

    private bool _draging;
    private Transform _rotator;
    private Rigidbody2D _RB;

    void Awake()
    {
        _RB = GetComponent<Rigidbody2D>();
        _rotator = GameObject.FindGameObjectWithTag("Rotator").transform;
    }

    void OnEnable()
    {
       /* if(SelectedPiece._previouslySelected)
            SelectedPiece._previouslySelected.GetComponent<UnlockRotation>().OnClickAway();
        */
        //SelectedPiece._selected = gameObject;
        if(SelectedPiece._selected == gameObject)
        {
            _RB.freezeRotation = true;
            GetComponent<Collider2D>().enabled = false;
            _RB.gravityScale = 0;
            _RB.velocity = Vector2.zero;
            _draging = false;

            _rotator.position = transform.position;
        }

    }


    //Fonction pour tourner

    // Cliquer ailleur pour faire tomber (activer gravité + collider)

    public void OnClickAway()
    {
        GetComponent<Collider2D>().enabled = true;
        _RB.gravityScale = 1;
        SelectedPiece._previouslySelected = gameObject;
        SelectedPiece._selected = null;
        _rotator.position = Vector3.one * 1000;
        GetComponent<SpriteRenderer>().sortingOrder = 0;
        _draging = false;

    }

    
    void OnCollisionStay2D(Collision2D coll)
    {
        _RB.freezeRotation = false;
        this.enabled = false;
    }



    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 ScreenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float dist = Vector3.Distance(new Vector3(ScreenPos.x, ScreenPos.y, 0), transform.position);
            if (dist > 5)
            {
                if(enabled)
                {
                    OnClickAway();
                }
                _draging = false;
            }

            if (dist < 2 && (SelectedPiece._selected == gameObject))
            {
                _draging = true;
                _rotator.position = Vector3.one * 1000;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _draging = false;
            if(SelectedPiece._selected == gameObject)
            {
                _rotator.position = transform.position;
            }
        }

            if (_draging)
        {
            Vector3 ScreenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(ScreenPos.x, ScreenPos.y, 0);
        }
    }

}
