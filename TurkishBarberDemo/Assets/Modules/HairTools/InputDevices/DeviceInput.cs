using UnityEngine;

namespace HairTools.InputDevices {
    public abstract class DeviceInput : MonoBehaviour {
        public abstract bool IsSelectPressed ();
        public abstract float TriggerValue();
        public abstract Ray GetRay();
    }
}
