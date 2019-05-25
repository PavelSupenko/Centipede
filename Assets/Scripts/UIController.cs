using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// The script controls UI Elements (text, endgame and start windows)
public class UIController : MonoBehaviour {
    public enum EndType { Comleted, Failure};
    [SerializeField] private Text _logoText;
    [SerializeField] private Text _maxPointsTextStart;
    [SerializeField] private Text _maxPointsText;
    [SerializeField] private Text _yourPointsText;
    [SerializeField] private GameObject _endWindow;
    [SerializeField] private GameObject _startWindow;
    [SerializeField] private Transform _centipede;
    [SerializeField] private GameObject _mainController;
    [SerializeField] private PointsController _pointController;
    [SerializeField] private SaveController _saveController;

    // Listeners for over and completed game
    private void Awake()
    {
        Messenger.AddListener(EventStrings.GAME_OVER, OnGameOver);
        Messenger.AddListener(EventStrings.GAME_COMPLETED, OnGameComplete);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(EventStrings.GAME_OVER, OnGameOver);
        Messenger.RemoveListener(EventStrings.GAME_COMPLETED, OnGameComplete);
    }

    private void Start()
    {
        InitializeWindows();
        OnGamePlayed();
    }

    // Show start window if app started for the
    // first time
    // Doesn`t show start window if we`ve already
    // completed previous level
    private void InitializeWindows()
    {
        GlobalVariables.IS_GAME_OVER = false;
        if (GlobalVariables.IS_FIRST_SCENE)
        {
            _startWindow.SetActive(true);
            _maxPointsTextStart.text = "MAX POINTS: " + _saveController.GetMaxPointValue().ToString();
            Messenger.Broadcast(EventStrings.START_SOFT_MUSIC);
        }
        else
        {
            _startWindow.SetActive(false);
            _mainController.SetActive(true);
            Messenger.Broadcast(EventStrings.START_HARD_MUSIC);
        }
    }

    public void OnGameOver()
    {
        if (!GlobalVariables.IS_GAME_OVER)
        {
            OnGamePaused();
            ShowEndGameWindow(EndType.Failure);
            GlobalVariables.IS_GAME_OVER = true;
        }
    }

    public void OnGameComplete()
    {
        if (!GlobalVariables.IS_GAME_OVER)
        {
            OnGamePaused();
            ShowEndGameWindow(EndType.Comleted);
            GlobalVariables.IS_GAME_OVER = true;
        }
    }
    
    // Pair methods to manipulate time scale
    // when the game paused
    public void OnGamePaused()
    {
        Time.timeScale = 0;
    }

    public void OnGamePlayed()
    {
        Time.timeScale = 1;
    }

    // Show final window with "Retry" button
    // and maximum of points you`ve collected
    public void ShowEndGameWindow(EndType type)
    {
        _endWindow.SetActive(true);
        switch (type)
        {
            case EndType.Comleted:
                _logoText.text = "Completed";
                break;
            case EndType.Failure:
                _logoText.text = "Game Over";
                break;
        }
        _maxPointsText.text = _saveController.GetMaxPointValue().ToString();
        _yourPointsText.text = _pointController.Points.ToString();
        _mainController.SetActive(false);

        var arr = CentipedeController.controllers;
        foreach (CentipedeController cc in arr)
            if (cc != null)
                cc.StopAllCoroutines();

        _saveController.SavePoints(_mainController.GetComponent<PointsController>().Points);
    }

    public void Retry()
    {
        GlobalVariables.IS_FIRST_SCENE = false;
        SceneManager.LoadScene("Main");
    }
}
