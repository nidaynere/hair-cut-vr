﻿using HairTools.Functions;
using UnityEngine;

namespace HairTools.Items {
    internal class PatTool : AbstractItem {
        [SerializeField] private float brushSize = 1, patForce = 12;
        private PatFunction patFunction;

        protected override IHairFunction hairFunction => patFunction;

        [SerializeField] private new ParticleSystem particleSystem;

        private void Awake() {
            patFunction = new PatFunction (brushSize, patForce, OnUse);
        }

        private void OnUse (float value01) {
            var main = particleSystem.main;
            var startColor = main.startColor;
            startColor.color = new Color (startColor.color.r, startColor.color.g, startColor.color.b, value01);
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