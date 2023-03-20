
using HairTools;
using HairTools.InputDevices;
using HairTools.Jobs;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace HairTools.Functions {
    public abstract class AbstractSprayFunction : IHairFunction {
        protected readonly HairBaker hairBaker;
        protected readonly HairRenderer hairRenderer;
        protected readonly DeviceInput[] deviceInputs;
        protected readonly HairRaycaster hairRaycaster;
        protected readonly PatFunction patFunction;

        private readonly float brushSize;

        public AbstractSprayFunction (float brushSize, float patPower) {
            hairRenderer = Object.FindObjectOfType<HairRenderer>();
            hairBaker = Object.FindObjectOfType<HairBaker>();
            deviceInputs = Object.FindObjectsOfType<DeviceInput>();
            hairRaycaster = new HairRaycaster();
            patFunction = new PatFunction(brushSize, patPower);
            this.brushSize = brushSize;
        }

        public void Trigger() {
            foreach (var deviceInput in deviceInputs) {
                var triggerValue = deviceInput.TriggerValue();

                if (triggerValue == 0f) {
                    patFunction.Pat(Vector3.zero);
                    continue;
                }

                var ray = deviceInput.GetRay();

                if (!hairRaycaster.TryHit(ray,
                    brushSize, out var point)) {
                    return;
                }

                patFunction.Pat(point);

                var queryLength = hairBaker.NativeBakedData.Length;

                var results = new NativeArray<bool>(queryLength, Allocator.TempJob);

                var raycastJob = new HairRootDistanceJob(point, brushSize, results, hairBaker.NativeBakedData);
                var jobHandle = raycastJob.Schedule(queryLength, 1);
                jobHandle.Complete();

                OnSpray(triggerValue, raycastJob.results);

                results.Dispose();
            }
        }

        protected abstract void OnSpray(float value01, [ReadOnly] in NativeArray<bool> patOrNotIndexes);
    }
}
