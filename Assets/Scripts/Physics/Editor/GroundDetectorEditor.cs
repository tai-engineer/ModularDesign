using System;
using UnityEngine;
using UnityEditor;

namespace Physics
{
    [CustomEditor(typeof(GroundDetector))]
    public class GroundDetectorEditor : Editor
    {
        SerializedProperty _sphereRadius;
        GroundDetector _groundDetector;
        void OnEnable()
        {
            _groundDetector = (GroundDetector)target;
            _sphereRadius = serializedObject.FindProperty("_sphereRadius");
        }

        void OnSceneGUI()
        {
            EditorGUI.BeginChangeCheck();
            Vector3 pos = Handles.PositionHandle(_groundDetector.Position, Quaternion.identity);
            
            if (EditorGUI.EndChangeCheck())
            {
                _groundDetector.Position = pos;
                serializedObject.ApplyModifiedProperties();
            }

            Handles.color = Color.red * new Color(1,1,1, 0.7f);
            Handles.SphereHandleCap(0, _groundDetector.Position, Quaternion.identity, _sphereRadius.floatValue * 2, EventType.Repaint);
        }
    }
}