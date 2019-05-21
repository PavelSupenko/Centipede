using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugController : MonoBehaviour {

    [SerializeField] private GameObject _bugPrefab;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _bugParent;
    public float pauseTime;
    
    private void OnEnable()
    {
        StartCoroutine(StartBug());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator StartBug()
    {
        Vector3 pos;
        while (true)
        {
            yield return new WaitForSeconds(pauseTime);
            pos = _camera.ViewportToWorldPoint(new Vector3(((float)Random.Range(0, 100)) / 100f, 1.2f));
            pos.z = 0;
            GameObject go = Instantiate(_bugPrefab, pos, Quaternion.identity, _bugParent);
        }
    }

}
