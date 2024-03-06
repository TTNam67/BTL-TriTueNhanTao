using Dasis.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Dasis.UI
{
    /// <summary>
    /// Combies Unity UI Slider (UUS) and TextMeshPro Input Field (TIF) to one.
    /// Required TIF to have content type is Integer or Decimal Number
    /// </summary>
    public class NumberSlider : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        [SerializeField]
        private TMP_InputField inputField;

        [SerializeField]
        private float minValue, maxValue;

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

        private float lastValue;

        private void Awake()
        {
            OverrideUUSAttributes();
            lastValue = value - 1;
        }

        private void Start()
        {
            OnChangeValue();
            UpdateDisplay();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            OverrideUUSAttributes();
            if (wholeNumbers)
            {
                value = (int)value;
            }
            UpdateDisplay();
        }
#endif

        public void OverrideUUSAttributes()
        {
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.wholeNumbers = wholeNumbers;
        }

        public void OnSliderChangeValue()
        {
            value = FastMath.Clamp(slider.value, minValue, maxValue);
            OnChangeValue();
            UpdateDisplay();
        }

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
            if (lastValue == value)
            {
                return;
            }
            onChangeValue?.Invoke();
            lastValue = value;
        }

        public void UpdateDisplay()
        {
            slider.value = value;
            if (wholeNumbers)
            {
                inputField.text = $"{value}";
                return;
            }
            inputField.text = $"{value:F2}";
        }
    }
}
