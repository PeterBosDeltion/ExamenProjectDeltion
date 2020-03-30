using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Pinwheel.Griffin
{
    [CustomEditor(typeof(GAssetExplorer))]
    public class GAssetExplorerInspector : Editor
    {
        private GAssetExplorer instance;

        private static string INSTRUCTION = string.Format("Below are some asset suggestions which we found helpful to enhance your scene.");

        public void OnEnable()
        {
            instance = target as GAssetExplorer;
        }

        public override void OnInspectorGUI()
        {
            DrawInstructionGUI();
            DrawCollectionsGUI();
            DrawCrossPromotionGUI();
        }

        public override bool RequiresConstantRepaint()
        {
            return false;
        }

        private void DrawInstructionGUI()
        {
            string label = "Instruction";
            string id = "instruction" + instance.GetInstanceID().ToString();
            GEditorCommon.Foldout(label, true, id, () =>
            {
                EditorGUILayout.LabelField(INSTRUCTION, GEditorCommon.WordWrapItalicLabel);
            });
        }

        private void DrawCollectionsGUI()
        {
            string label = "Collections";
            string id = "collections" + instance.GetInstanceID().ToString();
            GEditorCommon.Foldout(label, true, id, () =>
            {
                Rect r;
                r = EditorGUILayout.GetControlRect();
                if (GUI.Button(r, "Other Assets From Pinwheel"))
                {
                    GAnalytics.Record(GAnalytics.ASSET_EXPLORER_LINK_CLICK);
                    GAssetExplorer.ShowPinwheelAssets();
                }

                r = EditorGUILayout.GetControlRect();
                if (GUI.Button(r, "Stylized Vegetation"))
                {
                    GAnalytics.Record(GAnalytics.ASSET_EXPLORER_LINK_CLICK);
                    GAssetExplorer.ShowStylizedVegetationLink();
                }

                r = EditorGUILayout.GetControlRect();
                if (GUI.Button(r, "Stylized Rock & Props"))
                {
                    GAnalytics.Record(GAnalytics.ASSET_EXPLORER_LINK_CLICK);
                    GAssetExplorer.ShowStylizedRockPropsLink();
                }

                r = EditorGUILayout.GetControlRect();
                if (GUI.Button(r, "Stylized Water"))
                {
                    GAnalytics.Record(GAnalytics.ASSET_EXPLORER_LINK_CLICK);
                    GAssetExplorer.ShowStylizedWaterLink();
                }

                r = EditorGUILayout.GetControlRect();
                if (GUI.Button(r, "Stylized Sky & Ambient"))
                {
                    GAnalytics.Record(GAnalytics.ASSET_EXPLORER_LINK_CLICK);
                    GAssetExplorer.ShowStylizedSkyAmbientLink();
                }

                r = EditorGUILayout.GetControlRect();
                if (GUI.Button(r, "Stylized Character"))
                {
                    GAnalytics.Record(GAnalytics.ASSET_EXPLORER_LINK_CLICK);
                    GAssetExplorer.ShowStylizedCharacterLink();
                }
            });
        }

        private void DrawCrossPromotionGUI()
        {
            string label = "Cross Promotion";
            string id = "crosspromo" + instance.GetInstanceID().ToString();
            GEditorCommon.Foldout(label, true, id, () =>
            {
                string text = "Are you a Publisher, send us a message to get more expose to the community!";
                EditorGUILayout.LabelField(text, GEditorCommon.WordWrapItalicLabel);
                Rect r = EditorGUILayout.GetControlRect();
                if (GUI.Button(r, "Send an Email"))
                {
                    GEditorCommon.OpenEmailEditor(
                        GCommon.BUSINESS_EMAIL,
                        "[Polaris V2] CROSS PROMOTION",
                        "DETAIL ABOUT YOUR ASSET HERE");
                }
            });
        }
    }
}
