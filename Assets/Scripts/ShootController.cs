using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// The controller that Instantiate bullets when screen
// is pressed
public class ShootController : MonoBehaviour {

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _bulletParent;
    [SerializeField] private float _chargingTime;
    private bool IsCharging = false;

	void Update () {
		if(Input.GetMouseButton(0) && !IsCharging)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if(EventSystem.current.currentSelectedGameObject == null)
                Shoot();
#elif UNITY_ANDROID || UNITY_IOS
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId) &&
            EventSystem.current.currentSelectedGameObject == null)
            {
                Shoot();
            }
#endif
        }
    }

    // Instantiating new bullet and starting charging process
    private void Shoot()
    {
        GameObject go = Instantiate(_bulletPrefab, _playerTransform.position, Quaternion.identity, _bulletParent);
        StartCoroutine(Charging());
    }

    // The charging method that wait some time and
    // allows us to shoot again
    private IEnumerator Charging()
    {
        IsCharging = true;
        yield return new WaitForSeconds(_chargingTime);
        IsCharging = false;
    }
}
