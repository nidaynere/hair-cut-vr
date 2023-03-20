using HairTools.Functions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HairTools.InputDevices {
    public class VRDeviceInput : DeviceInput {
        [SerializeField] private InputActionReference triggerAction;
        [SerializeField] private InputActionReference selectAction;

        [SerializeField] private InputActionReference inputPositionAction;
        [SerializeField] private InputActionReference inputRotationAction;

        public override Ray GetRay() {
            var inputPosition = inputPositionAction.action.ReadValue<Vector3>();
            var inputDirection = inputRotationAction.action.ReadValue<Quaternion>();

            var ray = new Ray(inputPosition, inputDirection * Vector3.forward);

            return ray;
        }

        public override float TriggerValue() {
            return triggerAction.action.ReadValue<float>();
        }

        public override bool IsSelectPressed() {
            return selectAction.action.WasPressedThisFrame ();
        }
    }
}
