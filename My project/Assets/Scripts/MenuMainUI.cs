using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMainUI : MonoBehaviour
{

    public void MoveToIntro1_0(int scenetoChange)
    {
        Application.LoadLevel(scenetoChange);
    }

    public void ExitGame()
    {
        // if(Popup)
        Application.Quit();
    }
}
