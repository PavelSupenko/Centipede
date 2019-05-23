using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public enum EndType { Comleted, Failure};
    [SerializeField] private Text _logoText;
    [SerializeField] private Text _maxPointsTextStart;
    [SerializeField] private Text _maxPointsText;
    [SerializeField] private GameObject _endWindow;
    [SerializeField] private GameObject _startWindow;
    [SerializeField] private Transform _centipede;
    [SerializeField] private GameObject _mainController;
    [SerializeField] private SaveController _saveController;

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
    }

    private void InitializeWindows()
    {
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
            _centipede.gameObject.SetActive(true);
            Messenger.Broadcast(EventStrings.START_HARD_MUSIC);
        }
    }

    public void OnGameOver()
    {
        ShowEndGameWindow(EndType.Failure);
    }

    public void OnGameComplete()
    {
        ShowEndGameWindow(EndType.Comleted);
    }

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
        _mainController.SetActive(false);
        var arr = CentipedeController.controllers;
        foreach (CentipedeController cc in arr)
            if(cc != null)
                cc.StopAllCoroutines();

        _saveController.SavePoints(_mainController.GetComponent<PointsController>().Points);
    }

    public void Retry()
    {
        GlobalVariables.IS_FIRST_SCENE = false;
        SceneManager.LoadScene("Main");
    }
}
