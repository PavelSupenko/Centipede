﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour {

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _bulletParent;
    [SerializeField] private float _chargingTime;
    private bool IsCharging = false;

	void Update () {
		if(Input.GetMouseButton(0) && !IsCharging)
        {
            GameObject go = Instantiate(_bulletPrefab, _playerTransform.position, Quaternion.identity, _bulletParent);
            StartCoroutine(Charging());
        }
	}

    private IEnumerator Charging()
    {
        IsCharging = true;
        yield return new WaitForSeconds(_chargingTime);
        IsCharging = false;
    }
}
