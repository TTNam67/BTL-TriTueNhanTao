using Dasis.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dasis.UI
{
    public class TabGroup : MonoBehaviour
    {
        [SerializeField]
        private List<TabButton> tabButtons;

        private int value = 0;

        public int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnChangedValue?.Invoke(value);
                UpdateDisplay();
            }
        }

        public Action<int> OnChangedValue { get; set; }

        private void Awake()
        {
            foreach (var (tabButton, index) in tabButtons.WithIndex())
            {
                tabButton.Clicked += OnClickedOnTabButton;
                tabButton.Index = index;
            }
        }

        public void OnClickedOnTabButton(int index)
        {
            Value = index;
        }

        public void UpdateDisplay()
        {
            foreach (var tabButton in tabButtons)
            {
                tabButton.Selected = false;
            }
            tabButtons[value].Selected = true;
        }
    }
}
