using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : IEnumerable<Dot> {
    
    public Dot Next { get; set; }
    public int CentipedeNumber { get; set; }

    public static Dot First, Last;
    
    public Dot()
    {
        if (First == null)
        {
            First = this;
            Last = First;
        }
        else
        {
            Last.Next = this;
            Last = this;
        }
    }

    public Dot(Vector2 pos, Vector2 d, int id) : this()
    {
        position = pos;
        direction = d;
        CentipedeNumber = id;
    }

    public Vector2 position;
    public Vector2 direction;

    public IEnumerator<Dot> GetEnumerator()
    {
        return new DotEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class DotEnumerator : IEnumerator<Dot>
{
    private Dot _current;
    public Dot Current
    {
        get
        {
            return _current;
        }
    }

    object IEnumerator.Current
    {
        get
        {
            return _current;
        }
    }

    public void Dispose()
    {
        
    }

    public bool MoveNext()
    {
        if (_current == null)
        {
            _current = Dot.First;
        }
        else
        {
            _current = _current.Next;
        }

        return _current != null;
    }

    public void Reset()
    {
        _current = null;
    }
}
