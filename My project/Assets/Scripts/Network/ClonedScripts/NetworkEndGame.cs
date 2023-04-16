using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class NetworkEndGame : NetworkBehaviour
{
    public string newSceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (NetworkScoreManager.Instance.score >= 3)
            {
                Debug.Log("Won!");
                SceneManager.LoadScene(newSceneName);
            }
            else
            {
                Debug.Log("You must find more crystals to win!");
            }
        }
    }
}
