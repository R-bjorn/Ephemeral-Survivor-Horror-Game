using UnityEngine;
using UnityEngine.SceneManagement;

namespace End_Point
{
    public class EndPoint : MonoBehaviour
    {
        public string newSceneName;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (ScoreManager.Instance.score >= 3)
                {
                    SceneManager.LoadScene(newSceneName);
                }
                else
                {
                    Debug.Log("You must find more crystals to win!");
                }
            }
        }
    }
}
