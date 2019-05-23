using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CentipedeSection : MonoBehaviour {

    private Transform _thisTransform;
    public CentipedeController Head { get; set; }
    public int SectionIndex { get; set; }

    private void Start()
    {
        _thisTransform = transform;
    }

    public void OnDeath()
    {
        if (Head != null)
        {
            Head.Split(SectionIndex);
            Messenger<Vector3>.Broadcast(EventStrings.CREATE_WALL, _thisTransform.position);
        }
        Destroy(this.gameObject);
    }
}
