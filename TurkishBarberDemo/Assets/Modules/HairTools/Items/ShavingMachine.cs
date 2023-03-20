using HairTools.Functions;
using UnityEngine;

namespace HairTools.Items {
    internal class ShavingMachine : AbstractItem {
        [SerializeField] private float brushSize = 1, patForce = 12;
        private CutHairFunction cutHairFunction;

        protected override IHairFunction hairFunction => cutHairFunction;

        private void Awake() {
            cutHairFunction = new CutHairFunction(brushSize, patForce);
        }
    }
}