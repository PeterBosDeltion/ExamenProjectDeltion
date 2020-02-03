using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Profiling.Memory.Experimental;


public class ChangeObjectTool : EditorWindow
{
    Object[] selectedObjects = new Object[0];

    List<GameObject> newObjects = new List<GameObject>();

    Object toChangeTo;
    string objectName;
    bool randomization;
    bool parent;
    bool scale;
    bool rot;
    bool pos;

    string MessageText;

    internal Vector3 minScale;
    internal Vector3 maxScale;

    internal Vector3 minRot;
    internal Vector3 maxRot;

    internal Vector3 minPos;
    internal Vector3 maxPos;

    bool scaleValues;
    bool rotationValues;
    bool positionValues;

    internal bool MultiplyRandomScale;
    internal bool MultiplyRandomRotation;
    internal bool MultiplyRandomPosition;


    Vector2 scrollPos = new Vector2();

    [MenuItem("Movares/ChangeObjectsTool")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ChangeObjectTool));
    }

    void OnSelectionChange()
    {
        selectedObjects = Selection.objects;
    }

    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);


        EditorGUILayout.LabelField("Only affects Active objects", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Text in the name of the object to change", EditorStyles.boldLabel);
        objectName = EditorGUILayout.TextField("Text within Name", objectName , GUILayout.Width(350));

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Objects to change to prefab", EditorStyles.boldLabel);
        toChangeTo = EditorGUILayout.ObjectField("Change to this:", toChangeTo, typeof(GameObject), true , GUILayout.Width(350));

        EditorGUILayout.Space();

        parent = EditorGUILayout.ToggleLeft("Parent to original parent", parent);
        randomization = EditorGUILayout.ToggleLeft("Randomize after change",randomization);

        EditorGUILayout.Space();

        if (GUILayout.Button("Change", GUILayout.Width(350)))
        {
            Change();
        }
        if (GUILayout.Button("Select changed objects" , GUILayout.Width(350)))
        {
            SelectObjects();
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Randomization Values", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        EditorGUI.indentLevel++;

        scale = EditorGUILayout.BeginToggleGroup("Scale", scale);
            EditorGUI.indentLevel++;

            MultiplyRandomScale = EditorGUILayout.ToggleLeft("Scale relative to curent scale", MultiplyRandomScale);

            scaleValues = EditorGUILayout.Foldout(scaleValues, "Values");
            if (scaleValues)
            {
            EditorGUI.indentLevel++;

                minScale = EditorGUILayout.Vector3Field("Scale Min",minScale);

                EditorGUILayout.Space();
                    
                maxScale = EditorGUILayout.Vector3Field("Scale Max", maxScale);

            EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        EditorGUILayout.EndToggleGroup();

        rot = EditorGUILayout.BeginToggleGroup("Rotation", rot);
            EditorGUI.indentLevel++;

            MultiplyRandomRotation = EditorGUILayout.ToggleLeft("Rotate relative to curent rotation", MultiplyRandomRotation);

            rotationValues = EditorGUILayout.Foldout(rotationValues, "Values");
            if(rotationValues)
            {
            EditorGUI.indentLevel++;

                minRot = EditorGUILayout.Vector3Field("Rotation Min", minRot);

                EditorGUILayout.Space();

                maxRot = EditorGUILayout.Vector3Field("Rotation Max", maxRot);

            EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        EditorGUILayout.EndToggleGroup();

        pos = EditorGUILayout.BeginToggleGroup("Position", pos);
            EditorGUI.indentLevel++;

            MultiplyRandomPosition = EditorGUILayout.ToggleLeft("Place relative to curent Position", MultiplyRandomPosition);

            positionValues = EditorGUILayout.Foldout(positionValues, "Values");
            if(positionValues)
            {
            EditorGUI.indentLevel++;

                minPos = EditorGUILayout.Vector3Field("Position Min", minPos);

                EditorGUILayout.Space();

                maxPos = EditorGUILayout.Vector3Field("Position Max", maxPos);

            EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        EditorGUILayout.EndToggleGroup();

        EditorGUI.indentLevel--;

        EditorGUILayout.Space();

        if (GUILayout.Button("Randomize selected objects" , GUILayout.Width(350)))
        {
            Randomize();
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("", MessageText);

        EditorGUILayout.EndScrollView();
    }

    internal void Change()
    {
        bool found = false;
        int amount = 0;
        GameObject placedParent = null;
        newObjects = new List<GameObject>();

        if (toChangeTo != null)
        {
            foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                if (go.name.Contains(objectName))
                {
                    found = true;
                    amount += 1;

                    //Instantiates new object on the place of the old one and deactivates the old object
                    Vector3 pos = go.transform.position;
                    Undo.RecordObject(go,"SetFalse");
                    go.SetActive(false);
                    GameObject newObject;
                    Undo.RegisterCreatedObjectUndo(newObject = PrefabUtility.InstantiatePrefab(toChangeTo) as GameObject, "InstantiatedObject");
                    newObject.transform.position = pos;
                    newObject.transform.rotation = go.transform.rotation;
                    newObjects.Add(newObject);

                    //Triggers if "Randomize after change" is true
                    if(randomization)
                    {
                        EditObjectValues(newObject);
                    }

                    //Creates parent the placed objects parent to
                    if (!parent)
                    {
                        if(placedParent == null)
                        {
                            placedParent = new GameObject();
                            placedParent.name = "placedObjects";
                            Undo.RegisterCreatedObjectUndo(placedParent, "placedParent");
                        }

                        newObject.transform.SetParent(placedParent.transform);
                    }
                    else
                    {
                        newObject.transform.SetParent(go.transform.parent);
                    }
                }
            }
            if (found != true)
            {
                MessageText = "No objects found whit " + objectName + " in their name";
            }
            else
            {
                MessageText = amount + " Objects placed";
            }
        }
        else
        {
            MessageText = "No object assigned to change to";
        }
    }

    internal void Randomize()
    {
        int amount = 0;
        foreach (GameObject obj in selectedObjects)
        {
            Undo.RecordObject(obj.transform, "ChangedObjects");
            EditObjectValues(obj);
            amount += 1;
        }
        if(amount != 0)
        {
            MessageText = amount + " Objects randomized";
        }
        else
        {
            MessageText = "No objects selected";
        }
    }

    void SelectObjects()
    {
        selectedObjects = newObjects.ToArray();
        Selection.objects = selectedObjects;
    }

    float RandomizeNumber(float valueMin,float valueMax)
    {
        float newValue = 0;
        newValue = Random.Range(valueMin, valueMax);
        return newValue;
    }

    /// <summary>
    /// Randomizes Object values
    /// </summary>
    /// <param name="obj"> Object to randomize </param>
    void EditObjectValues(GameObject obj)
    {
        if (scale)
        {
            Scale(obj);
        }

        if(rot)
        {
           Rotation(obj);
        }

        if(pos)
        {
            Position(obj);
        }
    }

    internal void Position(GameObject obj)
    {
        float x = RandomizeNumber(minPos.x, maxPos.x);
        float y = RandomizeNumber(minPos.y, maxPos.y);
        float z = RandomizeNumber(minPos.z, maxPos.z);

        Vector3 randomPosition = new Vector3(x, y, z);

        if (MultiplyRandomPosition)
        {
            Vector3 newPosition = randomPosition + obj.transform.localPosition;
            obj.transform.localPosition = newPosition;
        }
        else
        {
            obj.transform.localPosition = randomPosition;
        }
    }

    internal void Rotation(GameObject obj)
    {
        float x = RandomizeNumber(minRot.x, maxRot.x);
        float y = RandomizeNumber(minRot.y, maxRot.y);
        float z = RandomizeNumber(minRot.z, maxRot.z);

        Vector3 randomRotation = new Vector3(x, y, z);

        if (MultiplyRandomRotation)
        {
            Vector3 newRotation = randomRotation + obj.transform.eulerAngles;
            obj.transform.eulerAngles = newRotation;
        }
        else
        {
            obj.transform.localRotation = Quaternion.Euler(randomRotation);
        }
    }
    internal void Scale(GameObject obj)
    {
        float x = RandomizeNumber(minScale.x, maxScale.x);
        float y = RandomizeNumber(minScale.y, maxScale.y);
        float z = RandomizeNumber(minScale.z, maxScale.z);

        if (x == 0 || y == 0 || z == 0)
        {
            if (x == 0)
                x = 1;
            if (y == 0)
                y = 1;
            if (z == 0)
                z = 1;
        }

        Vector3 randomScale = new Vector3(x, y, z);

        if (MultiplyRandomScale)
        {
            Vector3 newScale = randomScale + obj.transform.localScale;
            obj.transform.localScale = newScale;
        }
        else
        {
            obj.transform.localScale = randomScale;
        }
    }
}
