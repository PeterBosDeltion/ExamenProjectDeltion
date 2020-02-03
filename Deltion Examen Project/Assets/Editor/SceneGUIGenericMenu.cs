using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
#if UNITY_EDITOR
[InitializeOnLoad]
public class SceneGUIGenericMenu : Editor
{
    private static Transform[] SelectedObjects = new Transform[999];
    private static Vector3 currentMouseWorldpos;

    static SceneGUIGenericMenu()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }
    #region GUI Function
    static void OnSceneGUI(SceneView sceneview)
    {


        if (Event.current.button == 1 && Event.current.clickCount >= 2)
        {
            GenericMenu menu = new GenericMenu();

            if (Event.current.type == EventType.MouseDown)
            {
                Array.Clear(SelectedObjects, 0, SelectedObjects.Length);
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit))
                {
                    currentMouseWorldpos = hit.point;

                    SelectedObjects[0] = hit.transform;
                    //SelectObject(hit.transform.gameObject);
                    if (Selection.objects.Length > 1)
                    {
                        for (int i = 0; i < Selection.objects.Length; i++)
                        {
                            SelectedObjects[i] = (Transform)Selection.objects[i];
                        }
                    }
                    //Select/Hierarchy
                    menu.AddItem(new GUIContent(hit.transform.name), false, null, 999);
                    if (SelectedObjects[0].transform.parent)
                    {
                        menu.AddItem(new GUIContent("Select parent"), false, SelectParent, 21);
                    }
                    if (Selection.objects.Length == 1 && Selection.activeTransform != SelectedObjects[0].transform)
                    {
                        menu.AddItem(new GUIContent("Child to selected"), false, ParentToSelected, 22);
                    }

                    //Resets
                    DrawResets(menu);

                    //Prefab
                    if (PrefabUtility.GetCorrespondingObjectFromSource(SelectedObjects[0]) && Selection.objects.Length <= 1)
                    {
                        DrawPrefabRelated(menu);
                    }

                    //Components

                    //Colliders
                    DrawColliderRelated(menu);



                    //Audio
                    DrawAudioRelated(menu);


                }

                //Create objects
                DrawPrimitiveRelated(menu);
                DrawEffectsRelated(menu);

                //Lighting
                DrawLightingRelated(menu);

                //Misc.
                DrawMisc(menu);

                menu.ShowAsContext();
            }
        }
    }
    #endregion
    #region DrawGUI
    static void DrawResets(GenericMenu menu)
    {
        menu.AddItem(new GUIContent("GameObject/Reset position"), false, ResetPos, 1);
        menu.AddItem(new GUIContent("GameObject/Reset rotation"), false, ResetRotation, 7);
        menu.AddItem(new GUIContent("GameObject/Reset scale"), false, ResetScale, 8);
        menu.AddItem(new GUIContent("GameObject/Toggle active"), false, ToggleActive, 20);
    }

    static void DrawPrefabRelated(GenericMenu menu)
    {
        menu.AddItem(new GUIContent("Prefab/Apply to prefab"), false, ApplyToPrefab, 25);
        menu.AddItem(new GUIContent("Prefab/Revert to prefab"), false, RevertToPrefab, 26);
    }

    static void DrawColliderRelated(GenericMenu menu)
    {
        menu.AddItem(new GUIContent("Add component/Add RigidBody"), false, AddRigidbody, 2);
        menu.AddItem(new GUIContent("Add component/Colliders/Add Box collider"), false, AddBoxCollider, 3);
        menu.AddItem(new GUIContent("Add component/Colliders/Add Sphere collider"), false, AddSphereCollider, 4);
        menu.AddItem(new GUIContent("Add component/Colliders/Add Capsule collider"), false, AddCapsuleCollider, 5);
        menu.AddItem(new GUIContent("Add component/Colliders/Add Mesh collider"), false, AddMeshCollider, 6);
    }

    static void DrawAudioRelated(GenericMenu menu)
    {
        menu.AddItem(new GUIContent("Add component/Audio/Add Audio source"), false, AddAudioSource, 9);
    }

    static void DrawPrimitiveRelated(GenericMenu menu)
    {
        menu.AddItem(new GUIContent("Create/Empty"), false, CreateEmptyGameObject, 25);
        menu.AddItem(new GUIContent("Create/3D Object/Cube"), false, CreateCube, 10);
        menu.AddItem(new GUIContent("Create/3D Object/Sphere"), false, CreateSphere, 11);
        menu.AddItem(new GUIContent("Create/3D Object/Capsule"), false, CreateCapsule, 12);
        menu.AddItem(new GUIContent("Create/3D Object/Plane"), false, CreatePlane, 13);
        menu.AddItem(new GUIContent("Create/3D Object/Cylinder"), false, CreateCylinder, 14);
        menu.AddItem(new GUIContent("Create/3D Object/Quad"), false, CreateQuad, 15);
    }

    static void DrawLightingRelated(GenericMenu menu)
    {
        menu.AddItem(new GUIContent("Create/Light/Directional Light"), false, CreateDirectionalLight, 16);
        menu.AddItem(new GUIContent("Create/Light/Point Light"), false, CreatePointLight, 17);
        menu.AddItem(new GUIContent("Create/Light/Spotlight"), false, CreateSpotlight, 18);
        menu.AddItem(new GUIContent("Create/Light/Area Light"), false, CreateAreaLight, 19);
        menu.AddItem(new GUIContent("Create/Light/Reflection Probe"), false, CreateReflectionProbe, 23);
        menu.AddItem(new GUIContent("Create/Light/Light Probe Group"), false, CreateLightProbeGroup, 24);
    }

    static void DrawEffectsRelated(GenericMenu menu)
    {
        menu.AddItem(new GUIContent("Create/Effects/Particle System"), false, CreateParticleSystem, 27);
        menu.AddItem(new GUIContent("Create/Effects/Particle System Force Field"), false, CreateParticleSystemForceField, 28);
    }
    static void DrawMisc(GenericMenu menu)
    {
        menu.AddItem(new GUIContent("Create/Camera"), false, CreateCamera, 22);
    }
    #endregion
    #region Functionality
    static void SelectObject(object toSelect)
    {
        Selection.SetActiveObjectWithContext((UnityEngine.Object)toSelect, null);
    }

    static void ToggleActive(object obj)
    {
        foreach (Transform SelectedObject in SelectedObjects)
        {
            if (SelectedObject)
            {
                Undo.RecordObject(SelectedObject, "Undo toggle active");
                SelectedObject.gameObject.SetActive(!SelectedObject.gameObject.activeSelf);
                EditorUtility.SetDirty(SelectedObject);
                if (Selection.objects.Length <= 1)
                {
                    SelectObject(SelectedObject);
                }
            }
        }
    }
    static void ResetPos(object obj)
    {
        foreach (Transform SelectedObject in SelectedObjects)
        {
            if (SelectedObject)
            {
                Undo.RecordObject(SelectedObject.transform, "Undo position reset");
                SelectedObject.transform.localPosition = Vector3.zero;
                EditorUtility.SetDirty(SelectedObject.transform);
                if (Selection.objects.Length <= 1)
                {
                    SelectObject(SelectedObject);
                }
            }
        }
    }
    static void ResetRotation(object obj)
    {
        foreach (Transform SelectedObject in SelectedObjects)
        {
            if (SelectedObject)
            {
                Undo.RecordObject(SelectedObject.transform, "Undo rotation reset");
                SelectedObject.transform.rotation = Quaternion.identity;
                EditorUtility.SetDirty(SelectedObject.transform);
                if (Selection.objects.Length <= 1)
                {
                    SelectObject(SelectedObject);
                }
            }
        }
    }
    static void ResetScale(object obj)
    {
        foreach (Transform SelectedObject in SelectedObjects)
        {
            if (SelectedObject)
            {
                Undo.RecordObject(SelectedObject.transform, "Undo scale reset");
                SelectedObject.transform.localScale = new Vector3(1, 1, 1);
                EditorUtility.SetDirty(SelectedObject.transform);
                if (Selection.objects.Length <= 1)
                {
                    SelectObject(SelectedObject);
                }
            }
        }
    }
    static void AddComponentToObject(Type component)
    {
        foreach (Transform SelectedObject in SelectedObjects)
        {
            if (SelectedObject)
            {
                Undo.AddComponent(SelectedObject.gameObject, component);
                if (Selection.objects.Length <= 1)
                {
                    SelectObject(SelectedObject);
                }
            }
        }

    }
    static void CreatePrimitiveObject(PrimitiveType primitiveType)
    {
        GameObject g = GameObject.CreatePrimitive(primitiveType);
        g.transform.position = currentMouseWorldpos;
        Undo.RegisterCreatedObjectUndo(g, "Create " + primitiveType);
        SelectObject(g);
    }
    static GameObject CreateObjectWithComponents(string name, Type[] components)
    {
        GameObject gObject = new GameObject(name);
        gObject.transform.position = currentMouseWorldpos;
        gObject.transform.eulerAngles = new Vector3(0, 0, 0);
        if (components != null)
        {
            foreach (var component in components)
            {
                gObject.AddComponent(component);
            }
        }

        Undo.RegisterCreatedObjectUndo(gObject, "Create " + name);
        SelectObject(gObject);
        return gObject;
    }

    static void CreateLightGeneric(string name, LightType type)
    {
        GameObject g = new GameObject(name);
        g.transform.position = currentMouseWorldpos;
        Light l = g.AddComponent<Light>();
        l.type = type;
        Undo.RegisterCreatedObjectUndo(g, "Create " + name);
        SelectObject(g);
    }
    static void SelectParent(object obj)
    {
        Selection.activeObject = SelectedObjects[0].transform.parent;
    }

    static void ParentToSelected(object obj)
    {
        Transform toParent = SelectedObjects[0].transform;
        Undo.SetTransformParent(toParent, Selection.activeTransform, "Undo set parent");
    }
    static void ApplyToPrefab(object obj)
    {
        PrefabUtility.ApplyPrefabInstance(SelectedObjects[0].gameObject, InteractionMode.UserAction);
    }

    static void RevertToPrefab(object obj)
    {
        PrefabUtility.RevertPrefabInstance(SelectedObjects[0].gameObject, InteractionMode.UserAction);
    }
    #endregion
    #region Unity UI Requirements
    static void AddRigidbody(object obj)
    {
        AddComponentToObject(typeof(Rigidbody));
    }

    static void AddBoxCollider(object obj)
    {
        AddComponentToObject(typeof(BoxCollider));
    }

    static void AddSphereCollider(object obj)
    {
        AddComponentToObject(typeof(SphereCollider));
    }

    static void AddCapsuleCollider(object obj)
    {
        AddComponentToObject(typeof(CapsuleCollider));
    }

    static void AddMeshCollider(object obj)
    {
        AddComponentToObject(typeof(MeshCollider));
    }

    static void AddAudioSource(object obj)
    {
        AddComponentToObject(typeof(AudioSource));
    }

    static void CreateCube(object obj)
    {
        CreatePrimitiveObject(PrimitiveType.Cube);
    }

    static void CreateSphere(object obj)
    {
        CreatePrimitiveObject(PrimitiveType.Sphere);
    }

    static void CreateCapsule(object obj)
    {
        CreatePrimitiveObject(PrimitiveType.Capsule);
    }

    static void CreatePlane(object obj)
    {
        CreatePrimitiveObject(PrimitiveType.Plane);
    }

    static void CreateCylinder(object obj)
    {
        CreatePrimitiveObject(PrimitiveType.Cylinder);

    }

    static void CreateQuad(object obj)
    {
        CreatePrimitiveObject(PrimitiveType.Quad);
    }

    static void CreateDirectionalLight(object obj)
    {
        CreateLightGeneric("Directional Light", LightType.Directional);
    }
    static void CreatePointLight(object obj)
    {
        CreateLightGeneric("Point Light", LightType.Point);
    }
    static void CreateSpotlight(object obj)
    {
        CreateLightGeneric("Spotlight", LightType.Spot);
    }
    static void CreateAreaLight(object obj)
    {
        CreateLightGeneric("Area Light", LightType.Area);
    }
    static void CreateCamera(object obj)
    {
        CreateObjectWithComponents("Camera", new Type[] { typeof(Camera), typeof(AudioListener) });
    }
    static void CreateReflectionProbe(object obj)
    {
        CreateObjectWithComponents("Reflection Probe", new Type[] { typeof(ReflectionProbe) });
    }

    static void CreateLightProbeGroup(object obj)
    {
        CreateObjectWithComponents("Light Probe Group", new Type[] { typeof(LightProbeGroup) });
    }

    static void CreateEmptyGameObject(object obj)
    {
        CreateObjectWithComponents("GameObject", null);
    }

    static void CreateParticleSystem(object obj)
    {
        GameObject gObject = CreateObjectWithComponents("Particle System", new Type[] { typeof(ParticleSystem) });
        gObject.GetComponent<Renderer>().material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-ParticleSystem.mat");
    }

    static void CreateParticleSystemForceField(object obj)
    {
        CreateObjectWithComponents("Particle System", new Type[] { typeof(ParticleSystemForceField) });
    }
    #endregion
}
#endif