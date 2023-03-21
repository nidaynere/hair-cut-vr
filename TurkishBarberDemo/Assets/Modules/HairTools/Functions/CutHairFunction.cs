using System;
using Unity.Collections;
using UnityEngine;

namespace HairTools.Functions {
    public class CutHairFunction : AbstractSprayFunction {
        private const float CUT_HAIR_COLOR_LERP_SPEED = 0.5f;
        private static readonly Color CUT_HAIR_TARGET_COLOR = new Color(0,0,0,0);

        public CutHairFunction(float brushSize, float patPower, Action<float> onUse) : base(brushSize, patPower, onUse) {

        }

        protected override void OnSpray(float value01, [ReadOnly] in NativeArray<bool> patOrNotIndexes) {
            var dT = Time.deltaTime;

            var queryLength = patOrNotIndexes.Length;

            var lerpSpeed = dT * CUT_HAIR_COLOR_LERP_SPEED * value01;

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
