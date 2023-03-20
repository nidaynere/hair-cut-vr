using HairTools.InputDevices;
using UnityEngine;

namespace HairTools.Functions {
    public class PatFunction : IHairFunction {
        private const string PAT_SHADER_PROPERTY = "_PatInput";
        private const string RADIUS_SHADER_PROPERTY = "_RadiusInput";
        private const string PAT_FORCE_SHADER_PROPERTY = "_PatForce";

        private readonly DeviceInput deviceInput;
        private readonly HairRaycaster hairRaycaster;
        private readonly HairInput hairInput;

        public PatFunction() {
            hairInput = Object.FindObjectOfType<HairInput>();
            deviceInput = Object.FindObjectOfType<DeviceInput>();
            hairRaycaster = new HairRaycaster();
        }

        public void Trigger() {
            if (!deviceInput.IsPressed()) {
                return;
            }

            var ray = deviceInput.GetRay();

            if (!hairRaycaster.TryHit(ray,
                hairInput.brushSize, out var point)) {
                return;
            }

            Pat (point);
        }

        public void Pat(Vector3 point) {
            Shader.SetGlobalVector(PAT_SHADER_PROPERTY, point);
            Shader.SetGlobalFloat(RADIUS_SHADER_PROPERTY, hairInput.brushSize);
            Shader.SetGlobalFloat(PAT_FORCE_SHADER_PROPERTY, hairInput.patForce);
        }
    }
}
