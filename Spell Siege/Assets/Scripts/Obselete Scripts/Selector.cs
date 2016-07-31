using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {

	
    void OnMouseDown()
    {
        if(SelectedPiece._selected)
        {
            SelectedPiece._selected.GetComponent<UnlockRotation>().OnClickAway();
        }
        SelectedPiece._selected = gameObject;
        GetComponent<UnlockRotation>().enabled = true;
        GetComponent<SpriteRenderer>().sortingOrder = 50;
    }
    
}
