using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Pinwheel.Griffin
{
    public class GProductAdsWindow : EditorWindow
    {
        private static GUIStyle titleStyle;
        private static GUIStyle TitleStyle
        {
            get
            {
                if (titleStyle == null)
                {
                    titleStyle = new GUIStyle(EditorStyles.label);
                    titleStyle.fontSize = 20;
                    titleStyle.alignment = TextAnchor.MiddleCenter;
                    titleStyle.fontStyle = FontStyle.Bold;
                    titleStyle.normal.textColor = Color.white;
                }
                return titleStyle;
            }
        }

        private static GUIStyle subTitleStyle;
        private static GUIStyle SubTitleStyle
        {
            get
            {
                if (subTitleStyle == null)
                {
                    subTitleStyle = new GUIStyle(EditorStyles.label);
                    subTitleStyle.fontSize = 13;
                    subTitleStyle.alignment = TextAnchor.MiddleCenter;
                    subTitleStyle.fontStyle = FontStyle.Bold;
                    subTitleStyle.normal.textColor = Color.white;
                }
                return subTitleStyle;
            }
        }

        private static GUIStyle buttonStyle;
        public static GUIStyle ButtonStyle
        {
            get
            {
                if (buttonStyle == null)
                {
                    buttonStyle = new GUIStyle();
                    buttonStyle.alignment = TextAnchor.MiddleCenter;
                    buttonStyle.normal.background = Texture2D.whiteTexture;
                    buttonStyle.fontSize = 13;
                    buttonStyle.fontStyle = FontStyle.Bold;
                }
                return buttonStyle;
            }
        }

        private const string PREF_KEY = "poseidon-ad-in-p2-editor";
        private const string DONT_SHOW_PREF_KEY = "poseidon-ad-in-p2-editor-dont-show";
        private const string WINDOW_TITLE = "New Product";
        private const string TITLE = "Poseidon - Low Poly Water System - Builtin & LWRP";
        private const string SUB_TITLE = "";
        private const string BG_IMAGE_NAME = "PoseidonAdsImage";
        private const string ICON_IMAGE_NAME = "PoseidonIcon";
        private const string STORE_LINK = "http://bit.ly/Poseidon-P2";

        [DidReloadScripts]
        private static void OnScriptReloaded()
        {
            if (CanShow())
            {
                ShowWindow();
            }
        }

        public static bool CanShow()
        {
            string shownTodayPref = string.Format("{0}-{1}", PREF_KEY, System.DateTime.Now.Date.ToString());
            int shownTodayCount = PlayerPrefs.GetInt(shownTodayPref, 0);
            int dontShow = PlayerPrefs.GetInt(DONT_SHOW_PREF_KEY, 0);
            return shownTodayCount == 0 && dontShow == 0;
        }

        public static void IncreaseTodayShowCount()
        {
            string shownTodayPref = string.Format("{0}-{1}", PREF_KEY, System.DateTime.Now.Date.ToString());
            int shownTodayCount = PlayerPrefs.GetInt(shownTodayPref, 0);
            shownTodayCount += 1;
            PlayerPrefs.SetInt(shownTodayPref, shownTodayCount);
        }

        public static void MarkDontShowAgain()
        {
            PlayerPrefs.SetInt(DONT_SHOW_PREF_KEY, 1);
        }

        public static void ShowWindow()
        {
            IncreaseTodayShowCount();
            GProductAdsWindow window = ScriptableObject.CreateInstance<GProductAdsWindow>();
            window.titleContent = new GUIContent(WINDOW_TITLE);
            window.minSize = new Vector2(640, 360);
            window.maxSize = new Vector2(640, 361);
            window.ShowUtility();
        }

        private void OnEnable()
        {
            wantsMouseMove = true;
        }

        public void OnGUI()
        {
            Rect backgroundRect = GUILayoutUtility.GetAspectRect(16f / 9f);
            Texture2D backgroundTex = Resources.Load<Texture2D>(BG_IMAGE_NAME);
            if (backgroundTex != null)
            {
                GUI.DrawTexture(backgroundRect, backgroundTex);
            }
            else
            {
                EditorGUI.DrawRect(backgroundRect, Color.black);
                EditorGUI.LabelField(backgroundRect, "Failed to load image.", EditorStyles.whiteLabel);
            }

            Rect iconRect = new Rect();
            iconRect.size = new Vector2(128, 128);
            iconRect.center = backgroundRect.center - new Vector2(0, 64);
            Texture2D iconTex = Resources.Load<Texture2D>(ICON_IMAGE_NAME);
            if (iconTex != null)
            {
                Vector2 shadowOffset = new Vector2(1, 1);
                Rect shadowRect = new Rect(iconRect.position + shadowOffset, iconRect.size);
                GUI.DrawTexture(shadowRect, iconTex, ScaleMode.ScaleToFit, true, 1, Color.black, 0, 0);
                GUI.DrawTexture(iconRect, iconTex, ScaleMode.ScaleToFit, true, 1, Color.white, 0, 0);
            }

            Rect titleRect = new Rect();
            titleRect.size = new Vector2(640, 30);
            titleRect.center = backgroundRect.center + new Vector2(0, 30);
            EditorGUI.DropShadowLabel(titleRect, TITLE, TitleStyle);

            Rect subtitleRect = new Rect();
            subtitleRect.size = new Vector2(640, 20);
            subtitleRect.center = backgroundRect.center + new Vector2(0, 55);
            EditorGUI.DropShadowLabel(subtitleRect, SUB_TITLE, SubTitleStyle);

            Rect buttonsRect = new Rect();
            buttonsRect.size = new Vector2(640, 30);
            buttonsRect.center = backgroundRect.center + new Vector2(0, 90);

            Rect learnMoreRect = new Rect();
            learnMoreRect.size = new Vector2(140, 30);
            learnMoreRect.center = buttonsRect.center - new Vector2(0, 0);
            EditorGUIUtility.AddCursorRect(learnMoreRect, MouseCursor.Link);
            if (GUI.Button(learnMoreRect, "Learn more", ButtonStyle))
            {
                Application.OpenURL(STORE_LINK);
            }

            Rect dontShowAgainRect = new Rect();
            dontShowAgainRect.size = new Vector2(140, 20);
            dontShowAgainRect.center = backgroundRect.center + new Vector2(0, 125);
            EditorGUIUtility.AddCursorRect(dontShowAgainRect, MouseCursor.Link);
            if (GUI.Button(dontShowAgainRect, "Don't show again", GEditorCommon.CenteredWhiteLabel))
            {
                MarkDontShowAgain();
                Close();
            }

            //Rect getFreeRect = new Rect();
            //getFreeRect.size = new Vector2(140, 30);
            //getFreeRect.center = buttonsRect.center + new Vector2(75, 0);
            //EditorGUIUtility.AddCursorRect(getFreeRect, MouseCursor.Link);
            //if (GUI.Button(getFreeRect, "Get it FREE", ButtonStyle))
            //{
            //    Application.OpenURL("http://pinwheel.studio/polaris2-evaluation");
            //}

            Rect desLinkRect = new Rect();
            desLinkRect.size = new Vector2(640, 15);
            desLinkRect.position = new Vector2(backgroundRect.min.x, backgroundRect.max.y - 15);

            if (learnMoreRect.Contains(Event.current.mousePosition))
            {
                EditorGUI.DrawRect(desLinkRect, Color.white * 0.5f);
                EditorGUI.LabelField(desLinkRect, "https://assetstore.unity.com", EditorStyles.miniLabel);
            }

            //if (getFreeRect.Contains(Event.current.mousePosition))
            //{
            //    EditorGUI.DrawRect(desLinkRect, Color.white * 0.5f);
            //    EditorGUI.LabelField(desLinkRect, "https://pinwheel.studio", EditorStyles.miniLabel);
            //}

            if (Event.current != null && Event.current.isMouse)
            {
                Repaint();
            }
        }
    }
}
