using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
public class HairRenderer : MonoBehaviour
{
	[SerializeField] [ReadOnly] private MeshFilter hairSurfaceMeshFilter;

	[SerializeField] private Mesh hairBladeMesh;
    [SerializeField] private Material hairBladeMaterial;

	[SerializeField] private float hairSize = 1f;

	[SerializeField] private Gradient defaultHairColorGradient;

	[SerializeField] private bool useVertexNormalsForRotate;

    [SerializeField] private ShadowCastingMode ShadowCasting = ShadowCastingMode.Off;
	[SerializeField] private bool ReceiveShadows = true;

	private MaterialPropertyBlock MPB;

	private Bounds bounds;
	private ComputeBuffer instancesBuffer;
	private ComputeBuffer argsBuffer;

	private List<HairInstanceData> hairBladeInstances = new List<HairInstanceData>();

	private struct HairInstanceData
	{
		public Matrix4x4 Matrix;
		public Matrix4x4 MatrixInverse;
		public Color Color;

		public static int Size()
		{
			return sizeof(float) * 4 * 4
				+ sizeof(float) * 4 * 4
				+ sizeof(float) * 4;
		}
	}

	public void OnEnable()
	{
		MPB = new MaterialPropertyBlock();
		bounds = new Bounds(Vector3.zero, new Vector3(100000, 100000, 100000));
		InitializeBuffers();
	}

	private ComputeBuffer GetArgsBuffer(uint count)
	{
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
        hairBladeInstances.Clear();

		var hairSurfaceMeshTransform = hairSurfaceMeshFilter.transform;
		var hairSurfaceMesh = hairSurfaceMeshFilter.sharedMesh;
        var surfaceVertexCount = hairSurfaceMesh.vertexCount;

		var hairSurfacePosition = hairSurfaceMeshTransform.position;

        for (var i=0; i<surfaceVertexCount; i++) {
			var weight = hairSurfaceMesh.colors[i].maxColorComponent;
            if (weight < 0.5f) {
				continue;
			}

			var localPoint = hairSurfaceMesh.vertices[i];
			var globalPoint = hairSurfaceMeshTransform.TransformPoint(localPoint);

            var dataItem = new HairInstanceData();

			var positionItem = globalPoint;

			var rotationItem = Quaternion.LookRotation (globalPoint - hairSurfacePosition);

            var scaleItem = Vector3.one * weight * hairSize;

            dataItem.Matrix = Matrix4x4.TRS(positionItem, rotationItem, scaleItem);
            dataItem.MatrixInverse = dataItem.Matrix.inverse;

			var randomColor = defaultHairColorGradient.Evaluate (Random.Range(0f, 1f));
            dataItem.Color = randomColor;
            hairBladeInstances.Add(dataItem);
        }

        argsBuffer = GetArgsBuffer((uint)hairBladeInstances.Count);
        instancesBuffer = new ComputeBuffer(hairBladeInstances.Count, HairInstanceData.Size());
        instancesBuffer.SetData(hairBladeInstances);
        hairBladeMaterial.SetBuffer("_PerInstanceItemData", instancesBuffer);
	}

	public void Update()
	{
        Graphics.DrawMeshInstancedIndirect(hairBladeMesh, 0, hairBladeMaterial, bounds, argsBuffer, 0, MPB, ShadowCasting, ReceiveShadows);
	}

	private void OnDisable()
	{
		if (instancesBuffer != null)
		{
			instancesBuffer.Release();
			instancesBuffer = null;
		}
		
		if (argsBuffer != null)
		{
			argsBuffer.Release();
			argsBuffer = null;
		}
	}
}