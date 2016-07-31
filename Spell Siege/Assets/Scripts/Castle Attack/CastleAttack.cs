using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class CastleAttack : MonoBehaviour
{

    public static CastleAttack _CastleAttack;

    private GameObject[] _TabShapes;    // Liste contenant toutes les pièces possibles, place dans le tableau = ID

    public List<GameObject> _EditedPieces;  // Tableau des pièces du chateau actuellement en édition


    void Awake()
    {
        _CastleAttack = this;
    }

    void Start()
    {
        _TabShapes = AllShapes._AllShapes._TabShapes;

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
        //Load pas avant le Start()
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


    void Update()
    {

    }
}

