using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableObject : MonoBehaviour {

    [SerializeField] private int _pointForDeath;
    [SerializeField] private int _health;
    [SerializeField] private float _deathTime;
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
        gameObject.SendMessage("OnDeath");
        Messenger<int>.Broadcast(EventStrings.UP_POINTS, _pointForDeath);
        yield return new WaitForSeconds(_deathTime);
    }
}
