using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;
using System;

namespace HairTools.Jobs {
    [BurstCompile]
    public struct HairRootDistanceJob : IJobParallelFor {
        private const int GRID_SIZE = 8;

        [ReadOnly] private NativeArray<HairBakedData> bakedData;
        [ReadOnly] private Vector3 point;
        [ReadOnly] private float brushSizeSq;
        [ReadOnly] private float brushSize;

        public NativeArray<bool> results;

        [ReadOnly] private readonly int uPoint;

        public HairRootDistanceJob(Vector3 point, float brushSize, NativeArray<bool> results, [ReadOnly] NativeArray<HairBakedData> bakedData) {
            this.bakedData = bakedData;
            this.point = point;
            this.results = results;
            this.brushSize = brushSize;

            brushSizeSq = brushSize * brushSize;

            this.uPoint = GetUniqueKeyForPosition(point);
        }

        public void Execute(int index) {
            var instancePosition = bakedData[index].position;
            if (!ContainsUniqueKey(instancePosition, brushSize, in uPoint)) {
                return;
            }

            var lengthsq = math.lengthsq(point - bakedData[index].position);
            results[index] = lengthsq < brushSizeSq;
        }

        [BurstCompile]
        private static int GetUniqueKeyForPosition(in float3 position) {
            return (int)(GRID_SIZE * math.floor(position.x / GRID_SIZE) + GRID_SIZE * math.floor(position.z / GRID_SIZE));
        }

        [BurstCompile]
        private static bool ContainsUniqueKey(in float3 position, in float radius, in int uniquePosition) {
            var add = (int)radius / GRID_SIZE;
            var range = 3 + add * 2;

            var uniquePoint = GetUniqueKeyForPosition(position);

            var blocks = FindBlocksAround(uniquePoint, range);

            return DoesBlockContains(in blocks, in uniquePosition);
        }

        [BurstCompile]
        private static bool DoesBlockContains(in Int32 blocks, in int uniquePosition) {
            return blocks == (blocks | 1 << uniquePosition);
        }

        [BurstCompile]
        private static Int32 FindBlocksAround(int origin, int range) {

            Int32 result = default;

            for (var x = -range / 2; x <= range / 2; x++) {
                for (var y = -range / 2; y <= range / 2; y++) {
                    var xIndex = (origin + x) % GRID_SIZE;
                    var yIndex = origin / GRID_SIZE + y;

                    var final = xIndex + yIndex * GRID_SIZE;

                    result |= 1 << final;
                }
            }

            return result;
        }
    }
}
