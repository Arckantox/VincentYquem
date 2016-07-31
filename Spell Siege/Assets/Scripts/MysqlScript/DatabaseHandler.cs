using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;



public class DatabaseHandler : MonoBehaviour
{
    public static DatabaseHandler _mysql;

    private string host, database, user, password;

    public bool pooling = true;

    private string connexionstring;
    private MySqlConnection con = null;
    private MySqlCommand cmd = null;
    private MySqlDataReader rdr = null;


    private string userName = "";
    private string userPass = "";

    private string myUserName;
    public int myID;

    private bool _showConnexionGUI = true;
    private bool _showCreateUserGUI = true;
    private bool _showConnectUserGUI = true;

    public Text _errortxt;


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        _mysql = this;

        host = "sql7.freemysqlhosting.net";
        database = "sql7129042";
        user = "sql7129042";
        password = "IJa7FZdQts";

        con = new MySqlConnection();

        _errortxt.text = "";

    }

    void OnApplicationQuit()
    {
        if(con != null)
        {
            if(con.State.ToString() != "Closed")
            {
                con.Close();
                Debug.Log("MySQL Connection Closed");
            }
            con.Dispose();
        }
    }

    void OnGUI()
    {
        if(_showConnexionGUI)
        {
            //Connexion à la database
            GUI.BeginGroup(new Rect(75, 20, 200, 200), "");
            GUI.Box(new Rect(0, 0, 190, 190), "Connexion");
            host = GUI.TextField(new Rect(10, 30, 170, 20), host, 50);
            database = GUI.TextField(new Rect(10, 60, 170, 20), database, 20);
            user = GUI.TextField(new Rect(10, 90, 170, 20), user, 20);
            password = GUI.PasswordField(new Rect(10, 120, 170, 20), password, "*"[0], 20);

            if (GUI.Button(new Rect(10, 150, 170, 20), "Connecter") && host != "" && database != "" && user != "" && password != "")
            {
                DatabaseConnexion();
            }
            GUI.EndGroup();
        }

        if (_showCreateUserGUI)
        {
            //Création d'user
            GUI.BeginGroup(new Rect(275, 20, 200, 200), "");
            GUI.Box(new Rect(0, 0, 190, 190), "Create User");
            userName = GUI.TextField(new Rect(10, 30, 170, 20), userName, 20);
            userPass = GUI.PasswordField(new Rect(10, 60, 170, 20), userPass, "*"[0], 20);

            if (GUI.Button(new Rect(10, 90, 170, 20), "Créer") && userName != "" && userPass != "")
            {
                CreateAccount(userName, userPass);
            }
            GUI.EndGroup();
        }

        if (_showConnectUserGUI)
        {
            //Création d'user
            GUI.BeginGroup(new Rect(475, 20, 200, 200), "");
            GUI.Box(new Rect(0, 0, 190, 190), "Connect User");
            userName = GUI.TextField(new Rect(10, 30, 170, 20), userName, 20);
            userPass = GUI.PasswordField(new Rect(10, 60, 170, 20), userPass, "‡"[0], 20);

            if (GUI.Button(new Rect(10, 90, 170, 20), "Connexion") && userName != "" && userPass != "")
            {
                ConnexionAccount(userName, userPass);
            }
            GUI.EndGroup();
        }

    }

    public void ShowHideConGUI()
    {
        if(_showConnexionGUI)
        {
            _showConnexionGUI = false;
        }
        else
        {
            _showConnexionGUI = true;
        }
    }

    public void ShowHideCreateUserGUI()
    {
        if (_showCreateUserGUI)
        {
            _showCreateUserGUI = false;
        }
        else
        {
            _showCreateUserGUI = true;
        }
    }

    public void ShowHideConnectUserGUI()
    {
        if (_showConnectUserGUI)
        {
            _showConnectUserGUI = false;
        }
        else
        {
            _showConnectUserGUI = true;
        }
    }

    public void DatabaseConnexion()
    {
        //test
        _errortxt.text = "Database Connexion";

        if (con.State.ToString() == "Open")
        {
            con.Close();
        }

        connexionstring = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Pooling=";

        if (pooling)
        {
            connexionstring += "true;";
        }
        else
        {
            connexionstring += "false;";
        }
        try
        {
            con = new MySqlConnection(connexionstring);
            con.Open();
            //test
            _errortxt.text = "Mysql State: " + con.State;
            Debug.Log("Mysql State: " + con.State);
            ShowHideConGUI();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            //test
            _errortxt.text = e.ToString();
        }
    }

    void ConnexionAccount(string name, string pass)
    {
        string checkPass;
        if (con.State.ToString() == "Open")
        {
            cmd = new MySqlCommand("SELECT nameusers FROM users WHERE nameusers='" + userName + "';", con);
            try
            {
                rdr = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            rdr.Read();
            if (!rdr.HasRows) // Vérifie si il y a eu une réponse valide pour la recherche du nom entré, nom existe
            {
                _errortxt.text = "Wrong User/Password";
                rdr.Close();
                return;
            }
            else // Nom existant, donc vérification du MDP entré
            {
                rdr.Close();
                cmd = new MySqlCommand("SELECT passusers FROM users WHERE nameusers='" + name + "';", con);

                try
                {
                    rdr = cmd.ExecuteReader();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    //test
                    _errortxt.text = e.ToString();
                }

                rdr.Read();

                checkPass = rdr.GetString("passusers");

                rdr.Close();

                if (checkPass == pass)
                {
                    cmd = new MySqlCommand("SELECT nameusers FROM users WHERE nameusers='" + name + "';", con);
                    try
                    {
                        rdr.Close();
                        rdr = cmd.ExecuteReader();
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                    rdr.Read();

                    myUserName = rdr.GetString("nameusers");
                    rdr.Close();
                    myID = GetMyID();
                    myID = GetMyID();
                    Debug.Log("Connected as : " + myUserName + " | ID : " + myID);
                    //test
                    _errortxt.text = "Connected as : " + myUserName + " | ID : " + myID;

                    ShowHideConnectUserGUI();
                    ShowHideCreateUserGUI();

                }
                else
                {
                    Debug.Log("Wrong User/Password");
                    //test
                    _errortxt.text = "Wrong User/Password";
                }
            }


            
        }
        else
        {
            Debug.Log("Can't connect User if Not Connected to database");
            //test
            _errortxt.text = "Can't connect User if Not Connected to database";
        }
    }

    void CreateAccount(string name, string pass)
    {
        if(con.State.ToString() == "Open")
        {
            //Check if name entered already exist

            cmd = new MySqlCommand("SELECT nameusers FROM users WHERE nameusers='" + name + "';", con);
            try
            {
                rdr = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            rdr.Read();
           if(rdr.HasRows) // Vérifie si il y a eu une réponse valide pour la recherche du nom entré, signifiant que le nom est déjà utilisé pour un autre compte
            {
                _errortxt.text = "Name already taken";
                rdr.Close();
                return;
            }
           else // Nom non utilisé donc création de compte
            {
                rdr.Close();
                cmd = new MySqlCommand("INSERT INTO users VALUES(default,'" + name + "','" + pass + "',default, default)", con);
                try
                {
                    rdr = cmd.ExecuteReader();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                rdr.Close();
                Debug.Log("Created account : " + name + " | Password : " + pass);
                myUserName = name;

                //test
                _errortxt.text = "Created account : " + name + " | Password : " + pass;
                ShowHideCreateUserGUI();
                ShowHideConnectUserGUI();
            }

            
        }
        else
        {
            Debug.Log("Can't create User if Not Connected to database");
            //test
            _errortxt.text = "Can't create User if Not Connected to database";
        }
    }

    public int GetMyID()
    {
        if (con.State.ToString() == "Open")
        {
            int myiD;

            cmd = new MySqlCommand("SELECT idusers FROM users WHERE nameusers='" + userName + "';", con);

            try
            {
                rdr = cmd.ExecuteReader();
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }

            rdr.Read();

            myiD = rdr.GetInt32("idusers");

            rdr.Close();
            return myiD;
        }
        Debug.Log("Not Connected");
        return 3;
    }

    public void SendCastleToDatabase()
    {
        CastleEditor._CastleEditor.Save();

        string SQL;
        UInt32 FileSize;
        byte[] rawData;
        FileStream fs;

        try
        {
            fs = new FileStream(Application.dataPath + "/CastleSave.dat", FileMode.Open, FileAccess.Read);
            FileSize = (uint)fs.Length;


            rawData = new byte[FileSize];
            fs.Read(rawData, 0, (int)FileSize);
            fs.Close();


            SQL = "UPDATE users SET castledatasize=@FileSize, castledata=@File WHERE idusers=@ID;";

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@FileSize", FileSize);
            cmd.Parameters.AddWithValue("@File", rawData);
            cmd.Parameters.AddWithValue("@ID", myID);

            cmd.ExecuteNonQuery();
            
        }catch(Exception e)
        {
            _errortxt.text = e.ToString();
            Debug.Log(e);
        }


    }


    public void GetCastleFromDatabase(int userID)
    {
        // a supprimer
        userID = myID;
        // 
        string SQL;
        UInt32 FileSize;
        byte[] rawData;
        FileStream fs;

        SQL = "SELECT castledatasize, castledata FROM users WHERE idusers=" + userID + ";";

        try
        {

            cmd.CommandText = SQL;

            rdr = cmd.ExecuteReader();

            if (!rdr.HasRows)
            {
                Debug.Log(("There are no BLOBs to load"));
            }

            rdr.Read();

            FileSize = rdr.GetUInt32(rdr.GetOrdinal("castledatasize"));
            rawData = new byte[FileSize];

            rdr.GetBytes(rdr.GetOrdinal("castledata"), 0, rawData, 0, (int)FileSize);

            fs = new FileStream(Application.dataPath + "/CastleSave.dat", FileMode.OpenOrCreate, FileAccess.Write);
            fs.Write(rawData, 0, (int)FileSize);
            fs.Close();

            rdr.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            _errortxt.text = e.ToString();
        }

        if (CastleEditor._CastleEditor)
        {
            CastleEditor._CastleEditor.Load();
        }
        if(CastleAttack._CastleAttack)
        {
            CastleAttack._CastleAttack.Load();
        }
    }

    void Update()
    {
       // A supprimer
        if(Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log(myID = GetMyID());
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            ShowHideCreateUserGUI();
            ShowHideConnectUserGUI();
        }
    }

}


