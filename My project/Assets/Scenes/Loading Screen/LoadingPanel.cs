using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes.Loading_Screen
{
    public class LoadingPanel : MonoBehaviour
    {
        public Image progressBar;
        public TextMeshProUGUI  loadingText;
        private bool switchFlip;
        void Start()
        {
            StartCoroutine(LoadScene());
        }

        IEnumerator LoadScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Intro 1.1");

            while (!asyncLoad.isDone)
            {
                progressBar.fillAmount = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                yield return null;
            }
        }

        private void Update()
        {
            switchFlip = !switchFlip;
            loadingText.text = (switchFlip) ? "Loading .." : "Loading ...";
        }
    }
}
