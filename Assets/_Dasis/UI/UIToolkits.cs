using UnityEngine;

namespace Dasis.UI
{
    public static class UIToolkits
    {
        public static Vector3[] CornersOfRect(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            return corners;
        }
    }
}
