
using UnityEngine;

namespace HairTools.InputDevices {
    public class StandaloneDeviceInput : DeviceInput {
        public override bool IsPressed() {
            return false;
        }

        public override Vector3 GetDirection() {
            throw new System.NotImplementedException();
        }

        public override Vector3 GetPosition() {
            throw new System.NotImplementedException();
        }
    }
}
