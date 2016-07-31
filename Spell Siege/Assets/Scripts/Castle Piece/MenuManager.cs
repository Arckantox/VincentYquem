using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {


    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnMenu();
        }
    }

}
