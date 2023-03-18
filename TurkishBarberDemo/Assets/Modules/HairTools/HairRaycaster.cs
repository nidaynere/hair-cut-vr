using UnityEngine;

namespace HairTools {
    public class HairRaycaster {
        private readonly HairBaker hairBaker;
        private readonly HairRenderer hairRenderer;

        private readonly RaycastHit[] allocatedHits = new RaycastHit[256];

        public HairRaycaster () {
            hairBaker       = Object.FindObjectOfType<HairBaker>();
            hairRenderer    = Object.FindObjectOfType<HairRenderer>();
        }

        public int RaycastNonAlloc (Vector3 origin, Vector3 direction, float brushSize, int[] allocatedVertexIndexes) {
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
