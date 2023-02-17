using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMainUI : MonoBehaviour
{

    public void MoveToIntro1_0(int scenetoChange)
    {
        SceneManager.LoadScene(scenetoChange);
    }

    public void ExitGame()
    {
        // if(Popup)
        Application.Quit();
    }
}
