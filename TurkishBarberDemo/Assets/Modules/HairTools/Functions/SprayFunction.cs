
using FStudio.HairTools;
using HairTools.InputDevices;
using HairTools.Jobs;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace HairTools.Functions {
    public class SprayFunction : IHairFunction {
        private readonly HairBaker hairBaker;
        private readonly HairRenderer hairRenderer;
        private readonly DeviceInput deviceInput;
        private readonly HairRaycaster hairRaycaster;
        private readonly HairInput hairInput;
        private readonly PatFunction patFunction;

        public SprayFunction () {
            hairRenderer = Object.FindObjectOfType<HairRenderer>();
            hairBaker = Object.FindObjectOfType<HairBaker>();
            hairInput = Object.FindObjectOfType<HairInput>();
            deviceInput = Object.FindObjectOfType<DeviceInput>();
            hairRaycaster = new HairRaycaster();
            patFunction = new PatFunction();
        }

        public void Trigger() {
            if (!deviceInput.IsPressed()) {
                patFunction.Pat(Vector3.zero, 1);
                return;
            }

            var ray = deviceInput.GetRay();

            if (!hairRaycaster.TryHit(ray,
                hairInput.brushSize, out var point)) {
                return;
            }

            patFunction.Pat(point, hairInput.brushSize);

            var queryLength = hairBaker.NativeBakedData.Length;

            var results = new NativeArray<bool>(queryLength, Allocator.TempJob);

            var raycastJob = new RaycastJob(point, hairInput.brushSize, results, hairBaker.NativeBakedData);
            var jobHandle = raycastJob.Schedule(queryLength, 1);
            jobHandle.Complete();

            float dT = Time.deltaTime;

            var targetHairColor = hairInput.color;
            var sprayForce = hairInput.sprayForce;

            for (var i = 0; i < queryLength; i++) {
                if (!raycastJob.results[i]) {
                    continue;
                }

                var instance = hairRenderer.HairInstances[i];
                var color = instance.Color;
                color = Color.Lerp(color, targetHairColor, dT * sprayForce);
                instance.Color = color;
                hairRenderer.HairInstances[i] = instance;
            }

            results.Dispose();
        }
    }
}
