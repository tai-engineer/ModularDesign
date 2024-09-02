using System;
using UnityEngine;
using Cinemachine;
namespace Camera
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField]
        InputReaderSO _input;
        [SerializeField]
        CinemachineFreeLook _freeLookVCam;

        [SerializeField]
        Transform _characterTransform;

        bool _isRMBPressed;

        [SerializeField, Range(0.5f, 3f)]
        float _speedMultiplier = 1f;
        void Awake()
        {
            SetupCharacterVirtualCamera(_characterTransform);
        }

        void SetupCharacterVirtualCamera(Transform target)
        {
            _freeLookVCam.LookAt = target;
            _freeLookVCam.Follow = target;
            // Use when character suddenly moves to a different location (for example, through teleportation or loading new scene)
            // _freeLookVCam.OnTargetObjectWarped(target, target.position - 
            //     _freeLookVCam.transform.position - Vector3.forward);
        }
        void OnEnable()
        {
            _input.enableMouseControlCameraEvent += OnEnableMouseControlCamera;
            _input.disableMouseControlCameraEvent += 
                OnDisableMouseControlCamera;
            _input.cameraMoveEvent += OnCameraMove;
        }
        void OnDisable()
        {
            _input.enableMouseControlCameraEvent -= OnEnableMouseControlCamera;
            _input.disableMouseControlCameraEvent -= 
                OnDisableMouseControlCamera;
            _input.cameraMoveEvent -= OnCameraMove;
        }

        void OnCameraMove(Vector2 cameraMovement)
        {
            if (!_isRMBPressed)
                return;
            
            _freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * Time
                .deltaTime * _speedMultiplier;
            _freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * Time
                .deltaTime * _speedMultiplier;;
        }

        void OnDisableMouseControlCamera()
        {
            _isRMBPressed = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            // when mouse control is disabled, the input needs to be cleared
            // or the last frame's input will 'stick' until the action is invoked again
            // _freeLookVCam.m_XAxis.m_InputAxisValue = 0;
            // _freeLookVCam.m_YAxis.m_InputAxisValue = 0;
        }

        void OnEnableMouseControlCamera()
        {
            _isRMBPressed = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
    }
}