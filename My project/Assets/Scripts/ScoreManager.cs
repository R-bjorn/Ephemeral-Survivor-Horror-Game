using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; } // Singleton instance

    public int score; // player score
    public int numKeysToWin = 3; // number of keys player needs to win

    // public Text scoreText; // The UI text object that displays the player's score
    // public GameObject winMessage; // The UI message that displays when the player wins

    private void Awake()
    {
        Instance = this; // Set the singleton instance
    }

    public void AddScore(int scoreValue)
    {
        score += scoreValue; // Add the score value to the player's score
        // scoreText.text = "Score: " + score.ToString(); // Update the score text
        Debug.Log("score +");
        CheckWinCondition(); // Check if the player has won the game
    }

    private void CheckWinCondition()
    {
        if (score >= numKeysToWin) // Check if the player has collected enough coins to win
        {
            Time.timeScale = 0f; // Pause the game
            Debug.Log("won");
            // winMessage.SetActive(true); // Show the win message
        }
    }
}
