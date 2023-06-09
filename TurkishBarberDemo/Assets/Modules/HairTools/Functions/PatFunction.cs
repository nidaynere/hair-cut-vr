﻿using HairTools.InputDevices;
using System;
using UnityEngine;

namespace HairTools.Functions {
    public class PatFunction : IHairFunction {
        private const string PAT_SHADER_PROPERTY = "_PatInput";
        private const string RADIUS_SHADER_PROPERTY = "_RadiusInput";
        private const string PAT_FORCE_SHADER_PROPERTY = "_PatForce";

        private readonly DeviceInput[] deviceInputs;
        private readonly HairRaycaster hairRaycaster;

        private readonly float brushSize;
        private readonly float patForce;

        private event Action<float> onUse;

        public PatFunction(float brushSize, float patForce, Action<float> onUse = null) {
            deviceInputs = UnityEngine.Object.FindObjectsOfType<DeviceInput>();
            hairRaycaster = new HairRaycaster();

            this.patForce = patForce;
            this.brushSize = brushSize;

            this.onUse = onUse;
        }

        public void Trigger() {
            Shader.SetGlobalVector(PAT_SHADER_PROPERTY, Vector3.up * -1000);

            foreach (var deviceInput in deviceInputs) {
                var triggerValue = deviceInput.TriggerValue();

                onUse?.Invoke(triggerValue);

                if (triggerValue == 0f) {
                    continue;
                }

                var ray = deviceInput.GetRay();

                if (!hairRaycaster.TryHit(ray, brushSize, out var point)) {
                    return;
                }

                Pat(point);
            }
        }

        public void Pat(Vector3 point) {
            Shader.SetGlobalVector(PAT_SHADER_PROPERTY, point);
            Shader.SetGlobalFloat(RADIUS_SHADER_PROPERTY, brushSize * 2);
            Shader.SetGlobalFloat(PAT_FORCE_SHADER_PROPERTY, patForce);
        }
    }
}
