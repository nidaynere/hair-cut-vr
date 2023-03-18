
using UnityEngine;

namespace HairTools {
    public struct HairInstanceData {
        public Matrix4x4 Matrix;
        public Matrix4x4 MatrixInverse;
        public Color Color;

        public static int Size() {
            return sizeof(float) * 4 * 4
                 + sizeof(float) * 4 * 4
                 + sizeof(float) * 4;
        }
    }
}
