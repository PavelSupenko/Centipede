using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletComponent : MonoBehaviour {

    [SerializeField] private int _damage;

    private void Start()
    {
        Destroy(this.gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Hurted");
        DamageableObject Dobj = col.GetComponent<DamageableObject>();
        if(Dobj != null)
        {
            Dobj.Damage(_damage);
        }
    }
}
