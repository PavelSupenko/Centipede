using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CentipedeSection : MonoBehaviour {

    private Transform _thisTransform;

    // The head of this part of centipede
    public CentipedeController Head { get; set; }
    // Index of this section into centipede body relative the head
    public int SectionIndex { get; set; }

    private void Start()
    {
        _thisTransform = transform;
    }

    // Splitting the centipede in two relative index
    // of destroyed cell and
    // Creating the rock on it`s position
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
