using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugComponent : MonoBehaviour {

    [Range(0, 1)]
    public float probability;
    public float timeBwCreating;
    private Transform _thisTransform;
    private Animator _thisAnimator;
    private BoxCollider2D _thisCollider;
    private Camera _camera;

    private void Awake()
    {
        _thisAnimator = GetComponent<Animator>();
        _thisTransform = transform;
        _thisCollider = gameObject.GetComponent<BoxCollider2D>();
        _camera = Camera.main;
        StartCoroutine(CreatingWalls());
    }

    private IEnumerator CreatingWalls()
    {
        float number;
        while(true)
        {
            Vector3 coord = _camera.WorldToViewportPoint(_thisTransform.position);
            if (coord.y < 0.2)
            {
                yield break;
            }

            yield return new WaitForSeconds(timeBwCreating);

            number = ((float)Random.Range(0, 100)) / 100f;
            if (number <= probability)
                FieldController.Instance.CreateWall(_thisTransform.position);
        }
    }

    public void OnDeath()
    {
        StopAllCoroutines();
        _thisAnimator.SetTrigger("Explode");
        _thisCollider.enabled = false;
        Destroy(this.gameObject, 5f);
    }
}
