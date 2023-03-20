
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
        protected readonly DeviceInput deviceInput;
        protected readonly HairRaycaster hairRaycaster;
        protected readonly HairInput hairInput;
        protected readonly PatFunction patFunction;

        public AbstractSprayFunction () {
            hairRenderer = Object.FindObjectOfType<HairRenderer>();
            hairBaker = Object.FindObjectOfType<HairBaker>();
            hairInput = Object.FindObjectOfType<HairInput>();
            deviceInput = Object.FindObjectOfType<DeviceInput>();
            hairRaycaster = new HairRaycaster();
            patFunction = new PatFunction();
        }

        public void Trigger() {
            if (!deviceInput.IsPressed()) {
                patFunction.Pat(Vector3.zero);
                return;
            }

            var ray = deviceInput.GetRay();

            if (!hairRaycaster.TryHit(ray,
                hairInput.brushSize, out var point)) {
                return;
            }

            patFunction.Pat(point);

            var queryLength = hairBaker.NativeBakedData.Length;

            var results = new NativeArray<bool>(queryLength, Allocator.TempJob);

            var raycastJob = new RaycastJob(point, hairInput.brushSize, results, hairBaker.NativeBakedData);
            var jobHandle = raycastJob.Schedule(queryLength, 1);
            jobHandle.Complete();

            OnPat(raycastJob.results);

            results.Dispose();
        }

        protected abstract void OnPat([ReadOnly] in NativeArray<bool> patOrNotIndexes);
    }
}
