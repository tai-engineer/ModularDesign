using System;
using UnityEngine;

namespace CameraControl
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class OrbitCamera : MonoBehaviour
    {
        [SerializeField]
        Transform _focus;

        [SerializeField]
        float _distance;

        [SerializeField, Min(0)]
        float _focusRadius;

        [SerializeField, Range(0f, 1f)]
        [Tooltip("Make camera respond to small motion of the focus point")]
        float _focusCentering = 0.5f;
        
        Vector3 _focusPoint;
        Vector2 _orbitAngle = new Vector2(45f, 0f);

        void Awake()
        {
            _focusPoint = _focus.position;
        }

        void LateUpdate()
        {
            UpdateFocusPoint();
            Quaternion lookRotation = Quaternion.Euler(_orbitAngle);
            Vector3 lookDirection = transform.forward;
            Vector3 lookPosition = _focusPoint - lookDirection * _distance;
            transform.SetPositionAndRotation(lookPosition, lookRotation);
        }

        void UpdateFocusPoint()
        {
            if (_focusRadius <= 0)
                return;
            
            Vector3 targetPoint = _focus.position;
            float dist = Vector3.Distance(_focusPoint, targetPoint);
            float t = 1f;
            if (dist > 0.01f && _focusCentering > 0f)
            {
                // Using Time.DeltaTime makes camera motion depends on game's timescale
                // Camera would stop moving if the game is paused
                // Time.unscaledDeltaTime will be used to avoid this issue
                t = Mathf.Pow(1 - _focusCentering, Time.unscaledDeltaTime);
            }
            if (dist >= _focusRadius)
            {
                t = Mathf.Min(t, _focusRadius / dist);
            }
            _focusPoint = Vector3.Lerp(targetPoint, _focusPoint, t);
        }
    }
}