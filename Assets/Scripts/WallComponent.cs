using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallComponent : MonoBehaviour {

    private Animator _thisAnimator;
    private BoxCollider2D _thisCollider;
    private Transform _thisTransform;

	void Start () {
        _thisTransform = transform;
        _thisTransform.rotation *= Quaternion.Euler(0,0,90 * Random.Range(0,4));

        _thisAnimator = GetComponent<Animator>();
        //_thisCollider = gameObject.AddComponent<BoxCollider2D>();
        _thisCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    public void OnDeath()
    {
        Destroy(_thisCollider);
        transform.rotation *= Quaternion.Euler(0, 0, Random.Range(0, 360));
        _thisAnimator.SetTrigger("Explode");
    }
}
