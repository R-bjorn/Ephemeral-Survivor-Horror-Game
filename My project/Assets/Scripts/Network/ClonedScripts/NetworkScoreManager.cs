using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class NetworkScoreManager : NetworkBehaviour
{
    public static NetworkScoreManager Instance { get; private set; } // Singleton instance

    public int score; // player score
    public int numCrystalsToWin = 3; // number of keys player needs to win
    public TextMeshProUGUI crystalCounter;

    void Start()
    {
        GameObject crystalCounterObject = GameObject.Find("Crystal Count");
        crystalCounter = crystalCounterObject.GetComponent<TextMeshProUGUI>();
    }

    private void Awake()
    {
        Instance = this; // Set the singleton instance
    }

    public void AddScore(int scoreValue)
    {
        score += scoreValue; // Add the score value to the player's score
        crystalCounter.text = score.ToString();
        //CheckWinCondition(); // Check if the player has won the game
    }
}
