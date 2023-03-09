using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MainMenuDisplay : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string gameSceneName = "PlayerInput";
    
    public void Singleplayer()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
    }
    
    public void Multiplayer()
    {
        SceneManager.LoadScene("MultiplayerMenu");
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        NetworkManager.Singleton.SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void Settings()
    {
        Debug.Log("Test");
    }

    public void Quit()
    {
        Debug.Log("Quit");
    }
}
