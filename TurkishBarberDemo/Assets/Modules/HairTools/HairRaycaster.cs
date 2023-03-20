using UnityEngine;

namespace HairTools {
    public class HairRaycaster {
        private readonly HairBaker hairBaker;

        public HairRaycaster () {
            hairBaker       = Object.FindObjectOfType<HairBaker>();
        }

        public bool TryHit (Ray ray, float brushSize, out Vector3 point) {
            var layer = hairBaker.SurfaceLayer;

            Debug.DrawRay(ray.origin, ray.direction, Color.yellow, 1);

            if (Physics.Raycast (
                ray, out var hit, Mathf.Infinity, 1 << layer)) {
                point = hit.point;
                return true;
            }
            point = Vector3.zero;
            return false;
        }
    }
}
