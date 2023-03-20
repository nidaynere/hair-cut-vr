using HairTools.InputDevices;
using UnityEngine;

namespace HairTools.ItemSwitcher {
    public class ItemSwitcher : MonoBehaviour {
        [SerializeField] private GameObject[] items;

        private DeviceInput[] deviceInputs;
        private GameObject activeItem;
        private int activeIndex = 0;

        private void Awake() {
            deviceInputs = FindObjectsOfType<DeviceInput>();

            SwitchItem(activeIndex++);
        }

        private void Update() {
            foreach (var deviceInput in deviceInputs) {
                if (!deviceInput.IsSelectPressed()) {
                    continue;
                }

                SwitchItem(activeIndex++);

                if (activeIndex == items.Length) {
                    activeIndex = 0;
                }
            }
        }

        private void SwitchItem(int index) {
            if (activeItem != null) {
                Destroy(activeItem.gameObject);
            }

            activeItem = Instantiate(items[index]);
        }
    }
}