﻿
using HairTools.Functions;
using UnityEngine;

namespace HairTools {

    [DisallowMultipleComponent]
    public class HairInput : MonoBehaviour {
        protected IHairFunction hairFunction;

        public float brushSize = 2;
        public Color color;
        public float size = 1;
        public float sprayForce = 7;
        public float patForce = 12;

        private void Start() {
            hairFunction = new ColorSprayFunction();
        }

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
