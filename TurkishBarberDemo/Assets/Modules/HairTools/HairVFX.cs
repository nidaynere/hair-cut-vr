using UnityEditor;
using UnityEngine;

namespace HairTools {
    public class HairVFX : MonoBehaviour {
        [SerializeField] private ParticleSystem cutVFX;
        [SerializeField] private ParticleSystem sprayVFX;

        private static readonly Vector3 offset = new Vector3(0, 0.05f, 0);

        public void PlayCutVFX (in Vector3 point, in Color color, int count) {
            cutVFX.transform.position = point + offset;

            var main = cutVFX.main;
            main.startColor = color;
            var emitter = cutVFX.emission;
            emitter.SetBurst(0, new ParticleSystem.Burst(0, count));
            cutVFX.Play();
        }

        public void PlaySprayVFX (in Vector3 point, in Color color, int count) {
            sprayVFX.transform.position = point + offset;

            var main = sprayVFX.main;
            main.startColor = color;
            var emitter = sprayVFX.emission;
            emitter.SetBurst(0, new ParticleSystem.Burst(0, count));
            sprayVFX.Play();
        }
    }
}