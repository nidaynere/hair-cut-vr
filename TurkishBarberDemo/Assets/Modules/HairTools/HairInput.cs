
using HairTools.Functions;
using UnityEngine;

namespace FStudio.HairTools {

    [DisallowMultipleComponent]
    public class HairInput : MonoBehaviour {
        protected IHairFunction hairFunction;

        public float brushSize = 2;
        public Color color;
        public float size = 1;
        public float sprayForce = 7;

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
