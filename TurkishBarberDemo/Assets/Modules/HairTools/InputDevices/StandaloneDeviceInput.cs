using UnityEngine;
using UnityEngine.InputSystem;

namespace HairTools.InputDevices {
    public class StandaloneDeviceInput : DeviceInput {
        [SerializeField] private InputActionReference mouseClickAction;
        [SerializeField] private InputActionReference mousePositionAction;
        [SerializeField] private InputActionReference selectAction;

        public override Ray GetRay() {
            var camera = Camera.main;

            var near = camera.nearClipPlane;

            var mousePosition = mousePositionAction.action.ReadValue<Vector2>();

            Vector3 actualPos = mousePosition;
            actualPos.z = near;

            var ray = Camera.main.ScreenPointToRay(actualPos);

            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1);

            return ray;
        }

        public override float TriggerValue() {
            return mouseClickAction.action.ReadValue<float>();
        }

        public override bool IsSelectPressed() {
            return selectAction.action.WasPressedThisFrame();
        }
    }
}
