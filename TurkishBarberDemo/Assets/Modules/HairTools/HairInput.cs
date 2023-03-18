
using HairTools.Functions;
using UnityEngine;

namespace FStudio.HairTools {

    [DisallowMultipleComponent]
    public class HairInput : MonoBehaviour {
        protected IHairFunction hairFunction;

        public float brushSize = 2;

        public void SetFunction (IHairFunction hairFunction) {
            this.hairFunction = hairFunction; 
        }

        private void Update() {
            if (hairFunction == null)
                return;

            hairFunction.Trigger();
        }
    }
}
