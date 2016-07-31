using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class CastleEditor : MonoBehaviour
{
    public static CastleEditor _CastleEditor;

    /*
    [System.Serializable]
    public class CastlePiece : System.Object    // Class contenant les données des pièces du chateau, utilisé pour les SAVE / LOAD (Position, Rotation et ID)
    {
        public int tabSize;
        public float[] positionX;    // Position stocké pour SAVE / LOAD
        public float[] positionY;
        public float[] rotation;      // Rotation sauvée
        public int[] ID;              // ID de la pièce par rapport au tableau de pièces TabShapes
    }
    */

    public GameObject _EditorMenu;     
    public List<GameObject> _EditedPieces;  // Tableau des pièces du chateau actuellement en édition

    public Text _Show_Hide_Text;           // Text du bouton d'activation du menu d'affichage des pièces
    private bool _Showed = true;

    private GameObject[] _TabShapes;    // Liste contenant toutes les pièces possibles, place dans le tableau = ID 

    //private Transform _ControledPiece;  // Pièce actuellement contrôlée par le joueur
    //private bool _CTRL;                 // Boolean true si une pièce est sélectionnée
    private Camera _MCam;               // Main Camera

    void Awake()
    {
        _CastleEditor = this;
    }

    // Use this for initialization
    void Start()
    {
        _EditedPieces = new List<GameObject>();
        _EditorMenu.SetActive(true);
        _TabShapes = AllShapes._AllShapes._TabShapes;
        _MCam = Camera.main;
       // _ControledPiece = null;
        //_CTRL = false;
        if (!_Showed)
        {
            _Show_Hide_Text.text = "SHOW\nEDITOR";
            _Show_Hide_Text.color = Color.green;
        }
	
	}
	
	// Update is called once per frame
	void Update ()
    {  
       /* 
        if (Input.GetMouseButtonUp(0) && _ControledPiece!= null)
        {
            _CTRL = false;
            // Déja présente dans UnlockRotation.cs
            // _ControledPiece.GetComponent<Rigidbody2D>().gravityScale = 1;
            //_ControledPiece.GetComponent<Collider2D>().enabled = true;
            //_ControledPiece = null;
        }
        /*
            if (_CTRL)
        {
            _CursorPos = _MCam.ScreenToWorldPoint(Input.mousePosition);
            _ControledPiece.position = new Vector3 (_CursorPos.x, _CursorPos.y, 0) ;

        }
       */
	}

    public void ShowHideEditor()
    {
        if (!_Showed)   //Editor non visible
        {
            _Show_Hide_Text.text = "HIDE\nEDITOR";
            _Show_Hide_Text.color = Color.red;
            _Showed = true;
            _EditorMenu.SetActive(true);
        }
        else            //Editor visible
        {
            _Show_Hide_Text.text = "SHOW\nEDITOR";
            _Show_Hide_Text.color = Color.green;
            _Showed = false;
            _EditorMenu.SetActive(false);
        }
    }

    public void CreatePiece(int ID)
    {
        GameObject Piece = GameObject.Instantiate(_TabShapes[ID], new Vector3 (_MCam.transform.position.x, _MCam.transform.position.y, 0), Quaternion.identity) as GameObject;
        Piece.transform.parent = transform;
        _EditedPieces.Add(Piece);
        SelectedPiece._selected = Piece;
        PiecesManager._PiecesManager.SelectPiece(Piece);
       // Piece.GetComponent<Rigidbody2D>().gravityScale = 0; // Gravité désactivé
       // Piece.GetComponent<Collider2D>().enabled = false;
        //_ControledPiece = Piece.transform;
       // _CTRL = true;
    }


    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(Application.persistentDataPath + "/CastleSave.dat");
        FileStream file = File.Create(Application.dataPath + "/CastleSave.dat");
        CastlePiece _castle = new CastlePiece();
        GameObject[] AllPieces = GameObject.FindGameObjectsWithTag("CastlePiece");
            _castle.tabSize = AllPieces.Length;
            _castle.positionX = new float[AllPieces.Length];
            _castle.positionY = new float[AllPieces.Length];
            _castle.rotation = new float[AllPieces.Length];
            _castle.ID = new int[AllPieces.Length];

        for (int i = 0; i < AllPieces.Length; i++)
        {
            _castle.positionX[i] = AllPieces[i].transform.position.x;
            _castle.positionY[i] = AllPieces[i].transform.position.y;
            _castle.rotation[i] = AllPieces[i].transform.eulerAngles.z;
            _castle.ID[i] = AllPieces[i].GetComponent<MyPieceID>().MyID;
        }
        bf.Serialize(file, _castle);
        file.Close();
    }

    public void ClearCastle()
    {
        GameObject[] AllPieces = GameObject.FindGameObjectsWithTag("CastlePiece");
        _EditedPieces.Clear();
        foreach (GameObject go in AllPieces)
        {
            GameObject.Destroy(go);
        }
    }

    public void Load()
    {

        ClearCastle();

        if (File.Exists(Application.dataPath + "/CastleSave.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/CastleSave.dat", FileMode.Open);
            CastlePiece _castle = new CastlePiece();
            _castle = (CastlePiece)bf.Deserialize(file);
            file.Close();
            for (int i = 0; i < _castle.tabSize; i++)
            {
                Vector3 pos = new Vector3(_castle.positionX[i], _castle.positionY[i], 0);
                Vector3 rot = new Vector3(0, 0, _castle.rotation[i]);
                int _id = _castle.ID[i];
                GameObject _piece = GameObject.Instantiate(_TabShapes[_id], pos, Quaternion.identity) as GameObject;
                _piece.transform.eulerAngles = rot;
                _EditedPieces.Add(_piece);
                _piece.transform.parent = transform;             
            }
            file.Close();
        }
    }

    public void SendCastleToDatabase()
    {
        if(DatabaseHandler._mysql)
        {
            DatabaseHandler._mysql.SendCastleToDatabase();
        }
        else
        {
            Debug.Log("There is no DatabaseHandler");
        }
    }

    public void GetCastleFromDatabase(int userID)
    {
        if (DatabaseHandler._mysql)
        {
            DatabaseHandler._mysql.GetCastleFromDatabase(DatabaseHandler._mysql.GetMyID());
        }
        else
        {
            Debug.Log("There is no DatabaseHandler");
        }
    }
}


