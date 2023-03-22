using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenuDisplay : MonoBehaviour
    {
        // [Header("Settings")]
        // [SerializeField] private string gameSceneName = "CharacterSelection";
    
        public void Singleplayer()
        {
            ServerManager.Instance.StartHost();
        }
    
        public void Multiplayer()
        {
            SceneManager.LoadScene("MultiplayerMenu");
        }

        public void StartHost()
        {
            ServerManager.Instance.StartHost();
        }

        public void StartServer()
        {
            ServerManager.Instance.StartServer();
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

        public void GoBack()
        {
            SceneManager.LoadScene("MainMenu");
        }
    
    
    
    }
}





