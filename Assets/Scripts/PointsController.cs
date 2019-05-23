using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsController : MonoBehaviour {

    [SerializeField] private Text HPText;
    [SerializeField] private Text PointsText;
    [SerializeField] private UIController _uiController;
    private int _hp = 100;
    private int _points = 0;
    private static PointsController _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        PrintValues();
    }

    public void OnValuesChanged()
    {
        PrintValues();
    }

    public static PointsController Instance
    {
        get { return _instance; }
    }

    public int Points
    {
        get
        {
            return _points;
        }
        set
        {
            _points = value;
            OnValuesChanged();
        }
    }

    public int HP {
        get
        {
            return _hp;
        }
        set
        {
            if(value < 0)
                _uiController.ShowEndGameWindow(UIController.EndType.Failure);

            _hp = Mathf.Clamp(value, 0, 100);
            OnValuesChanged();
        }
    }

    public void UpHealth(int hp)
    {
        HP += hp;
    }

    public void UpPoints(int p)
    {
        Points += p;
    }

    private void PrintValues()
    {
        HPText.text = _hp.ToString();
        PointsText.text = _points.ToString();
    }
}
