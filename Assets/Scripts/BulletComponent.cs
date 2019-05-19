using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletComponent : MonoBehaviour {

    [SerializeField] private int _damage;
    private Camera _camera;
    private Transform _thisTransform;

    private void Start()
    {
        _camera = Camera.main;
        _thisTransform = transform;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        DamageableObject DamageObj = col.GetComponent<DamageableObject>();
        if(DamageObj != null)
        {
            DamageObj.Damage(_damage);
            Destroy(this.gameObject); 
        }
    }

    private void Update()
    {
        if(_camera.WorldToViewportPoint(_thisTransform.position).y > 1.5)
        {
            Destroy(this.gameObject);
        }
    }
}
