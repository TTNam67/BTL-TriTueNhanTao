using System.Collections.Generic;
using UnityEngine;

namespace Dasis.Common
{
    [DefaultExecutionOrder(order: -1)]
    public class CameraAutoFit : MonoBehaviour
    {
        public enum FitType
        {
            Height,
            Width,
            Area,
        }

        [SerializeField]
        private List<Camera> cameras;

        [SerializeField]
        private SpriteRenderer rink;

        [SerializeField]
        private FitType fitType;

        [SerializeField]
        private float ratio;

        private void Awake()
        {
            foreach (var camera in cameras)
            {
                switch (fitType)
                {
                    case FitType.Height:
                        FitHeight(camera);
                        break;
                    case FitType.Width:
                        FitWidth(camera);
                        break;
                    case FitType.Area:
                        FitArea(camera);
                        break;
                }
            }
        }

        public void FitHeight(Camera camera)
        {
            camera.orthographicSize = rink.bounds.size.y / 2 * ratio;
        }

        public void FitWidth(Camera camera)
        {
            camera.orthographicSize = rink.bounds.size.x * Screen.height / Screen.width * 0.5f * ratio;
        }

        public void FitArea(Camera camera)
        {
            float screenRatio = (float)Screen.width / Screen.height;
            float targetRatio = rink.bounds.size.x / rink.bounds.size.y;

            if (screenRatio >= targetRatio)
            {
                camera.orthographicSize = rink.bounds.size.y / 2 * ratio;
            }
            else
            {
                float differenceInSize = targetRatio / screenRatio;
                camera.orthographicSize = rink.bounds.size.y / 2 * differenceInSize * ratio;
            }
        }
    }
}
