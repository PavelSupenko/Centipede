using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour {

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _bulletParent;
    
	void Update () {
		if(Input.GetMouseButton(0))
        {
            GameObject go = Instantiate(_bulletPrefab, _playerTransform.position, Quaternion.identity, _bulletParent);
        }
	}
}
