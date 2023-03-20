using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private UIManager _scoreUI;
    [SerializeField] private UIManager _timerUI;
    [SerializeField] private UIManager _livesUI;
    [SerializeField] private int _maxLives = 3;
    [SerializeField] private float _levelStartTime = 60f;

    [SerializeField] private string loseScene = "Lose";
    [SerializeField] private string winScene = "Win";

    private int _score = 0;
    private float _timer = 0f;
    private int _lives = 0;
    
    //Singleton setup
    //allow classes to reference but not alter
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartGame();
        //Hide cursor
        Cursor.visible = false;
        
        //Lock cursor to center
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Physics.IgnoreLayerCollision(7, 8);
    }

    public void ResetGame()
    {
        //reset values
        _score = 0;
        _timer = _levelStartTime;
        _lives = _maxLives;
        
        //reset ui
        _scoreUI.UpdateUI(_score);
        _timerUI.UpdateUI(_timer);
        _livesUI.UpdateUI(_lives);
    }

    public void StartGame()
    {
        ResetGame();
        StartCoroutine(Countdown());
    }

    public void EndGame()
    {
        Debug.Log("you lose");
        StopCoroutine(Countdown());
        SceneSwapper.LoadScene(loseScene);
        //Show cursor
        Cursor.visible = true;
        
        //Unlock cursor
        Cursor.lockState = CursorLockMode.Confined;
    }

    //pass in score values
    public void UpdateScore(int value)
    {
        //evaluate new score
        _score += value;
        
        //update ui with new score
        _scoreUI.UpdateUI(_score);
    }

    //pass in timer values
    public void UpdateTimer(float value)
    {
        //update new timer
        _timer += value;
        
        //update ui with new time
        _timerUI.UpdateUI(_timer);
    }

    public void UpdateLives(int value)
    {
        _lives += value;
        if (_lives <= 0)
        {
            EndGame();
        }
        
        _livesUI.UpdateUI(_lives);
    }

    public void GameWon()
    {
        PlayerPrefs.SetInt("HighScore", _score);
        SceneSwapper.LoadScene(winScene);
        //Show cursor
        Cursor.visible = true;
        
        //Unlock cursor
        Cursor.lockState = CursorLockMode.Confined;
    }
    
    //create timer
    IEnumerator Countdown()
    {
        while (_timer > 0)
        {
            _timer--;
            _timerUI.UpdateUI(_timer);
            yield return new WaitForSeconds(1f);
        }

        _timer = 0f;
        _timerUI.UpdateUI(_timer);
        EndGame();
    }
    
}
