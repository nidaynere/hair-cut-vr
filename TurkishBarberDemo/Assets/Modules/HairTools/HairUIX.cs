using FStudio.HairTools;
using HairTools.Functions;
using UnityEngine;

namespace HairTools {
    [RequireComponent(typeof (HairInput))]
    public class HairUIX : MonoBehaviour {

        [SerializeField] [HideInInspector] private HairInput hairInput;

        private void OnValidate() {
            hairInput = GetComponent<HairInput>();
        }

        private void Start() {
            hairInput.SetFunction(new RemoveHairFunction());
        }
    }
}

