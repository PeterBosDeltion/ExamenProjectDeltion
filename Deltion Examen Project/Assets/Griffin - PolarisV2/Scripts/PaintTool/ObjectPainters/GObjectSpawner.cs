using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pinwheel.Griffin.PaintTool;
using Type = System.Type;

namespace Pinwheel.Griffin.PaintTool
{
    public class GObjectSpawner : IGObjectPainter
    {
        public string Instruction
        {
            get
            {
                return string.Format(
                    "Spawn game object into the scene.\n" +
                    "   - Hold Left Mouse to spawn.\n" +
                    "   - Hold Ctrl & Left Mouse to erase.");
            }
        }

        public List<Type> SuitableFilterTypes
        {
            get
            {
                List<Type> types = new List<Type>(new Type[]
                {
                    typeof(GAlignToSurfaceFilter),
                    typeof(GHeightConstraintFilter),
                    typeof(GSlopeConstraintFilter),
                    typeof(GRotationRandomizeFilter),
                    typeof(GScaleRandomizeFilter),
                    typeof(GScaleClampFilter)
                });
                return types;
            }
        }

        public void Paint(Pinwheel.Griffin.GStylizedTerrain terrain, GObjectPainterArgs args)
        {
            if (args.MouseEventType == GPainterMouseEventType.Up)
                return;

            Vector2[] uvCorners = new Vector2[args.WorldPointCorners.Length];
            for (int i = 0; i < uvCorners.Length; ++i)
            {
                uvCorners[i] = terrain.WorldPointToUV(args.WorldPointCorners[i]);
            }

            Rect dirtyRect = GUtilities.GetRectContainsPoints(uvCorners);
            if (!dirtyRect.Overlaps(new Rect(0, 0, 1, 1)))
                return;

            if (args.ActionType == GPainterActionType.Normal)
            {
                HandleSpawnObject(terrain, args);
            }
            else if (args.ActionType == GPainterActionType.Negative)
            {
                HandleEraseObject(terrain, args);
            }
        }

        private void HandleSpawnObject(GStylizedTerrain terrain, GObjectPainterArgs args)
        {
            int prototypeIndex = -1;
            Vector3 randomPos = Vector3.zero;
            Vector3 rayOrigin = Vector3.zero;
            Vector3 rayDirection = Vector3.down;
            float sqrtTwo = Mathf.Sqrt(2);
            Ray ray = new Ray();
            RaycastHit samplePoint;
            Vector3 bary0 = Vector3.zero;
            Vector3 bary1 = Vector3.zero;
            Vector2 maskUv = Vector2.zero;
            Color maskColor = Color.white;
            Texture2D clonedMask = null;
            if (args.Mask != null)
            {
                clonedMask = GCommon.CloneAndResizeTexture(args.Mask, 256, 256);
            }

            for (int i = 0; i < args.Density; ++i)
            {
                prototypeIndex = args.PrototypeIndices[Random.Range(0, args.PrototypeIndices.Count)];
                if (prototypeIndex < 0 || prototypeIndex >= args.Prototypes.Count)
                    continue;
                GameObject g = args.Prototypes[prototypeIndex];
                if (g == null)
                    continue;

                randomPos = args.HitPoint + Random.insideUnitSphere * args.Radius * sqrtTwo;
                rayOrigin.Set(
                    randomPos.x,
                    10000,
                    randomPos.z);
                ray.origin = rayOrigin;
                ray.direction = rayDirection;
                if (terrain.Raycast(ray, out samplePoint, float.MaxValue))
                {
                    GUtilities.CalculateBarycentricCoord(
                        new Vector2(samplePoint.point.x, samplePoint.point.z),
                        new Vector2(args.WorldPointCorners[0].x, args.WorldPointCorners[0].z),
                        new Vector2(args.WorldPointCorners[1].x, args.WorldPointCorners[1].z),
                        new Vector2(args.WorldPointCorners[2].x, args.WorldPointCorners[2].z),
                        ref bary0);
                    GUtilities.CalculateBarycentricCoord(
                        new Vector2(samplePoint.point.x, samplePoint.point.z),
                        new Vector2(args.WorldPointCorners[0].x, args.WorldPointCorners[0].z),
                        new Vector2(args.WorldPointCorners[2].x, args.WorldPointCorners[2].z),
                        new Vector2(args.WorldPointCorners[3].x, args.WorldPointCorners[3].z),
                        ref bary1);
                    if (bary0.x >= 0 && bary0.y >= 0 && bary0.z >= 0)
                    {
                        maskUv = bary0.x * Vector2.zero + bary0.y * Vector2.up + bary0.z * Vector2.one;
                    }
                    else if (bary1.x >= 0 && bary1.y >= 0 && bary1.z >= 0)
                    {
                        maskUv = bary1.x * Vector2.zero + bary1.y * Vector2.one + bary1.z * Vector2.right;
                    }
                    else
                    {
                        continue;
                    }

                    //sample mask
                    if (clonedMask != null)
                    {
                        maskColor = clonedMask.GetPixelBilinear(maskUv.x, maskUv.y);
                        if (Random.value > maskColor.grayscale)
                            continue;
                    }

                    //apply filter
                    GSpawnFilterArgs filterArgs = GSpawnFilterArgs.Create();
                    filterArgs.Terrain = terrain;
                    filterArgs.Position = samplePoint.point;
                    filterArgs.SurfaceNormal = samplePoint.normal;
                    filterArgs.SurfaceTexcoord = samplePoint.textureCoord;

                    List<Type> suitableFilter = SuitableFilterTypes;
                    if (args.Filters != null)
                    {
                        for (int fIndex = 0; fIndex < args.Filters.Length; ++fIndex)
                        {
                            if (args.Filters[fIndex] != null &&
                                args.Filters[fIndex].Ignore != true)
                            {
                                if (suitableFilter.Contains(args.Filters[fIndex].GetType()))
                                    args.Filters[fIndex].Apply(ref filterArgs);
                            }
                            if (filterArgs.ShouldExclude)
                                break;
                        }
                    }

                    //spawn
                    if (filterArgs.ShouldExclude)
                        continue;

                    //spawn here
                    GameObject instance = GSpawner.Spawn(terrain, g, samplePoint.point);
                    instance.transform.position = filterArgs.Position;
                    instance.transform.rotation = filterArgs.Rotation;
                    instance.transform.localScale = filterArgs.Scale;
                }
            }

            if (clonedMask != null)
                Object.DestroyImmediate(clonedMask);
        }

        private void HandleEraseObject(GStylizedTerrain terrain, GObjectPainterArgs args)
        {
            int prototypeIndex = -1;
            Vector3 localPos = Vector3.zero;
            Vector3 worldPos = Vector3.zero;
            Vector3 bary0 = Vector3.zero;
            Vector3 bary1 = Vector3.zero;
            Vector2 maskUv = Vector2.zero;
            Color maskColor = Color.white;
            Texture2D clonedMask = null;
            if (args.Mask != null)
            {
                clonedMask = GCommon.CloneAndResizeTexture(args.Mask, 256, 256);
            }
            for (int i = 0; i < args.PrototypeIndices.Count; ++i)
            {
                prototypeIndex = args.PrototypeIndices[i];
                if (prototypeIndex < 0 || prototypeIndex >= args.Prototypes.Count)
                    continue;
                GameObject g = args.Prototypes[prototypeIndex];
                if (g == null)
                    continue;
                GSpawner.DestroyIf(terrain, g, (instance =>
                {
                    worldPos = instance.transform.position;
                    GUtilities.CalculateBarycentricCoord(
                            new Vector2(worldPos.x, worldPos.z),
                            new Vector2(args.WorldPointCorners[0].x, args.WorldPointCorners[0].z),
                            new Vector2(args.WorldPointCorners[1].x, args.WorldPointCorners[1].z),
                            new Vector2(args.WorldPointCorners[2].x, args.WorldPointCorners[2].z),
                            ref bary0);
                    GUtilities.CalculateBarycentricCoord(
                        new Vector2(worldPos.x, worldPos.z),
                        new Vector2(args.WorldPointCorners[0].x, args.WorldPointCorners[0].z),
                        new Vector2(args.WorldPointCorners[2].x, args.WorldPointCorners[2].z),
                        new Vector2(args.WorldPointCorners[3].x, args.WorldPointCorners[3].z),
                        ref bary1);
                    if (bary0.x >= 0 && bary0.y >= 0 && bary0.z >= 0)
                    {
                        maskUv = bary0.x * Vector2.zero + bary0.y * Vector2.up + bary0.z * Vector2.one;
                    }
                    else if (bary1.x >= 0 && bary1.y >= 0 && bary1.z >= 0)
                    {
                        maskUv = bary1.x * Vector2.zero + bary1.y * Vector2.one + bary1.z * Vector2.right;
                    }
                    else
                    {
                        return false;
                    }

                    //sample mask
                    if (clonedMask != null)
                    {
                        maskColor = clonedMask.GetPixelBilinear(maskUv.x, maskUv.y);
                        if (Random.value > maskColor.grayscale * args.EraseRatio)
                            return false;
                    }

                    return true;
                }));
            }

            if (clonedMask != null)
                Object.DestroyImmediate(clonedMask);
        }
    }
}
