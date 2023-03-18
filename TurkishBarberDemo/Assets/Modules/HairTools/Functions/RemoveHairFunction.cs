
using FStudio.HairTools;
using HairTools.InputDevices;
using HairTools.Jobs;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace HairTools.Functions {
    public class RemoveHairFunction : IHairFunction {
        private readonly HairBaker hairBaker;
        private readonly HairRenderer hairRenderer;
        private readonly DeviceInput deviceInput;
        private readonly HairRaycaster hairRaycaster;
        private readonly HairInput hairInput;

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

            var ray = deviceInput.GetRay();

            if (!hairRaycaster.TryHit (ray,
                hairInput.brushSize,out var point)) {
                return;
            }

            var queryLength = hairBaker.NativeBakedData.Length;

            var results = new NativeArray<bool>(queryLength, Allocator.TempJob);

            var raycastJob = new RaycastJob(point, hairInput.brushSize, results, hairBaker.NativeBakedData);
            var jobHandle = raycastJob.Schedule(queryLength, 1);
            jobHandle.Complete();

            float dT = Time.deltaTime;

            for (var i=0; i<queryLength; i++) {
                if (!raycastJob.results[i]) {
                    continue;
                }

                var instance = hairRenderer.HairInstances[i];
                var color = instance.Color;
                color.a -= dT;
                instance.Color = color;
                hairRenderer.HairInstances[i] = instance;
            }

            results.Dispose();
        }
    }
}
