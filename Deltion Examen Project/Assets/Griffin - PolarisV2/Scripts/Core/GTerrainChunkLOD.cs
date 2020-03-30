using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pinwheel.Griffin
{
    [System.Serializable]
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class GTerrainChunkLOD : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter meshFilterComponent;
        public MeshFilter MeshFilterComponent
        {
            get
            {
                if (meshFilterComponent==null)
                {
                    meshFilterComponent = GetComponent<MeshFilter>();
                }
                return meshFilterComponent;
            }
        }

        [SerializeField]
        private MeshRenderer meshRendererComponent;
        public MeshRenderer MeshRendererComponent
        {
            get
            {
                if (meshRendererComponent == null)
                {
                    meshRendererComponent = GetComponent<MeshRenderer>();
                }
                return meshRendererComponent;
            }
        }
    }
}
