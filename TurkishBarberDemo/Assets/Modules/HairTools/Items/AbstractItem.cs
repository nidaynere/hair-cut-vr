using HairTools.Functions;
using HairTools.InputDevices;
using UnityEngine;

namespace HairTools.Items {
    public abstract class AbstractItem : MonoBehaviour {
        protected abstract IHairFunction hairFunction { get; }

        private DeviceInput[] deviceInputs;

        [SerializeField] private Vector3 orientationUpVector = new Vector3 (0, 1, 0);

        private void Start() {
            deviceInputs = FindObjectsOfType<DeviceInput>();
        }

        private void Update() {
            hairFunction.Trigger();
        }

        private void LateUpdate() {
            foreach (var deviceInput in deviceInputs) {
                var ray = deviceInput.GetRay();

                transform.position = ray.origin;
                transform.rotation = deviceInput.GetRotation();
            }
        }
    }
}