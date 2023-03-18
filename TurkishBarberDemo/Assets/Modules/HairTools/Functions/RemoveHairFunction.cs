
using FStudio.HairTools;
using HairTools.InputDevices;
using UnityEngine;

namespace HairTools.Functions {
    public class RemoveHairFunction : IHairFunction {
        private readonly HairBaker hairBaker;
        private readonly HairRenderer hairRenderer;
        private readonly DeviceInput deviceInput;
        private readonly HairRaycaster hairRaycaster;
        private readonly HairInput hairInput;

        private readonly int[] mAllocatedResults = new int[256];

        public RemoveHairFunction() {
            hairRenderer = Object.FindObjectOfType<HairRenderer>();
            hairBaker = Object.FindObjectOfType<HairBaker>();
            hairInput = Object.FindObjectOfType<HairInput>();
            deviceInput = Object.FindObjectOfType<DeviceInput>();
            hairRaycaster = new HairRaycaster();
        }

        public void Trigger() {
            if (!deviceInput.IsPressed()) {
                return;
            }

            var position = deviceInput.GetPosition();
            var direction = deviceInput.GetDirection();

            var count = hairRaycaster.RaycastNonAlloc(position, 
                direction, 
                hairInput.brushSize,
                mAllocatedResults);

            for (var i=0; i<count; i++) {
                if (!hairBaker.TryGetHairIndex(mAllocatedResults[i], out var hairIndex)) {
                    continue;
                }

                var instance = hairRenderer.HairInstances[hairIndex];
                var color = instance.Color;
                color.a = 0;
                instance.Color = color;
                hairRenderer.HairInstances[hairIndex] = instance;
            }
        }
    }
}
