using System.Collections.Generic;
using System.Linq;
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

        private readonly Dictionary<int, int> hashMap = new Dictionary<int, int>();

        public int BakedCount => hairBakeData == null ? 0 : hairBakeData.Length;

        public HairBakedData[] BakedData => hairBakeData;

        public LayerMask SurfaceLayer => hairSurfaceMeshFilter.gameObject.layer;

        public bool TryGetHairIndex (int vertexId, out int hairIndex) {
            return hashMap.TryGetValue (vertexId, out hairIndex); 
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

                hashMap.Add(i, -1);

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

                hashMap[i] = count;

                hairBakeData[count++] = bakeData;
            }

            hairBakeData = hairBakeData.Take(count).ToArray();

            EditorUtility.ClearProgressBar();
        }
#endif
    }
}
