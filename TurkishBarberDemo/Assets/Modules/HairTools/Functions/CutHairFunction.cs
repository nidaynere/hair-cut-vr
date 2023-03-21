using System;
using Unity.Collections;
using UnityEngine;

namespace HairTools.Functions {
    public class CutHairFunction : AbstractSprayFunction {
        private static readonly Color CUT_HAIR_TARGET_COLOR = new Color(0,0,0,0);

        private readonly float cutForce;

        public CutHairFunction(float brushSize, float patPower, float cutForce, Action<float> onUse) : base(brushSize, patPower, onUse) {
            this.cutForce = cutForce;
        }

        protected override void OnSpray(float value01, [ReadOnly] in NativeArray<bool> patOrNotIndexes) {
            var dT = Time.deltaTime;

            var queryLength = patOrNotIndexes.Length;

            var lerpSpeed = dT * cutForce * value01;

            for (var i = 0; i < queryLength; i++) {
                if (!patOrNotIndexes[i]) {
                    continue;
                }
            
                var instance = hairRenderer.HairInstances[i];
                var color = instance.Color;
                color = Color.Lerp(color, CUT_HAIR_TARGET_COLOR, lerpSpeed);
                instance.Color = color;
                hairRenderer.HairInstances[i] = instance;
            }
        }
    }
}
