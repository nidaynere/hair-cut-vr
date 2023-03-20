using HairTools.Functions;
using UnityEngine;

namespace HairTools.Items {
    internal class PatTool : AbstractItem {
        [SerializeField] private float brushSize = 1, patForce = 12;
        private PatFunction patFunction;

        protected override IHairFunction hairFunction => patFunction;

        private void Awake() {
            patFunction = new PatFunction (brushSize, patForce);
        }
    }
}