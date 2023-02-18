using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuMainUI : MonoBehaviour
{
    [SerializeField][Tooltip("Audio upon clicking the button")]
    private AudioSource enterClip;
    public void MoveToScene(int scenetoChange)
    {
        enterClip.Play();
        SceneManager.LoadScene(scenetoChange);
    }

    public void ExitGame()
    {
        // if(Popup)
        Application.Quit();
    }
}
