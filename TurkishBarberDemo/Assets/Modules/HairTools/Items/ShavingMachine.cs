using HairTools.Functions;
using UnityEngine;

namespace HairTools.Items {
    internal class ShavingMachine : AbstractItem {
        [SerializeField] private float brushSize = 1, patForce = 12, cutForce = 0.5f;
        private CutHairFunction cutHairFunction;

        protected override IHairFunction hairFunction => cutHairFunction;

        [SerializeField] private new ParticleSystem particleSystem;

        private void Awake() {
            cutHairFunction = new CutHairFunction(brushSize, patForce, cutForce, OnUse);
        }

        private void OnUse (float value01) {
            var main = particleSystem.main;
            var startColor = main.startColor;
            startColor.color = new Color(startColor.color.r, startColor.color.g, startColor.color.b, value01);
            main.startColor = startColor;

            if (value01 > 0.2f) {
                if (!particleSystem.isPlaying) {
                    particleSystem.Play();
                }
            } else {
                if (particleSystem.isPlaying) {
                    particleSystem.Stop();
                }
            }
        }
    }
}