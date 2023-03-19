using FStudio.HairTools;
using HairTools.InputDevices;
using UnityEngine;

namespace HairTools.Functions {
    public class PatFunction : IHairFunction {
        private const string PAT_SHADER_PROPERTY = "_PatInput";
        private const string RADIUS_SHADER_PROPERTY = "_RadiusInput";

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

            Pat(point, hairInput.brushSize);
        }

        public void Pat(Vector3 point, float brushSize) {
            Shader.SetGlobalVector(PAT_SHADER_PROPERTY, point);
            Shader.SetGlobalFloat(RADIUS_SHADER_PROPERTY, brushSize);
        }
    }
}
