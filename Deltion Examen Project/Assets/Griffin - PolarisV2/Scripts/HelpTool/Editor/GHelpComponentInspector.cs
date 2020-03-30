using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Pinwheel.Griffin.HelpTool
{
    [CustomEditor(typeof(GHelpComponent))]
    public class GHelpComponentInspector : Editor
    {
        private void OnEnable()
        {
            if (GHelpEditor.Instance == null)
            {
                GHelpEditor.ShowWindow();
            }
        }

        public override void OnInspectorGUI()
        {
            if (GHelpEditor.Instance == null)
            {
                if (GUILayout.Button("Show Help Window"))
                {
                    GHelpEditor.ShowWindow();
                }
            }
        }
    }
}
