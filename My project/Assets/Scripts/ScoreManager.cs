using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; } // Singleton instance

    public int score; // player score
    public int numCrystalsToWin = 3; // number of keys player needs to win

    private void Awake()
    {
        Instance = this; // Set the singleton instance
    }

    public void AddScore(int scoreValue)
    {
        score += scoreValue; // Add the score value to the player's score
        //CheckWinCondition(); // Check if the player has won the game
    }

    /*
    private void CheckWinCondition()
    {
        if (score >= numKeysToWin) // Check if the player has collected enough coins to win
        {
            Time.timeScale = 0f; // Pause the game
            Debug.Log("won");
            // winMessage.SetActive(true); // Show the win message
        }
    }
    */
}
