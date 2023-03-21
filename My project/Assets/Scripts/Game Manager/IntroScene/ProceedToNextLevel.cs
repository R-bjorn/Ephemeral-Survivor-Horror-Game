using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game_Manager.IntroScene
{
    public class ProceedToNextLevel : MonoBehaviour
    {
        public string nextSceneName = "Level1.0";

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
