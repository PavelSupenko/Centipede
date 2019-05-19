using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FieldSettings
{
    public static float CellSize;
}

public class WallController : MonoBehaviour {

    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private Transform _wallsParent;
    
    public Vector2 CellCount;
    [Range(0f,1f)]
    public float Probability;

	void Awake () {
        Vector3 position;
        FieldSettings.CellSize = (_camera.ViewportToWorldPoint(new Vector3(1, 0)) - _camera.ViewportToWorldPoint(new Vector3(0, 0))).x / CellCount.x;
        float cellSize = FieldSettings.CellSize * 0.4f;

        float shiftX = 1f / CellCount.x;
        float shiftY = 1f / CellCount.y;
        float randomNumber;

        for (int i = 0; i < CellCount.x; i++)
        {
            for (int j = 2; j < CellCount.y; j++)
            {
                randomNumber = Random.value;
                if(randomNumber <= Probability)
                {
                    position = _camera.ViewportToWorldPoint(new Vector3(i * shiftX + shiftX/2, j * shiftY + shiftY/2, 5));
                    GameObject go = Instantiate(_wallPrefab, position, Quaternion.identity, _wallsParent);
                    go.transform.localScale = new Vector3(cellSize, cellSize);
                }
            }
        }
	}
}
