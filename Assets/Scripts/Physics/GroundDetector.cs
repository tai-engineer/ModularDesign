using UnityEngine;

namespace Physics
{
    public class GroundDetector : MonoBehaviour
    {
        [SerializeField]
        internal Vector3 _relativePosition;

        [SerializeField]
        internal LayerMask _layer;

        [SerializeField]
        internal float _sphereRadius = 0.5f;

        public Vector3 Position
        {
            get => transform.position + _relativePosition;
            set => _relativePosition = value - transform.position;
        }

        public bool Check()
        {
            return UnityEngine.Physics.CheckSphere(Position, _sphereRadius, _layer);
        }
    }
}