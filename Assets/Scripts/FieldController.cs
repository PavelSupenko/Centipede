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
    [SerializeField] private int _startSectionCount;
    [SerializeField] private float _positionUpdateTime;

    // Number of cells that will be above the screen
    // to start centipede abroad the screen
    private int extraCellsCount = 2;
    private int indexOfFirstRow;
    // Field dimensions
    private Vector2Int _cellCount;
    // Min dimension of field
    public int cellCount;
    // Probability of creating the wall
    [Range(0f,1f)] public float Probability;
    public Matrix<WallComponent> FieldMatrix;

    private void Awake()
    {
        Messenger<Vector3>.AddListener(EventStrings.CREATE_EMPTY_WALL, CreateEmptyWall);
        Messenger<Vector3>.AddListener(EventStrings.CREATE_WALL, CreateWall);
    }

    private void OnDestroy()
    {
        CentipedeController.controllers = null;
        Messenger<Vector3>.RemoveListener(EventStrings.CREATE_EMPTY_WALL, CreateEmptyWall);
        Messenger<Vector3>.RemoveListener(EventStrings.CREATE_WALL, CreateWall);
     }

    void Start ()
    {
        BuildField();
        CreateNewCentipede();
    }

    // Method to create new centipede
    // shorter than previous and faster
    public void CreateNewCentipede()
    {
        // Complete the game when number of
        // cells of next creating centipede < 0
        if (_startSectionCount > 0)
        {
            Messenger<int, float>.Broadcast(EventStrings.CREATE_NEW_CENTIPEDE, _startSectionCount, _positionUpdateTime);
            _startSectionCount -= 2;
            _positionUpdateTime *= 0.8f;
            return;
        }

        Messenger.Broadcast(EventStrings.GAME_COMPLETED);
        CentipedeController.controllers = null;
    }
    
    // Constructing game field
    private void BuildField()
    {
        float ratio;
        float tempCellSize;
        float randomNumber;
        float shiftX, shiftY;
        Vector3 position;
        if (Screen.width > Screen.height)
        {
            _cellCount.y = cellCount;
            ratio = ((float)Screen.width) / (float)Screen.height;
            _cellCount.x = (int)(cellCount * ratio);
            tempCellSize = (_camera.ViewportToWorldPoint
            (new Vector3(0, 1)) - _camera.ViewportToWorldPoint(new Vector3(0, 0))).y / _cellCount.y;
        }
        else
        {
            _cellCount.x = cellCount;
            ratio = ((float)Screen.height) / (float)Screen.width;
            _cellCount.y = (int)(cellCount * ratio);
            tempCellSize = (_camera.ViewportToWorldPoint
            (new Vector3(1, 0)) - _camera.ViewportToWorldPoint(new Vector3(0, 0))).x / _cellCount.x;
        }

        indexOfFirstRow = _cellCount.y / 10;
        FieldMatrix = new Matrix<WallComponent>(_cellCount.x, _cellCount.y + extraCellsCount);
        GlobalVariables.CELL_SIZE = new Vector3(tempCellSize, tempCellSize, tempCellSize);
        shiftX = 1f / _cellCount.x;
        shiftY = 1f / _cellCount.y;

        for (int i = 0; i < _cellCount.x; i++)
        {
            for (int j = 0; j < _cellCount.y - 1; j++)
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

                go.transform.localScale = GlobalVariables.CELL_SIZE * 0.5f;
                WallComponent wc = go.GetComponent<WallComponent>();
                wc.SetFieldValues(i, j, randomNumber <= Probability);

                FieldMatrix.SetValueTo(i, j, wc);
            }

            // Creating extra cells without walls
            for (int j = _cellCount.y - 1; j < _cellCount.y + extraCellsCount; j++)
            {
                position = _camera.ViewportToWorldPoint(new Vector3(i * shiftX + shiftX / 2, j * shiftY + shiftY / 2, 5));
                GameObject go = Instantiate(
                    _emptyWallPrefab,
                    position,
                    Quaternion.identity,
                    _wallsParent);

                go.transform.localScale = GlobalVariables.CELL_SIZE * 0.5f;
                WallComponent wc = go.GetComponent<WallComponent>();
                wc.SetFieldValues(i, j, false);

                FieldMatrix.SetValueTo(i, j, wc);
            }
        }
    }

    // Get nearest field point to this position
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

    // Overloading method of getting nearest point
    public WallComponent GetNextPointInDirection(WallComponent wc, ref Vector2 direction)
    {
        return GetNextPointInDirection(wc.FieldCoordinates.x, wc.FieldCoordinates.y, ref direction);
    }

    // Creating wall on the nearest point to this position
    public void CreateWall(Vector3 position)
    {
        WallComponent pointWc = GetNearestPoint(position);

        if (pointWc.FieldCoordinates.y < indexOfFirstRow || pointWc.IsWall)
            return;

        GameObject instance = Instantiate(_wallPrefab);
        pointWc.SetValue(true);
        instance.transform.position = pointWc.Position;
        instance.transform.localScale = GlobalVariables.CELL_SIZE * 0.5f;
        FieldMatrix.SetValueTo(pointWc.FieldCoordinates.x, pointWc.FieldCoordinates.y, pointWc);
    }

    // Creating empty wall on the nearest point to this position
    public void CreateEmptyWall(Vector3 position)
    {
        GameObject instance = Instantiate(_emptyWallPrefab);
        WallComponent pointWc = GetNearestPoint(position);
        pointWc.SetValue(false);
        instance.transform.position = pointWc.Position;
        instance.transform.localScale = GlobalVariables.CELL_SIZE * 0.5f;
        FieldMatrix.SetValueTo(pointWc.FieldCoordinates.x, pointWc.FieldCoordinates.y, pointWc);
    }

    // Creating new centipede when
    // array of heads become empty
    private void Update()
    {
        var arr = CentipedeController.controllers;
        if (arr != null && arr.Count <= 0)
        {
            CreateNewCentipede();
        }
    }

    // The main logics of centipede moving
    // 1. move in direction if there is no obstacle
    // 2. go down if there is an obstacle and changing direction
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
