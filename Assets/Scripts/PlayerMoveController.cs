using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType { Vertical, Horizontal, Both};
public enum DeviceType { KeyBoard, Touch};
public class PlayerMoveController : MonoBehaviour {

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Rigidbody2D _playerRigid;
    [SerializeField] private Camera _camera;
    public float speed = 50;
    public MoveType moveType;
    public DeviceType inputType;
    	
	void Update () {
        
        Vector3 playerPosIntoCamera = _camera.WorldToViewportPoint(_playerTransform.position);
        Vector3 movement = Vector3.zero;

        switch(moveType)
        {
            case MoveType.Both:
                movement += GetHorizontalMovement(playerPosIntoCamera);
                movement += GetVerticalMovement(playerPosIntoCamera);
                break;
            case MoveType.Vertical:
                movement += GetVerticalMovement(playerPosIntoCamera);
                break;
            case MoveType.Horizontal:
                movement += GetHorizontalMovement(playerPosIntoCamera);
                break;
        }
        _playerRigid.MovePosition(_playerTransform.position + movement);
    }

    private Vector3 GetVerticalMovement(Vector3 playerPosIntoCamera)
    {
        float axisVer = Input.GetAxis("Vertical");
        if (axisVer > 0 && playerPosIntoCamera.y <= 1)
        {
            return Vector3.up * speed * Time.deltaTime;
        }
        else if (axisVer < 0 && playerPosIntoCamera.y >= 0)
        {
            return -Vector3.up * speed * Time.deltaTime;
        }

        return Vector3.zero;
    }

    private Vector3 GetHorizontalMovement(Vector3 playerPosIntoCamera)
    {
        float axisHor;

        switch(inputType)
        {
            case DeviceType.KeyBoard:
                axisHor = Input.GetAxis("Horizontal");
                break;
            case DeviceType.Touch:
                axisHor = Mathf.Round(Input.acceleration.x * 100) / 100f;
                if (Mathf.Abs(axisHor) < 0.15f)
                    return Vector3.zero;
                break;
            default: axisHor = Input.GetAxis("Horizontal"); break;
        }

        if (axisHor > 0 && playerPosIntoCamera.x <= 1)
        {
            return Vector3.right * speed * Time.deltaTime;
        }
        else if (axisHor < 0 && playerPosIntoCamera.x >= 0)
        {
            return -Vector3.right * speed * Time.deltaTime;
        }
        return Vector3.zero;
    }
}
