using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pinwheel.Griffin.PaintTool
{
    //[CreateAssetMenu(menuName = "Griffin/Texture Painter Custom Params")]
    public class GTexturePainterCustomParams : ScriptableObject
    {
        private static GTexturePainterCustomParams instance;
        public static GTexturePainterCustomParams Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<GTexturePainterCustomParams>("TexturePainterCustomParams");
                    if (instance == null)
                    {
                        instance = ScriptableObject.CreateInstance<GTexturePainterCustomParams>();
                    }
                }
                return instance;
            }
        }

        [SerializeField]
        private GTerracePainterParams terrace;
        public GTerracePainterParams Terrace
        {
            get
            {
                return terrace;
            }
            set
            {
                terrace = value;
            }
        }

        [SerializeField]
        private GRemapPainterParams remap;
        public GRemapPainterParams Remap
        {
            get
            {
                return remap;
            }
            set
            {
                remap = value;
            }
        }

        [SerializeField]
        private GNoisePainterParams noise;
        public GNoisePainterParams Noise
        {
            get
            {
                return noise;
            }
            set
            {
                noise = value;
            }
        }
    }
}