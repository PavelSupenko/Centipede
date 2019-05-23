using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsController : MonoBehaviour {

    [SerializeField] private Text HPText;
    [SerializeField] private Text PointsText;
    private int _hp = 100;
    private int _points = 0;

    private void Awake()
    {
        Messenger<int>.AddListener(EventStrings.UP_HEALTH, UpHealth);
        Messenger<int>.AddListener(EventStrings.UP_POINTS, UpPoints);
    }

    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(EventStrings.UP_HEALTH, UpHealth);
        Messenger<int>.RemoveListener(EventStrings.UP_POINTS, UpPoints);
    }

    private void Start()
    {
        PrintValues();
    }

    public void OnValuesChanged()
    {
        PrintValues();
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
                Messenger.Broadcast(EventStrings.GAME_OVER);

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
