using UnityEngine;
using UnityEngine.Rendering;

namespace HairTools {
    [RequireComponent(typeof(HairBaker))]
    public class HairRenderer : MonoBehaviour {
        [SerializeField] private Mesh hairBladeMesh;
        [SerializeField] private Material hairBladeMaterial;
        [SerializeField] private Vector2 hairSizeMinMax = new Vector2 (1, 1.25f);
        [SerializeField] private ShadowCastingMode ShadowCasting = ShadowCastingMode.Off;
        [SerializeField] private bool ReceiveShadows = true;

        private MaterialPropertyBlock materialBlockProperty;

        private Bounds bounds;
        private ComputeBuffer instancesBuffer;
        private ComputeBuffer argsBuffer;

        private HairInstanceData[] hairBladeInstances;

        public HairInstanceData[] HairInstances => hairBladeInstances;

        [SerializeField]
        [HideInInspector]
        private HairBaker hairBaker;

        private void OnValidate() {
            hairBaker = GetComponent<HairBaker>();
        }

        public void OnEnable() {
            materialBlockProperty = new MaterialPropertyBlock();
            bounds = new Bounds(Vector3.zero, new Vector3(100000, 100000, 100000));
            InitializeBuffers();
        }

        private ComputeBuffer GetArgsBuffer(uint count) {
            uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
            args[0] = (uint)hairBladeMesh.GetIndexCount(0);
            args[1] = (uint)count;
            args[2] = (uint)hairBladeMesh.GetIndexStart(0);
            args[3] = (uint)hairBladeMesh.GetBaseVertex(0);
            args[4] = 0;

            ComputeBuffer buffer = new ComputeBuffer(args.Length, sizeof(uint), ComputeBufferType.IndirectArguments);
            buffer.SetData(args);
            return buffer;
        }

        private void InitializeBuffers() {
            var hairBakeData = hairBaker.BakedData;

            var bakedLength = hairBakeData.Length;

            hairBladeInstances = new HairInstanceData[bakedLength];
            for (var i = 0; i < bakedLength; i++) {
                var instance = new HairInstanceData();
                instance.Matrix = Matrix4x4.TRS(hairBakeData[i].position, hairBakeData[i].rotation, hairBakeData[i].scale * Random.Range(hairSizeMinMax.x, hairSizeMinMax.y));
                instance.MatrixInverse = instance.Matrix.inverse;
                instance.Color = hairBakeData[i].color;
                hairBladeInstances[i] = instance;
            }

            argsBuffer = GetArgsBuffer((uint)bakedLength);
            instancesBuffer = new ComputeBuffer(bakedLength, HairInstanceData.Size());

            RefreshInstances();

            hairBladeMaterial.SetBuffer("_PerInstanceItemData", instancesBuffer);
        }

        private void FixedUpdate() {
            RefreshInstances();
        }

        public void RefreshInstances() {
            instancesBuffer.SetData(hairBladeInstances);
        }

        public void Update() {
            Graphics.DrawMeshInstancedIndirect(hairBladeMesh, 0, hairBladeMaterial, bounds, argsBuffer, 0, materialBlockProperty, ShadowCasting, ReceiveShadows);
        }

        private void OnDisable() {
            if (instancesBuffer != null) {
                instancesBuffer.Release();
                instancesBuffer = null;
            }

            if (argsBuffer != null) {
                argsBuffer.Release();
                argsBuffer = null;
            }
        }
    }
}
