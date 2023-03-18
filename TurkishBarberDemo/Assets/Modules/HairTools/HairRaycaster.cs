using UnityEngine;

namespace HairTools {
    [RequireComponent(typeof (HairBaker))]
    [RequireComponent(typeof (HairRenderer))]
    public class HairRaycaster : MonoBehaviour {
        [HideInInspector]
        [SerializeField]
        private HairBaker hairBaker;

        [HideInInspector]
        [SerializeField]
        private HairRenderer hairRenderer;

        private readonly RaycastHit[] allocatedHits = new RaycastHit[256];

        private void OnValidate() {
            hairBaker       = GetComponent<HairBaker>();
            hairRenderer    = GetComponent<HairRenderer>();
        }

        public int RaycastNonAlloc (Vector3 origin, Vector3 direction, float brushSize, ref int[] allocatedVertexIndexes) {
            var layer = hairBaker.SurfaceLayer;

            var hitCount = Physics.SphereCastNonAlloc(
                new Ray(origin, direction), 
                brushSize, 
                allocatedHits, 
                Mathf.Infinity, 1 << layer);

            var index = 0;
            for (var i=0; i<hitCount; i++) {
                var triangleIndex = allocatedHits[i].triangleIndex;
                allocatedVertexIndexes[index++] = triangleIndex * 3 + 0;
                allocatedVertexIndexes[index++] = triangleIndex * 3 + 1;
                allocatedVertexIndexes[index++] = triangleIndex * 3 + 2;
            }

            return index;
        }
    }
}
