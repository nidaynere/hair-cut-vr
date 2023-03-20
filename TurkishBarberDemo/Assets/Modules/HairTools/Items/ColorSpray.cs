using HairTools.Functions;
using UnityEngine;

namespace HairTools.Items {
    internal class ColorSpray : AbstractItem {
        [SerializeField] private float brushSize = 1, patForce = 12;
        [SerializeField] private Color sprayColor;
        [SerializeField] private float sprayPower;

        private ColorSprayFunction cutHairFunction;

        protected override IHairFunction hairFunction => cutHairFunction;

        private void Awake() {
            cutHairFunction = new ColorSprayFunction (brushSize, patForce, sprayColor, sprayPower);
        }
    }
}