using Dasis.Common;
using DG.Tweening;
using UnityEngine;

namespace Dasis.Effect
{
    public abstract class AttractableObject : TwoDimensionObject
    {
        [SerializeField]
        protected Vector2 appearDuration;

        [SerializeField]
        protected Vector2 appearRadius;

        [SerializeField]
        protected Vector2 attractDuration;

        [SerializeField]
        protected float targetScale = 1;

        protected Sequence appear, attract;
        protected float duration;

        public Transform Collector { get; set; }

        public virtual void Appear()
        {
            attract?.Kill();
            appear?.Kill();
            appear = DOTween.Sequence();
            duration = Random.Range(appearDuration.x, appearDuration.y);
            Transform.localScale = Vector3.zero;
            Transform.eulerAngles = Vector3.forward * Random.Range(0, 360);
            appear.Join(Transform.DORotate(Vector3.forward * Random.Range(-360, 360), duration, RotateMode.LocalAxisAdd).SetEase(Ease.OutCirc));
            appear.Join(Transform.DOMove(XYPosition + Random.insideUnitCircle.normalized * Random.Range(appearRadius.x, appearRadius.y), duration).SetEase(Ease.OutCirc));
            appear.Join(Transform.DOScale(Vector3.one * targetScale, duration).SetEase(Ease.OutBack));
            appear.OnComplete(() => { Attract(); });
            appear.SetUpdate(true);
        }

        public virtual void Attract()
        {
            attract?.Kill();
            attract = DOTween.Sequence();
            duration = Random.Range(attractDuration.x, attractDuration.y);
            attract.Join(Transform.DOMoveX(Collector.position.x, duration).SetEase(Ease.InSine));
            attract.Join(Transform.DOMoveY(Collector.position.y, duration).SetEase(Ease.InBack));
            attract.Join(Transform.DORotate(Vector3.forward * Random.Range(-360, 360), duration, RotateMode.LocalAxisAdd).SetEase(Ease.InSine));
            attract.OnComplete(() =>
            {
                OnAttactCompleted();
            });
            attract.SetUpdate(true);
        }

        public abstract void OnAttactCompleted();

        protected void OnDestroy()
        {
            attract?.Kill();
            appear?.Kill();
        }
    }
}