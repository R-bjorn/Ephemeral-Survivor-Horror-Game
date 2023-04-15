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
                SceneManager.LoadScene(newSceneName);
            }
        }
    }
}
