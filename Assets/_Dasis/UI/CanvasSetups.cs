using Dasis.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Dasis.UI
{
    [DefaultExecutionOrder(order: -1)]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    public class CanvasSetups : MonoBehaviour
    {
        private Canvas _canvas;
        private CanvasScaler _canvasScaler;

        private readonly Vector2 aspectRange = new Vector2(0.45f, 0.75f);
        private readonly Vector2 matchRange = new Vector2(0.3f, 1);

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvasScaler = GetComponent<CanvasScaler>();
            _canvasScaler.matchWidthOrHeight = GetMatch(_canvas.worldCamera.aspect);
        }

        public float GetMatch(float aspect)
        {
            aspect = FastMath.Clamp(aspect, aspectRange.x, aspectRange.y);
            float percentage = (aspect - aspectRange.x) / (aspectRange.y - aspectRange.x);
            return matchRange.x + percentage * (matchRange.y - matchRange.x);
        }
    }
}
