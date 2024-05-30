using System;
using UnityEngine;

namespace CameraControl
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class OrbitCamera : MonoBehaviour
    {
        [SerializeField]
        InputReaderSO _cameraInput;
        
        [SerializeField]
        Transform _focus;

        [SerializeField]
        float _distance;

        [SerializeField, Min(0)]
        float _focusRadius;

        [SerializeField, Range(0f, 1f)]
        [Tooltip("Make camera respond to small motion of the focus point")]
        float _focusCentering = 0.5f;

        Vector3 _focusPoint, _lastFocusPoint;
        Vector2 _orbitAngle = new Vector2(45f, 0f);

        [SerializeField, Range(1f, 360f)]
        float _rotationSpeed = 90f;

        [SerializeField, Range(-89f, 89f)]
        float _minVerticalAngle = -30f, _maxVerticalAngle = 60f;

        [SerializeField, Min(0f), Tooltip("Automatic Rotation Time")]
        float _alignDelay = 5f;

        float _lastManualRotationTime = 0f;

        Camera _regularCamera;
        [SerializeField]
        float _rotationSmoothRange = 45f;

        [SerializeField]
        LayerMask _obstructionMask = -1;
        Vector3 CameraHalfExtends
        {
            get
            {
                Vector3 halfExtends;
                halfExtends.y = _regularCamera.nearClipPlane *
                                Mathf.Tan(0.5f * Mathf.Deg2Rad * _regularCamera.fieldOfView);
                halfExtends.x = halfExtends.y * _regularCamera.aspect;
                halfExtends.z = 0f;
                return halfExtends;
            }
        }
        void OnValidate()
        {
            if (_maxVerticalAngle < _minVerticalAngle)
                _maxVerticalAngle = _minVerticalAngle;
        }

        void Awake()
        {
            _regularCamera = GetComponent<Camera>();
            _focusPoint = _focus.position;
            transform.localRotation = Quaternion.Euler(_orbitAngle);
        }

        void LateUpdate()
        {
            UpdateFocusPoint();
           
            Quaternion lookRotation;
            if (ManualRotation() || AutomaticRotation())
            {
                ContrainAngles();
                lookRotation = Quaternion.Euler(_orbitAngle);
            }
            else
            {
                lookRotation = transform.localRotation;
            }
            Vector3 lookDirection = transform.forward;
            Vector3 lookPosition = _focusPoint - lookDirection * _distance;

            if (Physics.BoxCast(_focusPoint, CameraHalfExtends,-lookDirection, out RaycastHit hit, lookRotation, 
                    _distance - _regularCamera.nearClipPlane, _obstructionMask))
            {
                lookPosition = _focusPoint - lookDirection * (hit.distance + _regularCamera.nearClipPlane);
            }
            transform.SetPositionAndRotation(lookPosition, lookRotation);
        }

        void OnDrawGizmosSelected()
        {
            _regularCamera = GetComponent<Camera>();
            Vector3 halfExtends;
            halfExtends.y = _regularCamera.nearClipPlane *
                            Mathf.Tan(0.5f * Mathf.Deg2Rad * _regularCamera.fieldOfView);
            halfExtends.x = halfExtends.y * _regularCamera.aspect;
            halfExtends.z = 0f;
            
            Vector3 backBottomLeft, backBottomRight, backTopLeft, backTopRight;
            Vector3 frontBottomLeft, frontBottomRight, frontTopLeft, frontTopRight;
            Gizmos.color = Color.red;

            Vector3 cameraPos = _regularCamera.transform.position;
            Vector3 dir = _regularCamera.transform.forward;
            Vector3 center = cameraPos + dir * _regularCamera.nearClipPlane;
            
            Quaternion cameraRot = _regularCamera.transform.rotation;
            halfExtends = cameraRot * halfExtends;
            Gizmos.DrawLine(cameraPos,center);
            
            backBottomLeft = center - halfExtends;
            backBottomRight = backBottomLeft + Vector3.right * halfExtends.x * 2f;
            backTopRight = center + halfExtends;
            backTopLeft = backTopRight - Vector3.right * halfExtends.x * 2f;

            Vector3 focusPos = _focus.transform.position;
            frontBottomLeft = new Vector3(focusPos.x - halfExtends.x, focusPos.y - halfExtends.y, focusPos.z);
            frontBottomRight = frontBottomLeft + Vector3.right * halfExtends.x * 2f;
            frontTopLeft = frontBottomLeft + Vector3.up * halfExtends.y * 2f;
            frontTopRight = frontTopLeft + Vector3.right * halfExtends.x * 2f;
            Gizmos.DrawLine(backBottomLeft, frontBottomLeft);
            Gizmos.DrawLine(backBottomRight, frontBottomRight);
            Gizmos.DrawLine(backTopLeft, frontTopLeft);
            Gizmos.DrawLine(backTopRight, frontTopRight);
            
            Gizmos.DrawLine(backBottomLeft, backTopLeft);
            Gizmos.DrawLine(backBottomRight, backTopRight);
            Gizmos.DrawLine(backBottomLeft, backBottomRight);
            Gizmos.DrawLine(backTopLeft, backTopRight);
            
            Gizmos.DrawLine(frontBottomLeft, frontTopLeft);
            Gizmos.DrawLine(frontBottomRight, frontTopRight);
            Gizmos.DrawLine(frontBottomLeft, frontBottomRight);
            Gizmos.DrawLine(frontTopLeft, frontTopRight);
        }

        void UpdateFocusPoint()
        {
            _lastFocusPoint = _focusPoint;
            Vector3 targetPoint = _focus.position;
            if (_focusRadius <= 0)
            {
                _focusPoint = targetPoint;
                return;
            }
            
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

        bool ManualRotation()
        {
            Vector2 cameraInput = new Vector2(_cameraInput.CameraInput.y, -_cameraInput.CameraInput.x);
            float e = 0.001f;
            if (cameraInput.x < -e || cameraInput.x > e || cameraInput.y < -e || cameraInput.y > e)
            {
                _orbitAngle += _rotationSpeed * Time.unscaledDeltaTime * cameraInput;
                _lastManualRotationTime = Time.unscaledTime;
                return true;
            }

            return false;
        }

        void ContrainAngles()
        {
            _orbitAngle.x = Mathf.Clamp(_orbitAngle.x, _minVerticalAngle, _maxVerticalAngle);

            if (_orbitAngle.y > 360f)
                _orbitAngle.y -= 360f;
            else if (_orbitAngle.y < 0f)
                _orbitAngle.y += 360f;
        }

        bool AutomaticRotation()
        {
            if ((Time.unscaledTime - _lastManualRotationTime) < _alignDelay)
                return false;

            Vector2 movement = new Vector2(_focusPoint.x - _lastFocusPoint.x, _focusPoint.z - _lastFocusPoint.z);
            float movementDeltaSqr = movement.sqrMagnitude;
            if (movementDeltaSqr < 0.0001f)
            {
                return false;
            }

            float headingAngle = GetAngle(movement / (Mathf.Sqrt(movementDeltaSqr)));
            float rotationChange = _rotationSpeed * Mathf.Min(Time.unscaledDeltaTime, movementDeltaSqr);
            float rotationDelta = Mathf.Abs(Mathf.DeltaAngle(headingAngle, _orbitAngle.y));
            if (rotationDelta < _rotationSmoothRange)
            {
                rotationChange *= rotationDelta / _rotationSmoothRange;
            }
            else if (180f - rotationDelta < _rotationSmoothRange)
            {
                // Prevent camera from rotating at full speed when focus moves toward camera
                rotationChange *= (180f - rotationDelta) / _rotationSmoothRange;
                Debug.Log("RotationDelta: " + rotationDelta);
            }
            _orbitAngle.y = Mathf.MoveTowardsAngle(_orbitAngle.y, headingAngle, rotationChange);
            return true;
        }
        
        static float GetAngle(Vector2 direction)
        {
            float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
            return direction.x < 0f ? 360f - angle : angle;
        }
    }
}