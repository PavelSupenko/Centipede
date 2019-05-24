using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugComponent : MonoBehaviour {
    
    private Transform _thisTransform;
    private Animator _thisAnimator;
    private BoxCollider2D _thisCollider;

    // Settings of probability to spawn rocks
    // with pause time
    [Range(0, 1)]
    public float probability;
    public float timeBwCreating;

    // Initializing variables and starting
    // coroutine of spawning rocks
    private void Awake()
    {
        _thisAnimator = GetComponent<Animator>();
        _thisTransform = transform;
        _thisCollider = gameObject.GetComponent<BoxCollider2D>();
        StartCoroutine(CreatingWalls());
    }

    // Generating random number and creating rock
    // based on it`s value and defined probability
    private IEnumerator CreatingWalls()
    {
        float number;
        while(true)
        {
            yield return new WaitForSeconds(timeBwCreating);

            number = ((float)Random.Range(0, 100)) / 100f;
            if (number <= probability)
                Messenger<Vector3>.Broadcast(EventStrings.CREATE_WALL, _thisTransform.position);
        }
    }

    // Some actions when object deadly damaged
    // This function is called from other component
    public void OnDeath()
    {
        StopAllCoroutines();
        _thisAnimator.SetTrigger("Explode");
        _thisCollider.enabled = false;
        Destroy(this.gameObject, 5f);
    }
}
