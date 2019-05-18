using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour {

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Rigidbody2D _playerRigid;
    [SerializeField] private Camera _camera;
    public float speed = 50;
    	
	void Update () {

        float axisHor = Input.GetAxis("Horizontal");
        float axisVer = Input.GetAxis("Vertical");
        Vector3 playerPosIntoCamera = _camera.WorldToViewportPoint(_playerTransform.position);
        Vector3 movement = Vector3.zero;

        if (axisHor > 0 && playerPosIntoCamera.x <= 1)
        {
            movement += Vector3.right * speed * Time.deltaTime;
        }
        else if (axisHor < 0 && playerPosIntoCamera.x >= 0)
        {
            movement -= Vector3.right * speed * Time.deltaTime;
        }

        if (axisVer > 0 && playerPosIntoCamera.y <= 1)
        {
            movement += Vector3.up * speed * Time.deltaTime;
        }
        else if (axisVer < 0 && playerPosIntoCamera.y >= 0)
        {
            movement -= Vector3.up * speed * Time.deltaTime;
        }
        
        _playerRigid.MovePosition(_playerTransform.position + movement);
    }
}
