
using System;
using Unity.Collections;
using UnityEngine;

namespace HairTools.Functions {
    public class ColorSprayFunction : AbstractSprayFunction {
        private readonly HairVFX hairVFX;

        private readonly Color sprayColor;
        private readonly float sprayPower;

        public ColorSprayFunction(float brushSize, float patPower, Color sprayColor, float sprayPower, Action<float> onUse = null) : base(brushSize, patPower, onUse) {
            hairVFX = UnityEngine.Object.FindObjectOfType<HairVFX>();

            this.sprayPower = sprayPower;
            this.sprayColor = sprayColor;
        }

        protected override void OnSpray(float value01, [ReadOnly] in NativeArray<bool> patOrNotIndexes) {
            var dT = Time.deltaTime;

            var queryLength = patOrNotIndexes.Length;

            var lerpSpeed = dT * sprayPower * value01;

            for (var i = 0; i < queryLength; i++) {
                if (!patOrNotIndexes[i]) {
                    continue;
                }

                var instance = hairRenderer.HairInstances[i];
                var color = instance.Color;
                color = Color.Lerp(color, sprayColor, lerpSpeed);
                instance.Color = color;
                hairRenderer.HairInstances[i] = instance;

                hairVFX.PlaySprayVFX(instance.Matrix.GetPosition(), sprayColor * color.a * value01, 1);
            }
        }
    }
}
