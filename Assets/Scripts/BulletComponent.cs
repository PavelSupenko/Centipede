using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletComponent : MonoBehaviour {

    [SerializeField] private int _damage;
    private BoxCollider2D _thisCollider;
    private LinearMove _thisMove;
    private Animator _thisAnimator;
    private bool _isAlreadyDamaged = false;

    // Initializing
    private void Start()
    {
        _thisCollider = GetComponent<BoxCollider2D>();
        _thisMove = GetComponent<LinearMove>();
        _thisAnimator = GetComponent<Animator>();
    }

    // Giving damage to object if it`s have
    // DamageableObject component
    private void OnTriggerEnter2D(Collider2D col)
    {
        DamageableObject DamageObj = col.GetComponent<DamageableObject>();
        if (DamageObj != null && !_isAlreadyDamaged)
        {
            _isAlreadyDamaged = true;
            DamageObj.Damage(_damage);
            OnDeath();
        }
    }

    // Starting Explode animation
    // and destroying object after it
    private void OnDeath()
    {
        _thisCollider.enabled = false;
        _thisMove.speed = 0;
        _thisAnimator.SetTrigger("Explode");
        Destroy(this.gameObject, 0.5f);
    }
}
