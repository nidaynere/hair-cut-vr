﻿using HairTools.Functions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HairTools.InputDevices {
    public class StandaloneDeviceInput : DeviceInput {
        [SerializeField] private InputActionReference mouseClickAction;
        [SerializeField] private InputActionReference mousePositionAction;
        [SerializeField] private InputActionReference cutHairFunctionAction;
        [SerializeField] private InputActionReference sprayHairFunctionAction;

        private HairInput hairInput;

        private void Start() {
            hairInput = Object.FindObjectOfType<HairInput>();
        }

        private void Update() {
            if (cutHairFunctionAction.action.ReadValue<float>() == 1f) {
                hairInput.SetFunction(new CutHairFunction());
            }

            if (sprayHairFunctionAction.action.ReadValue<float>() == 1f) {
                hairInput.SetFunction(new ColorSprayFunction());
            }
        }

        public override bool IsPressed() {
            return mouseClickAction.action.ReadValue<float>() == 1f;
        }

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
    }
}
