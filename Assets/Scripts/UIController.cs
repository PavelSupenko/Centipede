using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public enum EndType { Comleted, Failure};
    [SerializeField] private Text _logoText;
    [SerializeField] private Text _maxPointsText;
    [SerializeField] private GameObject _endWindow;
    [SerializeField] private GameObject _startWindow;
    [SerializeField] private Transform _centipede;
    [SerializeField] private GameObject _mainController;

    public static UIController Instance { get { return _instance; } }
    private static UIController _instance;

    private void Start()
    {
        InitializeWindows();
    }

    private void InitializeWindows()
    {
        if (SaveClass.IsFirstScene)
        {
            _startWindow.SetActive(true);
        }
        else
        {
            _startWindow.SetActive(false);
            _mainController.SetActive(true);
            _centipede.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        var arr = CentipedeController.controllers;
        if(arr != null && arr.Count <= 0)
        {
            ShowEndGameWindow(EndType.Comleted);
            SaveClass.SavePoints(_mainController.GetComponent<PointsController>().Points);
        }
    }

    public void ShowEndGameWindow(EndType type)
    {
        _endWindow.SetActive(true);
        switch(type)
        {
            case EndType.Comleted:
                _logoText.text = "Completed";
                break;
            case EndType.Failure:
                _logoText.text = "Game Over";
                break;
        }
        _maxPointsText.text = SaveClass.GetMaxPointValue().ToString();
        _mainController.SetActive(false);
        var arr = CentipedeController.controllers;
        foreach (CentipedeController cc in arr)
            cc.StopAllCoroutines();
    }

    public void Retry()
    {
        SaveClass.IsFirstScene = false;
        SceneManager.LoadScene("Main");
    }
}
