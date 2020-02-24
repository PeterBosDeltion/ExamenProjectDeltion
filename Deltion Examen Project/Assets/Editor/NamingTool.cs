using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NamingTool : EditorWindow
{
    Object[] selections = new Object[0];

    string feedackText;

    string suffix;
    string prefix;
    string baseName;
    bool suffixEnabled;
    bool prefixEnabled;
    bool numberdEnabled;
    int baseValue;
    int stepValue;
    Vector2 scrollPos = new Vector2();

    [MenuItem("Tools/RenameTool")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(NamingTool));
    }
    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        baseName = EditorGUILayout.TextField("BaseName", baseName, GUILayout.Width(350));

        EditorGUILayout.Space();

        prefixEnabled = EditorGUILayout.BeginToggleGroup("Prefix", prefixEnabled);
            prefix = EditorGUILayout.TextField("Prefix", prefix, GUILayout.Width(350));
        EditorGUILayout.EndToggleGroup();

        suffixEnabled = EditorGUILayout.BeginToggleGroup("Suffix", suffixEnabled);
            suffix = EditorGUILayout.TextField("Suffix", suffix, GUILayout.Width(350));
        EditorGUILayout.EndToggleGroup();


        EditorGUILayout.Space();

        numberdEnabled = EditorGUILayout.BeginToggleGroup("Numberd", numberdEnabled);
            baseValue = EditorGUILayout.IntField("BaseNumber", baseValue, GUILayout.Width(350));
            stepValue = EditorGUILayout.IntField("Step", stepValue, GUILayout.Width(350));
        if(stepValue == 0)
        {
            stepValue = 1;
        }

        EditorGUILayout.EndToggleGroup();

        EditorGUILayout.Space();

        if(GUILayout.Button("Rename", GUILayout.Width(350)))
        {
            Rename();
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("",feedackText);

        EditorGUILayout.EndScrollView();
    }

    void OnSelectionChange()
    {
        selections = Selection.objects;
    }

    void Rename()
    {
        if(selections.Length != 0)
        {
            Undo.RecordObjects(selections,"selections");

            int objectIndex = 0;
            int currentBaseValue = baseValue;
            foreach (GameObject currentObject in selections)
            {
                string objectName = "";

                if(prefixEnabled)
                {
                    objectName += prefix;
                }

                if(baseName != "")
                {
                    Debug.Log(baseName);
                    objectName += baseName;
                }
                else
                {
                    Debug.Log("doe het");
                    objectName += currentObject.name;
                }

                if (suffixEnabled)
                {
                    objectName += suffix;
                }

                if(numberdEnabled)
                {
                    if(objectIndex == 0)
                    {
                        objectName += "_" + baseValue;
                    }
                    else
                    {
                        int newValue = baseValue + stepValue * objectIndex;
                        objectName += "_" + newValue;
                    }
                    objectIndex += 1;
                }

                currentObject.name = objectName;
            }
            feedackText = "Objects renamed";
        }
        else
        {
            feedackText = "No selected objects";
        }
    }
}
