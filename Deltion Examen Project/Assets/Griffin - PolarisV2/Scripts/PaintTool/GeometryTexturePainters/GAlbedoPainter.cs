using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pinwheel.Griffin.PaintTool;

namespace Pinwheel.Griffin.PaintTool
{
    public class GAlbedoPainter : IGTexturePainter, IGTexturePainterWithLivePreview
    {
        public string Instruction
        {
            get
            {
                string s = string.Format(
                    "Modify terrain Albedo Map color.\n" +
                    "   - Hold Left Mouse to paint.\n" +
                    "   - Hold Ctrl & Left Mouse to erase.\n" +
                    "Use a material that utilizes Albedo Map to see the result.");
                return s;
            }
        }

        public string HistoryPrefix
        {
            get
            {
                return "Albedo Painting";
            }
        }

        public List<GTerrainResourceFlag> GetResourceFlagForHistory(GTexturePainterArgs args)
        {
            return GCommon.AlbedoResourceFlags;
        }

        public void Paint(Pinwheel.Griffin.GStylizedTerrain terrain, GTexturePainterArgs args)
        {
            if (terrain.TerrainData == null)
                return;
            if (args.MouseEventType == GPainterMouseEventType.Up)
            {
                return;
            }

            Vector2[] uvCorners = new Vector2[args.WorldPointCorners.Length];
            for (int i = 0; i < uvCorners.Length; ++i)
            {
                uvCorners[i] = terrain.WorldPointToUV(args.WorldPointCorners[i]);
            }

            Rect dirtyRect = GUtilities.GetRectContainsPoints(uvCorners);
            if (!dirtyRect.Overlaps(new Rect(0, 0, 1, 1)))
                return;

            Texture2D bg = terrain.TerrainData.Shading.Internal_AlbedoMap;
            int albedoResolution = terrain.TerrainData.Shading.AlbedoMapResolution;
            RenderTexture rt = GTerrainTexturePainter.Internal_GetRenderTexture(albedoResolution);
            GCommon.CopyToRT(bg, rt);

            Material mat = GInternalMaterials.AlbedoPainterMaterial;
            mat.SetColor("_Color", args.Color);
            mat.SetTexture("_MainTex", bg);
            mat.SetTexture("_Mask", args.Mask);
            mat.SetFloat("_Opacity", args.Opacity);
            int pass =
                args.ActionType == GPainterActionType.Normal ? 0 :
                args.ActionType == GPainterActionType.Negative ? 1 :
                args.ActionType == GPainterActionType.Alternative ? 2 : 0;
            GCommon.DrawQuad(rt, uvCorners, mat, pass);

            RenderTexture.active = rt;
            terrain.TerrainData.Shading.Internal_AlbedoMap.ReadPixels(
                new Rect(0, 0, albedoResolution, albedoResolution), 0, 0);
            terrain.TerrainData.Shading.Internal_AlbedoMap.Apply();
            RenderTexture.active = null;

            terrain.TerrainData.SetDirty(GTerrainData.DirtyFlags.Shading);

            if (!args.ForceUpdateGeometry)
                return;
            terrain.TerrainData.Geometry.SetRegionDirty(dirtyRect);
            terrain.TerrainData.SetDirty(GTerrainData.DirtyFlags.Geometry);
        }
        
        public void Editor_DrawLivePreview(GStylizedTerrain terrain, GTexturePainterArgs args, Camera cam)
        {
#if UNITY_EDITOR
            Vector2[] uvCorners = new Vector2[args.WorldPointCorners.Length];
            for (int i = 0; i < uvCorners.Length; ++i)
            {
                uvCorners[i] = terrain.WorldPointToUV(args.WorldPointCorners[i]);
            }

            Rect dirtyRect = GUtilities.GetRectContainsPoints(uvCorners);
            if (!dirtyRect.Overlaps(new Rect(0, 0, 1, 1)))
                return;

            Texture2D bg = terrain.TerrainData.Shading.Internal_AlbedoMap;
            int albedoResolution = terrain.TerrainData.Shading.AlbedoMapResolution;
            RenderTexture rt = GTerrainTexturePainter.Internal_GetRenderTexture(terrain, albedoResolution);
            GCommon.CopyToRT(bg, rt);

            Material mat = GInternalMaterials.AlbedoPainterMaterial;
            mat.SetColor("_Color", args.Color);
            mat.SetTexture("_MainTex", bg);
            mat.SetTexture("_Mask", args.Mask);
            mat.SetFloat("_Opacity", args.Opacity);
            int pass =
                args.ActionType == GPainterActionType.Normal ? 0 :
                args.ActionType == GPainterActionType.Negative ? 1 :
                args.ActionType == GPainterActionType.Alternative ? 2 : 0;
            GCommon.DrawQuad(rt, uvCorners, mat, pass);

            GLivePreviewDrawer.DrawAlbedoLivePreview(terrain, cam, rt, dirtyRect);
#endif
        }
    }
}
