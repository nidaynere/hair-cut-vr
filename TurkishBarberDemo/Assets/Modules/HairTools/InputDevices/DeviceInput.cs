using UnityEngine;

namespace HairTools.InputDevices {
    public abstract class DeviceInput : MonoBehaviour {
        public abstract bool IsPressed();
        public abstract Ray GetRay();
    }
}
