using HairTools.Functions;
using UnityEngine;

namespace HairTools.Items {
    internal class ColorSpray : AbstractItem {
        [SerializeField] private float brushSize = 1, patForce = 12;
        [SerializeField] private Color sprayColor;
        [SerializeField] private float sprayPower;

        private ColorSprayFunction cutHairFunction;

        [SerializeField] private new ParticleSystem particleSystem;

        protected override IHairFunction hairFunction => cutHairFunction;

        private void Awake() {
            cutHairFunction = new ColorSprayFunction (brushSize, patForce, sprayColor, sprayPower, OnUse);
        }

        private void OnUse (float value01) {
            var main = particleSystem.main;
            var startColor = main.startColor;
            startColor.color = new Color(sprayColor.r, sprayColor.g, sprayColor.b, value01);
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