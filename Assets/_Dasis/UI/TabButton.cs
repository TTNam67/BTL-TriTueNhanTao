using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dasis.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class TabButton : MonoBehaviour
    {
        [SerializeField]
        protected Image icon;

        [SerializeField]
        protected TextMeshProUGUI text;

        [SerializeField, BoxGroup("ON")]
        protected Sprite onSprite;

        [SerializeField, BoxGroup("OFF")]
        protected Sprite offSprite;

        [SerializeField, BoxGroup("ON")]
        protected Color onTextColor = Color.white;

        [SerializeField, BoxGroup("OFF")]
        protected Color offTextColor = Color.white;

        protected bool selected;

        public Action<int> Clicked { get; set; }

        public int Index { get; set; }

        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                UpdateDisplay();
            }
        }

        public void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            if (selected) return;
            OnClicked();
            Clicked?.Invoke(Index);
        }

        public void UpdateDisplay()
        {
            if (selected)
            {
                icon.sprite = onSprite;
                icon.color = Color.white;
                text.color = onTextColor;
                OnSelected();
                return;
            }
            icon.sprite = offSprite;
            icon.color = (offSprite == null) ? Color.clear : Color.white;
            text.color = offTextColor;
            OnDeselected();
        }

        public abstract void OnClicked();
        public abstract void OnSelected();
        public abstract void OnDeselected();
    }
}

