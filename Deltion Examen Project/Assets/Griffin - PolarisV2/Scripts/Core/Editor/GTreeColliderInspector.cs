using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Pinwheel.Griffin
{
    [CustomEditor(typeof(GTreeCollider))]
    public class GTreeColliderInspector : Editor
    {
        private GTreeCollider instance;

        private void OnEnable()
        {
            instance = target as GTreeCollider;
        }

        public override void OnInspectorGUI()
        {
            instance.Terrain = EditorGUILayout.ObjectField("Terrain", instance.Terrain, typeof(GStylizedTerrain), true) as GStylizedTerrain;
            instance.Target = EditorGUILayout.ObjectField("Target", instance.Target, typeof(GameObject), true) as GameObject;
            instance.ColliderBudget = EditorGUILayout.IntField("Budget", instance.ColliderBudget);
            instance.Distance = EditorGUILayout.FloatField("Distance", instance.Distance);
        }
    }
}
