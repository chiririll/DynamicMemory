using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DynamicMem
{
    public class MemoryUnitInputField : MonoBehaviour
    {
        [SerializeField] private TMP_InputField size;
        [SerializeField] private TMP_Dropdown units;

        private void Awake()
        {
            var options = new List<string>(MemoryUnits.Units);
            units.AddOptions(options);
        }

        public bool TryGetValue(out int value, int maxSize = -1)
        {
            value = 0;
            try
            {
                value = int.Parse(this.size.text);
                // FIXME: Handle overflow
                value *= 1 << (10 * units.value);
                if (value <= 0 || maxSize > 0 && value >= maxSize)
                {
                    throw new System.ArgumentOutOfRangeException();
                }
                return true;
            }
            catch
            {
                this.size.HighlightUntilClick();
                return false;
            }
        }
    }
}
