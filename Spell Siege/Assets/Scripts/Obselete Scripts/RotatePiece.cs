using UnityEngine;
using System.Collections;

public class RotatePiece : MonoBehaviour {

    private bool _rotActive;

	void OnMouseDown()
    {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        _rotActive = true;
        SelectedPiece._selected.transform.parent = transform;
    }
    
	void Start ()
    {
        _rotActive = false;
	}
	
	void Update ()
    {
        if (_rotActive)
        {
            Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }

	    if(Input.GetMouseButtonUp(0))
        {
            if (SelectedPiece._selected)
            {
                SelectedPiece._selected.transform.parent = GameObject.FindGameObjectWithTag("EditorScripts").transform;
            }
            _rotActive = false;
        }
	}
}
