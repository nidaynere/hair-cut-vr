
using Unity.Collections;
using UnityEngine;

namespace HairTools.Functions {
    public class ColorSprayFunction : AbstractSprayFunction {
        private readonly HairVFX hairVFX;

        public ColorSprayFunction() : base() {
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
                color = Color.Lerp(color, hairInput.color, dT * hairInput.sprayForce);
                instance.Color = color;
                hairRenderer.HairInstances[i] = instance;

                hairVFX.PlaySprayVFX(instance.Matrix.GetPosition(), hairInput.color * color.a, 1);
            }
        }
    }
}
