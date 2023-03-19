using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

namespace HairTools {
    public class HairBaker : MonoBehaviour {
        [SerializeField] private MeshFilter hairSurfaceMeshFilter;
        [SerializeField] private float minVertexColorWeight = 0.3f;
        [SerializeField] private Gradient defaultHairColorGradient;

        [SerializeField]
        [HideInInspector]
        private HairBakedData[] hairBakeData;
        public int BakedCount => hairBakeData == null ? 0 : hairBakeData.Length;

        public HairBakedData[] BakedData => hairBakeData;
        public NativeArray<HairBakedData> NativeBakedData { private set; get; }

        public LayerMask SurfaceLayer => hairSurfaceMeshFilter.gameObject.layer;

        private void Start() {
            NativeBakedData = new NativeArray<HairBakedData> (BakedData, Allocator.Persistent);
        }

        private void OnDestroy() {
            NativeBakedData.Dispose();
        }

#if UNITY_EDITOR
        public void Bake() {
            var hairSurfaceMeshTransform = hairSurfaceMeshFilter.transform;
            var hairSurfaceMesh = hairSurfaceMeshFilter.sharedMesh;
            var surfaceVertexCount = hairSurfaceMesh.vertexCount;

            var surfacePosition = hairSurfaceMeshTransform.position;

            hairBakeData = new HairBakedData[surfaceVertexCount];

            var colors = new List<Color>();
            hairSurfaceMesh.GetColors(colors);

            int count = 0;

            for (var i = 0; i < surfaceVertexCount; i++) {
                EditorUtility.DisplayProgressBar("Baking", $"{i + 1}/{surfaceVertexCount}", (float)i / surfaceVertexCount);

                var weight = colors[i].maxColorComponent;
                if (weight < minVertexColorWeight) {
                    continue;
                }

                var localPoint = hairSurfaceMesh.vertices[i];
                var globalPoint = hairSurfaceMeshTransform.TransformPoint(localPoint);

                var bakeData = new HairBakedData();

                var positionItem = globalPoint;
                var rotationItem = Quaternion.LookRotation(globalPoint - surfacePosition);

                var scaleItem = Vector3.one * weight;

                bakeData.position = positionItem;
                bakeData.rotation = rotationItem;
                bakeData.scale = scaleItem;

                var randomColor = defaultHairColorGradient.Evaluate(Random.Range(0f, 1f));
                bakeData.color = randomColor;

                hairBakeData[count++] = bakeData;
            }

            hairBakeData = hairBakeData.Take(count).ToArray();

            EditorUtility.ClearProgressBar();
        }
#endif
    }
}
