using Dasis.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Dasis.UI
{
    /// <summary>
    /// An input field only works for numbers with clamping
    /// </summary>
    public class NumberInput : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField inputField;

        [SerializeField]
        private float minValue, maxValue;

        [SerializeField]
        private bool initAtStart = true;

        [SerializeField]
        private bool ignoreSameValue = true;

        [SerializeField]
        private bool wholeNumbers;

        [SerializeField, PropertyRange("minValue", "maxValue")]
        private float value;

        [SerializeField]
        private UnityEvent onChangeValue;

        public float Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                OnChangeValue();
                UpdateDisplay();
            }
        }

        public TMP_InputField InputField => inputField;

        private float lastValue;

        private void Awake()
        {
            lastValue = value - 1;
        }

        private void Start()
        {
            if (!initAtStart)
                return;
            OnChangeValue();
            UpdateDisplay();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (wholeNumbers)
            {
                value = (int)value;
            }
            UpdateDisplay();
        }
#endif

        public void OnInputFieldChangeValue()
        {
            if (float.TryParse(inputField.text, out float input))
            {
                value = FastMath.Clamp(input, minValue, maxValue);
            }
            OnChangeValue();
            UpdateDisplay();
        }

        private void OnChangeValue()
        {
            if (lastValue == value && ignoreSameValue)
            {
                return;
            }
            onChangeValue?.Invoke();
            lastValue = value;
        }

        public void UpdateDisplay()
        {
            if (wholeNumbers)
            {
                inputField.text = $"{value}";
                return;
            }
            inputField.text = $"{value:F2}";
        }
    }
}
