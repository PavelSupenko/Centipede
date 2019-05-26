using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableObject : MonoBehaviour {

    // Points that will be given to player if this object
    // will be hit
    [SerializeField] private int _pointForDeath;
    [SerializeField] private int _health;
    [SerializeField] private float _deathTime;

    // Some variables to avoid situation when we are trying
    // to kill already killed object
    private Coroutine _deathRoutine;
    private bool _isDead = false;
    
    // If health count < 0 -- kill this object
    public void Damage(int damage)
    {
        if (_deathRoutine != null)
            return;

        _health -= damage;
        if (_health <= 0)
            _deathRoutine = StartCoroutine(Death());
    }

    // Sending message "OnDeath" to this object`s components
    private IEnumerator Death()
    {
        if (!_isDead)
        {
            _isDead = true;
            gameObject.SendMessage("OnDeath");
            Messenger<int>.Broadcast(EventStrings.UP_POINTS,
                _pointForDeath + (GlobalVariables.DIFFICULTY + 5) * 3);
            yield return new WaitForSeconds(_deathTime);
        }
    }
}
