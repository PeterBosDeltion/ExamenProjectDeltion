using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pinwheel.Griffin.TextureTool;

namespace Pinwheel.Griffin.TextureTool
{
    public class GSteepnessMapGenerator : IGTextureGenerator
    {
        private static Material mat;
        private static Material Mat
        {
            get
            {
                if (mat==null)
                {
                    mat = new Material(GGriffinSettings.Instance.InternalShaderSettings.SteepnessMapGeneratorShader);
                }
                return mat;
            }
        }

        public void Generate(RenderTexture targetRt)
        {
            GSteepnessMapGeneratorParams param = GTextureToolParams.Instance.Steepness;
            if (param.Terrain == null || param.Terrain.TerrainData == null)
            {
                GCommon.FillTexture(targetRt, Color.clear);
            }
            else
            {
                GNormalMapGeneratorParams normalMapParam = new GNormalMapGeneratorParams();
                normalMapParam.Terrain = param.Terrain;
                normalMapParam.Space = GNormalMapSpace.Local;
                normalMapParam.Mode = param.Mode;

                RenderTexture normalMap = new RenderTexture(targetRt);
                GNormalMapGenerator gen = new GNormalMapGenerator();
                if (param.Mode == GNormalMapMode.Sharp)
                {
                    gen.RenderSharpNormalMap(normalMapParam, normalMap);
                }
                else if (param.Mode == GNormalMapMode.Interpolated)
                {
                    gen.RenderInterpolatedNormalMap(normalMapParam, normalMap);
                }
                else if (param.Mode == GNormalMapMode.PerPixel)
                {
                    gen.RenderPerPixelNormalMap(normalMapParam, normalMap);
                }

                Mat.SetTexture("_BumpMap", normalMap);
                GCommon.DrawQuad(targetRt, GCommon.FullRectUvPoints, Mat, 0);

                normalMap.Release();
                GUtilities.DestroyObject(normalMap);
            }
        }
    }
}
