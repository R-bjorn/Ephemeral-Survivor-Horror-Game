using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuMainUI : MonoBehaviour
{
    // [SerializeField][Tooltip("Audio play upon mouse enter")]
    // private AudioClip mouseEnter;
    // [SerializeField][Tooltip("Audio play upon mouse click ")]
    // private AudioClip mouseClick;
    //
    // private bool _buttonClicked = false;
    // private AudioSource _audioSource;
    //
    // private void Start()
    // {
    //     _audioSource = GetComponent<AudioSource>();
    // }
    //
    // private void OnMouseOver()
    // {
    //     Debug.Log("Enter mouse");
    //     if (!_buttonClicked)
    //     {
    //         _audioSource.clip = mouseEnter;
    //         _audioSource.Play();
    //     }
    // }
    //
    // private void OnMouseDown()
    // {
    //     _buttonClicked = true;
    //     _audioSource.clip = mouseClick;
    //     _audioSource.Play();
    // }
    //
    // private void OnMouseUp()
    // {
    //     _buttonClicked = false;
    // }

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
