
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;

namespace HairTools.Jobs {
    [BurstCompile]
    public struct RaycastJob : IJobParallelFor {
        [ReadOnly] private NativeArray<HairBakedData> bakedData;
        [ReadOnly] private Vector3 point;
        [ReadOnly] private float brushSizeSq;

        public NativeArray<bool> results;

        public RaycastJob(Vector3 point, float brushSize, NativeArray<bool> results, [ReadOnly] NativeArray<HairBakedData> bakedData) {
            this.bakedData = bakedData;
            this.point = point;
            this.results = results;

            brushSizeSq = brushSize * brushSize;
        }

        public void Execute(int index) {
            var lengthsq = math.lengthsq(point - bakedData[index].position);
            results[index] = lengthsq < brushSizeSq;
        }
    }
}
