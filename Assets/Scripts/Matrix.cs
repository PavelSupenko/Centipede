using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Supported class to store some objects into matrix structure
// We will be store the points of game field with it

public class Matrix<T> where T:MonoBehaviour {
    private T[,] _matrix;
    public int RowCount { get; private set; }
    public int ColumnCount { get; private set; }
    public T[,] Field { get { return _matrix; } }
    
    public Matrix(int n, int m)
    {
        _matrix = new T[n, m];
        ColumnCount = n;
        RowCount = m;
    }

    public void SetValueTo(int n, int m, T value)
    {
        _matrix[n, m] = value;
    }

    public T GetValueFor(int n, int m)
    {
        return _matrix[n, m];
    }

    public T Last()
    {
        return _matrix[ColumnCount - 1, RowCount - 1];
    }
}
