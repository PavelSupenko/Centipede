using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletComponent : MonoBehaviour {

    [SerializeField] private int _damage;
    private Camera _camera;
    private BoxCollider2D _thisCollider;
    private Transform _thisTransform;
    private LinearMove _thisMove;
    private Animator _thisAnimator;

    private void Start()
    {
        _camera = Camera.main;
        _thisTransform = transform;
        _thisCollider = GetComponent<BoxCollider2D>();
        _thisMove = GetComponent<LinearMove>();
        _thisAnimator = GetComponent<Animator>();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        DamageableObject DamageObj = col.GetComponent<DamageableObject>();
        if (DamageObj != null)
        {
            DamageObj.Damage(_damage);
            OnDeath();
        }
    }

    private void Update()
    {
        if(_camera.WorldToViewportPoint(_thisTransform.position).y > 1.5)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDeath()
    {
        _thisCollider.enabled = false;
        _thisMove.speed = 0;
        _thisAnimator.SetTrigger("Explode");
        Destroy(this.gameObject, 0.5f);
    }
}
