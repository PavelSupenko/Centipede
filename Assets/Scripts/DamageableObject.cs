using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableObject : MonoBehaviour {

    [SerializeField] private int _health;
    private Coroutine _deathRoutine;
    
    public void Damage(int damage)
    {
        if (_deathRoutine != null)
            return;

        _health -= damage;
        if (_health <= 0)
            _deathRoutine = StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        Destroy(this.gameObject);
        yield break;
    }
}
