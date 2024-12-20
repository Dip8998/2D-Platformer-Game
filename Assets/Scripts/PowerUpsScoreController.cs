using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PowerUpsScoreController : MonoBehaviour
{
    public TextMeshProUGUI jumpScore;
    public TextMeshProUGUI shieldScore;

    public int jScore = 0;
    public int sScore = 0;

    private void Awake()
    {
        ResetJumpUI();
        ResetShieldUI();
    }

    public void JumpCountForUI(int increment)
    {
        jScore += increment;
        ResetJumpUI();
    }
    public void JumpScoreDecreament(int decrement)
    {
        jScore -= decrement;
        ResetJumpUI();
    }
    public void ResetJumpUI()
    {
        jumpScore.text = " : " + jScore;
    }
    public void ShieldScoreIncrease(int increment)
    {
        sScore += increment;
        ResetShieldUI();
    }
    public void ShieldScoreDecrease(int decrement)
    {
        sScore -= decrement;
        ResetShieldUI();
    }
    public void ResetShieldUI()
    {
        shieldScore.text = " : " + sScore;
    }
}
