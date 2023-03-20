using Unity.Collections;
using UnityEngine;

namespace HairTools.Functions {
    public class CutHairFunction : AbstractSprayFunction {

        private const float CUT_HAIR_COLOR_LERP_SPEED = 0.5f;
        private static readonly Color CUT_HAIR_TARGET_COLOR = new Color(0,0,0,0);

        private readonly HairVFX hairVFX;

        public CutHairFunction() : base() {
            hairVFX = Object.FindObjectOfType<HairVFX>();
        }

        protected override void OnPat([ReadOnly] in NativeArray<bool> patOrNotIndexes) {
            var dT = Time.deltaTime;

            var queryLength = patOrNotIndexes.Length;

            for (var i = 0; i < queryLength; i++) {
                if (!patOrNotIndexes[i]) {
                    continue;
                }
            
                var instance = hairRenderer.HairInstances[i];
                var color = instance.Color;
                color = Color.Lerp(color, CUT_HAIR_TARGET_COLOR, dT * CUT_HAIR_COLOR_LERP_SPEED);
                instance.Color = color;
                hairRenderer.HairInstances[i] = instance;

                hairVFX.PlayCutVFX(instance.Matrix.GetPosition(), color, 1);
            }
        }
    }
}
