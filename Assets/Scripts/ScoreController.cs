using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private int score = 0;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        ResetUI();
    }

    public void IncreaseScore(int increment)
    {
        score += increment;
        ResetUI();
    }

    private void ResetUI()
    {
        scoreText.text = "Score : " + score;
    }
}
