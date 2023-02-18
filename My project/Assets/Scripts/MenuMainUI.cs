using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuMainUI : MonoBehaviour
{
    public void MoveToScene(int scenetoChange)
    {
        SceneManager.LoadScene(scenetoChange);
    }

    public void ExitGame()
    {
        // if(Popup)
        Application.Quit();
    }
}
