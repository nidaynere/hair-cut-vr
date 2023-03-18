
using UnityEditor;
using UnityEngine;

namespace HairTools {
    [CustomEditor(typeof(HairBaker))]
    [CanEditMultipleObjects]
    internal class HairBakerEditor : Editor {
        HairBaker hairBaker;

        private void OnEnable() {
            hairBaker = (HairBaker)target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (GUILayout.Button("Bake")) {
                hairBaker.Bake();
            }

            GUILayout.Label("Baked hair root count: " + hairBaker.BakedCount);
        }
    }
}
