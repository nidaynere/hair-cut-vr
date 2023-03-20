using HairTools.InputDevices;
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

        public PatFunction(float brushSize, float patForce) {
            deviceInputs = Object.FindObjectsOfType<DeviceInput>();
            hairRaycaster = new HairRaycaster();

            this.patForce = patForce;
            this.brushSize = brushSize;
        }

        public void Trigger() {
            foreach (var deviceInput in deviceInputs) {
                var triggerValue = deviceInput.TriggerValue();

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
            Shader.SetGlobalFloat(RADIUS_SHADER_PROPERTY, brushSize);
            Shader.SetGlobalFloat(PAT_FORCE_SHADER_PROPERTY, patForce);
        }
    }
}
