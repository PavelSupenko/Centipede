using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldController : MonoBehaviour {

    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private GameObject _emptyWallPrefab;
    [SerializeField] private Transform _wallsParent;
    [SerializeField] private UIController _uiController;
    [SerializeField] private CentipedeSpawner _spawner;
    [SerializeField] private int _startSectionCount;
    [SerializeField] private float _positionUpdateTime;
    private float _cellSize;
    private int extraCellsCount = 2;
    private int indexOfFirstRow;
    public Vector2Int CellCount;
    public Vector3 CellSize { get { return new Vector3(_cellSize, _cellSize, _cellSize); } }
    [Range(0f,1f)] public float Probability;

    public Matrix<WallComponent> FieldMatrix;

    private static FieldController _instance;
    public static FieldController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            DestroyImmediate(this.gameObject);
    }

    void Start ()
    {
        BuildField();
        CreateNewCentipede();
    }

    public void CreateNewCentipede()
    {
        //if(_startSectionCount <= 0)
        //    _uiController.ShowEndGameWindow(UIController.EndType.Comleted);

        //_spawner.CreateNewCentipede(_startSectionCount, _positionUpdateTime);
        //_startSectionCount-=2;
        //_positionUpdateTime *= 0.8f;

        if (_startSectionCount > 0)
        {
            _spawner.CreateNewCentipede(_startSectionCount, _positionUpdateTime);
            _startSectionCount -= 2;
            _positionUpdateTime *= 0.8f;
            return;
        }
        _uiController.ShowEndGameWindow(UIController.EndType.Comleted);
    }
    
    private void BuildField()
    {
        indexOfFirstRow = CellCount.y / 10;
        FieldMatrix = new Matrix<WallComponent>(CellCount.x, CellCount.y + 10);
        Vector3 position;
        _cellSize = (_camera.ViewportToWorldPoint(new Vector3(1, 0)) - _camera.ViewportToWorldPoint(new Vector3(0, 0))).x / CellCount.x;
        float cellSize = _cellSize * 0.4f;
        float shiftX = 1f / CellCount.x;
        float shiftY = 1f / CellCount.y;
        float randomNumber;

        for (int i = 0; i < CellCount.x; i++)
        {
            for (int j = 0; j < CellCount.y; j++)
            {
                if (j >= indexOfFirstRow)
                    randomNumber = Random.value;
                else
                    randomNumber = 1;

                position = _camera.ViewportToWorldPoint(new Vector3(i * shiftX + shiftX / 2, j * shiftY + shiftY / 2, 5));
                GameObject go = Instantiate(
                    (randomNumber <= Probability) ? _wallPrefab : _emptyWallPrefab,
                    position,
                    Quaternion.identity,
                    _wallsParent);

                go.transform.localScale = new Vector3(cellSize, cellSize);
                WallComponent wc = go.GetComponent<WallComponent>();
                wc.SetFieldValues(i, j, randomNumber <= Probability);

                FieldMatrix.SetValueTo(i, j, wc);
            }

            for (int j = CellCount.y; j < CellCount.y + extraCellsCount; j++)
            {
                position = _camera.ViewportToWorldPoint(new Vector3(i * shiftX + shiftX / 2, j * shiftY + shiftY / 2, 5));
                GameObject go = Instantiate(
                    _emptyWallPrefab,
                    position,
                    Quaternion.identity,
                    _wallsParent);

                go.transform.localScale = new Vector3(cellSize, cellSize);
                WallComponent wc = go.GetComponent<WallComponent>();
                wc.SetFieldValues(i, j, false);

                FieldMatrix.SetValueTo(i, j, wc);
            }
        }
    }

    public WallComponent GetNearestPoint(Vector2 position)
    {
        IEnumerable<WallComponent> values = FieldMatrix.Field.Cast<WallComponent>();
        WallComponent nearestPoint = values.Where(v => v != null).Select(v => new
        {
            originalObj = v,
            v.Position,
            v.FieldCoordinates,
            Distance = Vector2.Distance(v.Position, position)
        }).OrderBy(v => v.Distance).First().originalObj;

        return nearestPoint;
    }

    public WallComponent GetNextPointInDirection(WallComponent wc, ref Vector2 direction)
    {
        return GetNextPointInDirection(wc.FieldCoordinates.x, wc.FieldCoordinates.y, ref direction);
    }

    public void CreateEmptyWall(Vector3 position)
    {
        GameObject instance = Instantiate(_emptyWallPrefab);
        WallComponent pointWc = GetNearestPoint(position);
        pointWc.SetValue(false);
        instance.transform.position = pointWc.Position;
        FieldMatrix.SetValueTo(pointWc.FieldCoordinates.x, pointWc.FieldCoordinates.y, pointWc);
    }

    public void CreateWall(Vector3 position)
    {
        GameObject instance = Instantiate(_wallPrefab);
        WallComponent pointWc = GetNearestPoint(position);
        pointWc.SetValue(true);
        instance.transform.position = pointWc.Position;
        FieldMatrix.SetValueTo(pointWc.FieldCoordinates.x, pointWc.FieldCoordinates.y, pointWc);

        //GameObject instance = Instantiate(Resources.Load("BlockR", typeof(GameObject))) as GameObject;
        //WallComponent pointWc = GetNearestPoint(position);
        //pointWc.SetValue(true);
        //instance.transform.position = pointWc.Position;
        //FieldMatrix.SetValueTo(pointWc.FieldCoordinates.x, pointWc.FieldCoordinates.y, pointWc);
    }

    private void Update()
    {
        var arr = CentipedeController.controllers;
        if (arr != null && arr.Count <= 0)
        {
            CreateNewCentipede();
        }
    }

    public WallComponent GetNextPointInDirection(int n, int m, ref Vector2 direction)
    {
        if (direction.Equals(Vector2.left))
        {
            if (n - 1 >= 0 && !FieldMatrix.GetValueFor(n-1, m).IsWall)
            {
                return FieldMatrix.GetValueFor(n - 1, m);
            }
            else
            {
                direction = -direction;
                if (m - 1 >= 0 && !FieldMatrix.GetValueFor(n, m - 1).IsWall)
                {
                    return FieldMatrix.GetValueFor(n, m - 1);
                }
                else if(m - 1 < 0)
                {
                    //_uiController.ShowEndGameWindow(UIController.EndType.Failure);
                    //CreateNewCentipede();
                    return FieldMatrix.GetValueFor(n, m);
                }
                else if(n + 1 < FieldMatrix.ColumnCount)
                {
                    return FieldMatrix.GetValueFor(n + 1, m);
                }
                else return FieldMatrix.GetValueFor(n, m);
            }
        }
        else
        {
            if (n + 1 < FieldMatrix.ColumnCount && !FieldMatrix.GetValueFor(n + 1, m).IsWall)
            {
                return FieldMatrix.GetValueFor(n + 1, m);
            }
            else
            {
                direction = -direction;
                if (m - 1 >= 0 && !FieldMatrix.GetValueFor(n, m - 1).IsWall)
                {
                    return FieldMatrix.GetValueFor(n, m - 1);
                }
                else if(m - 1 < 0)
                {
                    //_uiController.ShowEndGameWindow(UIController.EndType.Failure);
                    //CreateNewCentipede();
                    return FieldMatrix.GetValueFor(n, m);
                }
                else if(n - 1 > 0)
                {
                    return FieldMatrix.GetValueFor(n - 1, m);
                }
                else return FieldMatrix.GetValueFor(n, m);
            }
        }
    }
}
