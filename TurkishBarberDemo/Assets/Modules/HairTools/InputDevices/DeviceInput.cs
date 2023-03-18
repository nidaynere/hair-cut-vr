

using UnityEngine;

namespace HairTools.InputDevices {
    public abstract class DeviceInput : MonoBehaviour {
        public abstract bool IsPressed();
        public abstract Vector3 GetPosition();
        public abstract Vector3 GetDirection();
    }
}
