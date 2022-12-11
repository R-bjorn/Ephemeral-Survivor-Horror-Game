using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuControler : MonoBehaviour
{
    [Header("Levels to Load")] 
    public string _newGameLevel;
    public string levelToLoad;

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void MultiplayerDialogYes()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void Exitbutton()
    {
        Application.Quit();
    }

}
