using System;
using Character;
using UnityEngine;
using UnityEngine.InputSystem;
namespace CharacterInput
{
    [RequireComponent(typeof(CharacterManager))]
    public class ClickAndMove : MonoBehaviour
    {
        [SerializeField]
        Camera _mainCamera;

        [SerializeField]
        LayerMask _hitLayer = -1;
        CharacterManager _character;

        void Awake()
        {
            _character = GetComponent<CharacterManager>();
        }

        void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out RaycastHit hitInfo, _hitLayer))
                {
                    Vector3 targetPos = hitInfo.point;
                    _character.MoveToPosition(targetPos);
                }
            }
        }
    }
}