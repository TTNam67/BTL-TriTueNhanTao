using UnityEngine;
using UnityEngine.UI;

namespace Dasis.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class ToggleButton : MonoBehaviour
    {
        [SerializeField]
        protected Image icon;

        [SerializeField]
        protected Sprite onSprite, offSprite;

        protected bool active;

        public bool Active
        {
            get { return active; }
            set
            {
                active = value;
                UpdateDisplay();
            }
        }

        public void OnClicked()
        {
            SwapState();
            OnTap();
        }

        public void SwapState()
        {
            Active = !Active;
            if (active)
            {
                OnActive();
            }
            else
            {
                OnDeactive();
            }
        }

        public abstract void OnTap();
        public abstract void OnActive();
        public abstract void OnDeactive();

        public void UpdateDisplay()
        {
            icon.gameObject.SetActive(true);
            if (active)
            {
                icon.sprite = onSprite;
                return;
            }
            if (offSprite == null)
            {
                icon.gameObject.SetActive(false);
                return;
            }
            icon.sprite = offSprite;
        }
    }
}
