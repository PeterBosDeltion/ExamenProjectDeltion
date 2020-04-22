using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using Pinwheel.Griffin.GroupTool;
using Pinwheel.Griffin.PaintTool;
using Pinwheel.Griffin.BackupTool;
using Pinwheel.Griffin.SplineTool;
using Pinwheel.Griffin.BillboardTool;
using Pinwheel.Griffin.StampTool;
using Pinwheel.Griffin.HelpTool;
using Pinwheel.Griffin.DataTool;
using Pinwheel.Griffin.ExtensionSystem;

namespace Pinwheel.Griffin
{
    public static class GEditorMenus
    {
        [MenuItem("GameObject/3D Object/Griffin/Stylized Terrain (Wizard)", false, -10)]
        public static void ShowCreateTerrainWizard(MenuCommand menuCmd)
        {
            GTerrainWizardWindow.ShowWindow(menuCmd);
        }

        [MenuItem("GameObject/3D Object/Griffin/Stylized Terrain", false, -10)]
        public static GStylizedTerrain CreateStylizedTerrain(MenuCommand menuCmd)
        {
            GameObject environmentRoot = null;
            if (menuCmd != null && menuCmd.context != null)
            {
                environmentRoot = menuCmd.context as GameObject;
            }
            if (environmentRoot == null)
            {
                environmentRoot = new GameObject("Griffin Environment");
                GUtilities.ResetTransform(environmentRoot.transform, null);
            }

            GStylizedTerrain t = CreateStylizedTerrain();
            GameObjectUtility.SetParentAndAlign(t.gameObject, environmentRoot);

            return t;
        }

        public static GStylizedTerrain CreateStylizedTerrain(GTerrainData srcData = null)
        {
            GameObject g = new GameObject("Stylized Terrain");

            g.transform.localPosition = Vector3.zero;
            g.transform.localRotation = Quaternion.identity;
            g.transform.localScale = Vector3.one;

            GTerrainData terrainData = GTerrainData.CreateInstance<GTerrainData>();
            terrainData.Reset();
            string assetName = "TerrainData_" + terrainData.Id;
            //AssetDatabase.CreateAsset(terrainData, string.Format("Assets/{0}.asset", assetName));
            GUtilities.EnsureDirectoryExists(GGriffinSettings.Instance.DefaultTerrainDataDirectory);
            AssetDatabase.CreateAsset(terrainData, Path.Combine(GGriffinSettings.Instance.DefaultTerrainDataDirectory, assetName + ".asset"));
            if (srcData != null)
            {
                srcData.CopyTo(terrainData);
            }

            Material mat = GGriffinSettings.Instance.WizardSettings.GetClonedMaterial();
            if (mat == null && GGriffinSettings.Instance.TerrainDataDefault.Shading.CustomMaterial != null)
            {
                mat = Object.Instantiate(GGriffinSettings.Instance.TerrainDataDefault.Shading.CustomMaterial);
            }
            if (mat != null)
            {
                string matName = "TerrainMaterial_" + terrainData.Id;
                mat.name = matName;
                //AssetDatabase.CreateAsset(mat, string.Format("Assets/{0}.mat", matName));
                GUtilities.EnsureDirectoryExists(GGriffinSettings.Instance.DefaultTerrainDataDirectory);
                AssetDatabase.CreateAsset(mat, Path.Combine(GGriffinSettings.Instance.DefaultTerrainDataDirectory, matName + ".mat"));
                terrainData.Shading.CustomMaterial = mat;
            }

            GStylizedTerrain terrain = g.AddComponent<GStylizedTerrain>();
            terrain.GroupId = 0;
            terrain.TerrainData = terrainData;

            Undo.RegisterCreatedObjectUndo(g, "Creating Stylized Terrain");

            GameObject colliderGO = new GameObject("Tree Collider");
            GameObjectUtility.SetParentAndAlign(colliderGO, terrain.gameObject);
            colliderGO.transform.localPosition = Vector3.zero;
            colliderGO.transform.localRotation = Quaternion.identity;
            colliderGO.transform.localScale = Vector3.one;

            GTreeCollider collider = colliderGO.AddComponent<GTreeCollider>();
            collider.Terrain = terrain;

            GTerrainTools tools = Object.FindObjectOfType<GTerrainTools>();
            if (tools == null)
                CreateTerrainTools(null);

            Selection.activeGameObject = g;
            return terrain;
        }

        [MenuItem("GameObject/3D Object/Griffin/Tree Collider", false, -10)]
        public static void CreateTreeCollider(MenuCommand menuCmd)
        {
            GameObject g = new GameObject("Tree Collider");
            if (menuCmd != null)
                GameObjectUtility.SetParentAndAlign(g, menuCmd.context as GameObject);
            g.transform.localPosition = Vector3.zero;
            g.transform.localRotation = Quaternion.identity;
            g.transform.localScale = Vector3.one;

            GTreeCollider collider = g.AddComponent<GTreeCollider>();
            if (g.transform.parent != null)
            {
                GStylizedTerrain terrain = g.transform.parent.GetComponent<GStylizedTerrain>();
                collider.Terrain = terrain;
            }

            Undo.RegisterCreatedObjectUndo(g, "Creating Tree Collider");
            Selection.activeGameObject = g;
        }

        [MenuItem("GameObject/3D Object/Griffin/Wind Zone", false, -10)]
        public static GWindZone CreateWindZone(MenuCommand menuCmd)
        {
            GameObject root = null;
            if (menuCmd != null && menuCmd.context != null)
            {
                root = menuCmd.context as GameObject;
            }

            GameObject windZoneGO = new GameObject("Wind Zone");
            GWindZone windZone = windZoneGO.AddComponent<GWindZone>();
            GameObjectUtility.SetParentAndAlign(windZoneGO, root);

            return windZone;
        }

        [MenuItem("GameObject/3D Object/Griffin/Tools/Basic Tools", false, -10)]
        public static void CreateTerrainTools(MenuCommand menuCmd)
        {
            GameObject g = new GameObject("Griffin Tools");
            if (menuCmd != null)
                GameObjectUtility.SetParentAndAlign(g, menuCmd.context as GameObject);
            g.transform.localPosition = Vector3.zero;
            g.transform.localRotation = Quaternion.identity;
            g.transform.localScale = Vector3.one;
            g.transform.hideFlags = HideFlags.HideInInspector;

            GTerrainTools tools = g.AddComponent<GTerrainTools>();
            tools.hideFlags = HideFlags.HideInInspector;

            GameObject terrainGroup = new GameObject("Terrain Group");
            GUtilities.ResetTransform(terrainGroup.transform, g.transform);
            terrainGroup.transform.hideFlags = HideFlags.HideInInspector;
            GTerrainGroup group = terrainGroup.AddComponent<GTerrainGroup>();
            group.GroupId = -1;

            GameObject texturePainter = new GameObject("Geometry & Texture Painter");
            GUtilities.ResetTransform(texturePainter.transform, g.transform);
            texturePainter.transform.hideFlags = HideFlags.HideInInspector;
            GTerrainTexturePainter texturePainterComponent = texturePainter.AddComponent<GTerrainTexturePainter>();
            texturePainterComponent.GroupId = -1;

            GameObject foliagePainter = new GameObject("Foliage Painter");
            GUtilities.ResetTransform(foliagePainter.transform, g.transform);
            foliagePainter.transform.hideFlags = HideFlags.HideInInspector;
            GFoliagePainter foliagePainterComponent = foliagePainter.AddComponent<GFoliagePainter>();
            foliagePainterComponent.GroupId = -1;
            foliagePainterComponent.gameObject.AddComponent<GRotationRandomizeFilter>();
            foliagePainterComponent.gameObject.AddComponent<GScaleRandomizeFilter>();

            GameObject objectPainter = new GameObject("Object Painter");
            GUtilities.ResetTransform(objectPainter.transform, g.transform);
            objectPainter.transform.hideFlags = HideFlags.HideInInspector;
            GObjectPainter objectPainterComponent = objectPainter.AddComponent<GObjectPainter>();
            objectPainterComponent.GroupId = -1;
            objectPainterComponent.gameObject.AddComponent<GRotationRandomizeFilter>();
            objectPainterComponent.gameObject.AddComponent<GScaleRandomizeFilter>();

            GameObject assetExplorer = new GameObject("Asset Explorer");
            GUtilities.ResetTransform(assetExplorer.transform, g.transform);
            assetExplorer.transform.hideFlags = HideFlags.HideInInspector;
            assetExplorer.AddComponent<GAssetExplorer>();

            GameObject helpTool = new GameObject("Help");
            GUtilities.ResetTransform(helpTool.transform, g.transform);
            helpTool.transform.hideFlags = HideFlags.HideInInspector;
            helpTool.AddComponent<GHelpComponent>();

            Selection.activeGameObject = terrainGroup;
            Undo.RegisterCreatedObjectUndo(g, "Creating Terrain Tools");
        }

        [MenuItem("GameObject/3D Object/Griffin/Tools/Group", false, 10)]
        public static void CreateGroupTool(MenuCommand menuCmd)
        {
            GameObject g = new GameObject("Terrain Group");
            if (menuCmd != null)
                GameObjectUtility.SetParentAndAlign(g, menuCmd.context as GameObject);
            g.transform.localPosition = Vector3.zero;
            g.transform.hideFlags = HideFlags.HideInInspector;
            GTerrainGroup group = g.AddComponent<GTerrainGroup>();
            group.GroupId = -1;

            Selection.activeGameObject = g;
            Undo.RegisterCreatedObjectUndo(g, "Creating Terrain Group");
        }

        [MenuItem("GameObject/3D Object/Griffin/Tools/Geometry - Texture Painter", false, 10)]
        public static void CreateTexturePainter(MenuCommand menuCmd)
        {
            GameObject g = new GameObject("Geometry & Texture Painter");
            if (menuCmd != null)
                GameObjectUtility.SetParentAndAlign(g, menuCmd.context as GameObject);
            g.transform.localPosition = Vector3.zero;
            g.transform.hideFlags = HideFlags.HideInInspector;
            GTerrainTexturePainter painter = g.AddComponent<GTerrainTexturePainter>();
            painter.GroupId = -1;

            Selection.activeGameObject = g;
            Undo.RegisterCreatedObjectUndo(g, "Creating Texture Painter");
        }

        [MenuItem("GameObject/3D Object/Griffin/Tools/Foliage Painter", false, 10)]
        public static void CreateFoliagePainter(MenuCommand menuCmd)
        {
            GameObject g = new GameObject("Foliage Painter");
            if (menuCmd != null)
                GameObjectUtility.SetParentAndAlign(g, menuCmd.context as GameObject);
            g.transform.localPosition = Vector3.zero;
            g.transform.hideFlags = HideFlags.HideInInspector;
            GFoliagePainter painter = g.AddComponent<GFoliagePainter>();
            painter.GroupId = -1;
            painter.gameObject.AddComponent<GRotationRandomizeFilter>();
            painter.gameObject.AddComponent<GScaleRandomizeFilter>();

            Selection.activeGameObject = g;
            Undo.RegisterCreatedObjectUndo(g, "Creating Foliage Painter");
        }

        [MenuItem("GameObject/3D Object/Griffin/Tools/Object Painter", false, 10)]
        public static void CreateObjectPainter(MenuCommand menuCmd)
        {
            GameObject g = new GameObject("Object Painter");
            if (menuCmd != null)
                GameObjectUtility.SetParentAndAlign(g, menuCmd.context as GameObject);
            g.transform.localPosition = Vector3.zero;
            g.transform.hideFlags = HideFlags.HideInInspector;
            GObjectPainter painter = g.AddComponent<GObjectPainter>();
            painter.GroupId = -1;
            painter.gameObject.AddComponent<GRotationRandomizeFilter>();
            painter.gameObject.AddComponent<GScaleRandomizeFilter>();

            Selection.activeGameObject = g;
            Undo.RegisterCreatedObjectUndo(g, "Creating Object Painter");
        }

        [MenuItem("GameObject/3D Object/Griffin/Tools/Spline", false, 10)]
        public static void CreateSpline(MenuCommand menuCmd)
        {
            GameObject g = new GameObject("Spline");
            if (menuCmd != null)
                GameObjectUtility.SetParentAndAlign(g, menuCmd.context as GameObject);
            g.transform.localPosition = Vector3.zero;
            g.transform.hideFlags = HideFlags.HideInInspector;
            GSplineCreator spline = g.AddComponent<GSplineCreator>();
            spline.GroupId = -1;

            Selection.activeGameObject = g;
            Undo.RegisterCreatedObjectUndo(g, "Creating Terrain Painter");
        }

        [MenuItem("GameObject/3D Object/Griffin/Tools/Geometry Stamper", false, 10)]
        public static void CreateGeometryStamper(MenuCommand menuCmd)
        {
            GameObject geometryStamperGO = new GameObject("Geometry Stamper");
            if (menuCmd != null)
                GameObjectUtility.SetParentAndAlign(geometryStamperGO, menuCmd.context as GameObject);
            geometryStamperGO.transform.localPosition = Vector3.zero;
            geometryStamperGO.transform.hideFlags = HideFlags.HideInInspector;
            GGeometryStamper geoStamper = geometryStamperGO.AddComponent<GGeometryStamper>();
            geoStamper.GroupId = -1;

            Selection.activeGameObject = geometryStamperGO;
            Undo.RegisterCreatedObjectUndo(geometryStamperGO, "Creating Geometry Stamper");
        }

        [MenuItem("GameObject/3D Object/Griffin/Tools/Texture Stamper", false, 10)]
        public static void CreateTextureStamper(MenuCommand menuCmd)
        {
            GameObject textureStamperGO = new GameObject("Texture Stamper");
            if (menuCmd != null)
                GameObjectUtility.SetParentAndAlign(textureStamperGO, menuCmd.context as GameObject);
            textureStamperGO.transform.localPosition = Vector3.zero;
            textureStamperGO.transform.hideFlags = HideFlags.HideInInspector;
            GTextureStamper texStamper = textureStamperGO.AddComponent<GTextureStamper>();
            texStamper.GroupId = -1;

            Selection.activeGameObject = textureStamperGO;
            Undo.RegisterCreatedObjectUndo(textureStamperGO, "Creating Texture Stamper");
        }

        [MenuItem("GameObject/3D Object/Griffin/Tools/Foliage Stamper", false, 10)]
        public static void CreateFoliageStamper(MenuCommand menuCmd)
        {
            GameObject foliageStamperGO = new GameObject("Foliage Stamper");
            if (menuCmd != null)
                GameObjectUtility.SetParentAndAlign(foliageStamperGO, menuCmd.context as GameObject);
            foliageStamperGO.transform.localPosition = Vector3.zero;
            foliageStamperGO.transform.hideFlags = HideFlags.HideInInspector;
            GFoliageStamper foliageStamper = foliageStamperGO.AddComponent<GFoliageStamper>();
            foliageStamper.GroupId = -1;

            Selection.activeGameObject = foliageStamperGO;
            Undo.RegisterCreatedObjectUndo(foliageStamperGO, "Creating Foliage Stampers");
        }

        [MenuItem("GameObject/3D Object/Griffin/Tools/Object Stamper", false, 10)]
        public static void CreateObjectStamper(MenuCommand menuCmd)
        {
            GameObject objectStamperGO = new GameObject("Object Stamper");
            if (menuCmd != null)
                GameObjectUtility.SetParentAndAlign(objectStamperGO, menuCmd.context as GameObject);
            objectStamperGO.transform.localPosition = Vector3.zero;
            objectStamperGO.transform.hideFlags = HideFlags.HideInInspector;
            GObjectStamper objectStamper = objectStamperGO.AddComponent<GObjectStamper>();
            objectStamper.GroupId = -1;

            Selection.activeGameObject = objectStamperGO;
            Undo.RegisterCreatedObjectUndo(objectStamperGO, "Creating Object Stampers");
        }

        [MenuItem("GameObject/3D Object/Griffin/Tools/Navigation Helper", false, 10)]
        public static void CreateNavigationHelper(MenuCommand menuCmd)
        {
            GameObject navHelperGO = new GameObject("Navigation Helper");
            if (menuCmd != null)
                GameObjectUtility.SetParentAndAlign(navHelperGO, menuCmd.context as GameObject);
            navHelperGO.transform.localPosition = Vector3.zero;
            navHelperGO.transform.hideFlags = HideFlags.HideInInspector;
            GNavigationHelper nav = navHelperGO.AddComponent<GNavigationHelper>();
            nav.GroupId = -1;

            Selection.activeGameObject = navHelperGO;
            Undo.RegisterCreatedObjectUndo(navHelperGO, "Creating Navigation Helper");
        }

        public static bool ValidateShowUnityTerrainConverter(MenuCommand menuCmd)
        {
            if (menuCmd == null)
                return false;
            GameObject go = menuCmd.context as GameObject;
            if (go == null)
                return false;
            Terrain[] terrains = go.GetComponentsInChildren<Terrain>();
            bool valid = false;
            for (int i = 0; i < terrains.Length; ++i)
            {
                if (terrains[i].terrainData != null)
                {
                    valid = true;
                    break;
                }
            }

            return valid;
        }

        [MenuItem("GameObject/3D Object/Griffin/Convert From Unity Terrain", false, 11)]
        public static void ShowUnityTerrainConverter(MenuCommand menuCmd)
        {
            bool valid = ValidateShowUnityTerrainConverter(menuCmd);
            if (!valid)
            {
                Debug.Log("No Terrain with Terrain Data found!");
                return;
            }

            GUnityTerrainGroupConverterWindow window = GUnityTerrainGroupConverterWindow.ShowWindow();
            GameObject g = menuCmd.context as GameObject;
            window.Root = g;
        }

        [MenuItem("Window/Griffin/Tools/Backup")]
        public static void ShowBackupWindow()
        {
            GBackupEditor.ShowWindow();
        }

        [MenuItem("Assets/Create/Griffin/Billboard Asset")]
        [MenuItem("Window/Griffin/Tools/Billboard Creator")]
        public static void ShowBillboardEditor()
        {
            GBillboardEditor.ShowWindow();
        }

        [MenuItem("Window/Griffin/Tools/Extensions", false, 1000000)]
        public static void ShowExtensionWindow()
        {
            GExtensionWindow.ShowWindow();
        }

        [MenuItem("Window/Griffin/Project/Version Info")]
        public static void ShowVersionInfo()
        {
            EditorUtility.DisplayDialog(
                "Version Info",
                GVersionInfo.ProductNameAndVersion,
                "OK");
        }

        [MenuItem("Window/Griffin/Project/Update Dependencies")]
        public static void UpdateDependencies()
        {
            GPackageInitializer.Init();
        }

        [MenuItem("Window/Griffin/Project/Settings")]
        public static void ShowSettings()
        {
            Selection.activeObject = GGriffinSettings.Instance;
        }

        [MenuItem("Window/Griffin/Learning Resources/Online Manual")]
        public static void ShowOnlineUserGuide()
        {
            GAnalytics.Record(GAnalytics.LINK_ONLINE_MANUAL);
            Application.OpenURL(GCommon.ONLINE_MANUAL);
        }

        [MenuItem("Window/Griffin/Learning Resources/Youtube Channel")]
        public static void ShowYoutubeChannel()
        {
            GAnalytics.Record(GAnalytics.LINK_YOUTUBE);
            Application.OpenURL(GCommon.YOUTUBE_CHANNEL);
        }

        [MenuItem("Window/Griffin/Learning Resources/Facebook Page")]
        public static void ShowFacebookPage()
        {
            GAnalytics.Record(GAnalytics.LINK_FACEBOOK);
            Application.OpenURL(GCommon.FACEBOOK_PAGE);
        }

        [MenuItem("Window/Griffin/Learning Resources/Help")]
        public static void ShowHelpEditor()
        {
            GHelpEditor.ShowWindow();
        }

        [MenuItem("Window/Griffin/Explore/Assets From Pinwheel")]
        public static void ShowAssetsFromPinwheel()
        {
            GAssetExplorer.ShowPinwheelAssets();
        }

        [MenuItem("Window/Griffin/Explore/Stylized Vegetation Assets")]
        public static void ShowStylizedVegetationLink()
        {
            GAnalytics.Record(GAnalytics.LINK_EXPLORE_ASSET);
            GAssetExplorer.ShowStylizedVegetationLink();
        }

        [MenuItem("Window/Griffin/Explore/Stylized Rock - Props Assets")]
        public static void ShowStylizedRockPropsLink()
        {
            GAnalytics.Record(GAnalytics.LINK_EXPLORE_ASSET);
            GAssetExplorer.ShowStylizedRockPropsLink();
        }

        [MenuItem("Window/Griffin/Explore/Stylized Water Assets")]
        public static void ShowStylizedWaterLink()
        {
            GAnalytics.Record(GAnalytics.LINK_EXPLORE_ASSET);
            GAssetExplorer.ShowStylizedRockPropsLink();
        }

        [MenuItem("Window/Griffin/Explore/Stylized Sky - Ambient Assets")]
        public static void ShowStylizedSkyAmbientLink()
        {
            GAnalytics.Record(GAnalytics.LINK_EXPLORE_ASSET);
            GAssetExplorer.ShowStylizedSkyAmbientLink();
        }

        [MenuItem("Window/Griffin/Explore/Stylized Character Assets")]
        public static void ShowStylizedCharacterLink()
        {
            GAnalytics.Record(GAnalytics.LINK_EXPLORE_ASSET);
            GAssetExplorer.ShowStylizedCharacterLink();
        }

        [MenuItem("Window/Griffin/Contact/Support")]
        public static void ShowSupportEmailEditor()
        {
            GEditorCommon.OpenEmailEditor(
                GCommon.SUPPORT_EMAIL,
                "[Polaris V2] SHORT_QUESTION_HERE",
                "YOUR_QUESTION_IN_DETAIL");
        }

        [MenuItem("Window/Griffin/Contact/Business")]
        public static void ShowBusinessEmailEditor()
        {
            GEditorCommon.OpenEmailEditor(
                GCommon.BUSINESS_EMAIL,
                "[Polaris V2] SHORT_MESSAGE_HERE",
                "YOUR_MESSAGE_IN_DETAIL");
        }

        [MenuItem("Window/Griffin/Explore/NEW PRODUCT")]
        public static void ShowNewProductAds()
        {
            GAnalytics.Record(GAnalytics.LINK_EXPLORE_ASSET);
            GProductAdsWindow.ShowWindow();
        }

        //[MenuItem("Window/Griffin/Internal/Clear Progress Bar")]
        //public static void ClearProgressBar()
        //{
        //    GCommon.Editor_ProgressBar("", "", 1f);
        //    GCommon.Editor_ClearProgressBar();
        //}

        //[MenuItem("Window/Griffin/Internal/Reload Editor Layout")]
        //public static void ReloadEditorLayout()
        //{

        //}
    }
}
