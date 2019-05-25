using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The controller that controls the player movement
// in Vertical, Horizontal or both directions
// There is different mechanics for different build
// target platforms:
// 1. Use WASD or arrow keys to move player on you computer
// 2. Use accelerometer to control player on your Android or IPhine
public enum MoveType { Vertical, Horizontal, Both};
public class PlayerMoveController : MonoBehaviour {

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Rigidbody2D _playerRigid;
    [SerializeField] private Camera _camera;
    public float speed = 50;
    public MoveType moveType;
    	
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
#if UNITY_EDITOR || UNITY_STANDALONE
        axisHor = Input.GetAxis("Horizontal");
#elif UNITY_ANDROID || UNITY_IOS
        axisHor = Mathf.Round(Input.acceleration.x * 100) / 100f;
        if (Mathf.Abs(axisHor) < 0.08f)
            return Vector3.zero;
#endif

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
