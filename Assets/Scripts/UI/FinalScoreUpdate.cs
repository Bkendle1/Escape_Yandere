using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScoreUpdate : MonoBehaviour
{
    [SerializeField] private UIManager finalScore;

    private void Start()
    {
        finalScore.UpdateUI(PlayerPrefs.GetInt("HighScore"));
    }
}
