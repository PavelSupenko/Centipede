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
    [SerializeField] private AudioController _audioController;
    [SerializeField] private AudioClip _endClip;

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
            _audioController.OnStart();
        }
        else
        {
            _startWindow.SetActive(false);
            _mainController.SetActive(true);
            _centipede.gameObject.SetActive(true);
            _audioController.OnPlay();
        }
    }

    private void Update()
    {
        var arr = CentipedeController.controllers;
        if(arr != null && arr.Count <= 0)
        {
            ShowEndGameWindow(EndType.Comleted);
        }
    }

    public void ShowEndGameWindow(EndType type, string str = "")
    {
        _endWindow.SetActive(true);
        switch (type)
        {
            case EndType.Comleted:
                _logoText.text = "Completed" + str; ;
                break;
            case EndType.Failure:
                _logoText.text = "Game Over" + str;
                break;
        }
        _maxPointsText.text = SaveClass.GetMaxPointValue().ToString();
        _mainController.SetActive(false);
        var arr = CentipedeController.controllers;
        foreach (CentipedeController cc in arr)
            if(cc != null)
                cc.StopAllCoroutines();

        _audioController.GetComponent<AudioSource>().PlayOneShot(_endClip);
        SaveClass.SavePoints(_mainController.GetComponent<PointsController>().Points);
    }

    public void Retry()
    {
        SaveClass.IsFirstScene = false;
        SceneManager.LoadScene("Main");
    }
}
