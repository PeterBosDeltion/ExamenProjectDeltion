using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pinwheel.Griffin.PaintTool;

namespace Pinwheel.Griffin.PaintTool
{
    public class GMetallicPainter : IGTexturePainter, IGTexturePainterWithLivePreview
    {
        public string Instruction
        {
            get
            {
                string s = string.Format(
                    "Modify terrain Metallic Map Red channel.\n" +
                    "   - Hold Left Mouse to paint.\n" +
                    "   - Hold Ctrl & Left Mouse to erase.\n" +
                    "   - Hold Shift & Left Mouse to smooth.\n" +
                    "This painter uses the Red channel of brush color.\n" +
                    "Use a material that utilizes Metallic Map to see the result.");
                return s;
            }
        }

        public string HistoryPrefix
        {
            get
            {
                return "Metallic Painting";
            }
        }

        public List<GTerrainResourceFlag> GetResourceFlagForHistory(GTexturePainterArgs args)
        {
            return GCommon.MetallicResourceFlags;
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

            Texture2D bg = terrain.TerrainData.Shading.Internal_MetallicMap;
            int metallicResolution = terrain.TerrainData.Shading.MetallicMapResolution;
            RenderTexture rt = GTerrainTexturePainter.Internal_GetRenderTexture(metallicResolution);
            GCommon.CopyToRT(bg, rt);

            Material mat = GInternalMaterials.MetallicPainterMaterial;
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
            terrain.TerrainData.Shading.Internal_MetallicMap.ReadPixels(
                new Rect(0, 0, metallicResolution, metallicResolution), 0, 0);
            terrain.TerrainData.Shading.Internal_MetallicMap.Apply();
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

            Texture2D bg = terrain.TerrainData.Shading.Internal_MetallicMap;
            int metallicResolution = terrain.TerrainData.Shading.MetallicMapResolution;
            RenderTexture rt = GTerrainTexturePainter.Internal_GetRenderTexture(terrain, metallicResolution);
            GCommon.CopyToRT(bg, rt);

            Material mat = GInternalMaterials.MetallicPainterMaterial;
            mat.SetColor("_Color", args.Color);
            mat.SetTexture("_MainTex", bg);
            mat.SetTexture("_Mask", args.Mask);
            mat.SetFloat("_Opacity", args.Opacity);
            int pass =
                args.ActionType == GPainterActionType.Normal ? 0 :
                args.ActionType == GPainterActionType.Negative ? 1 :
                args.ActionType == GPainterActionType.Alternative ? 2 : 0;
            GCommon.DrawQuad(rt, uvCorners, mat, pass);

            GLivePreviewDrawer.DrawMetallicSmoothnessLivePreview(terrain, cam, rt, dirtyRect);
#endif
        }
    }
}
