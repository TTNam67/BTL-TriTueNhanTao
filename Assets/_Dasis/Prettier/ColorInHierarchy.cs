using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Dasis.Prettier
{
#if UNITY_EDITOR
    /// <summary> Sets a background color for this game object in the Unity Hierarchy window </summary>
    [InitializeOnLoad]
#endif
    public class ColorInHierarchy : MonoBehaviour
    {
#if UNITY_EDITOR
        #region [ static - Update editor ]
        private static readonly Dictionary<UnityEngine.Object, Color> coloredObjects = new Dictionary<UnityEngine.Object, Color>();
        private static Vector2 offset = new Vector2(20, 1);

        static ColorInHierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        }

        private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID);

            if (obj != null && coloredObjects.ContainsKey(obj))
            {
                if ((obj as GameObject).GetComponent<ColorInHierarchy>())
                {
                    HierarchyBackground(obj, selectionRect, coloredObjects[obj]);
                }
                else // the ColorInHierarchy component has been removed from the gameObject
                {
                    coloredObjects.Remove(obj);
                }
            }
        }

        private static void HierarchyBackground(UnityEngine.Object obj, Rect selectionRect, Color backgroundColor)
        {
            Rect offsetRect = new Rect(selectionRect.position + offset, selectionRect.size);
            Rect bgRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width + 50, selectionRect.height);

            EditorGUI.DrawRect(bgRect, backgroundColor);
            EditorGUI.LabelField(offsetRect, obj.name, new GUIStyle()
            {
                normal = new GUIStyleState() { textColor = InvertColor(backgroundColor) },
                fontStyle = FontStyle.Bold
            }
            );
        }

        private static Color InvertColor(Color toInvert)
        {
            Color.RGBToHSV(toInvert, out float h, out float s, out float v);
            h = (h + .5f) % 1;
            v = (v + .5f) % 1;
            return Color.HSVToRGB(h, s, v);
        }
        #endregion [ static - Update editor ]

        #region [ instanced - Set color to be used by the editor ]
        public Color colorInHierarchy = Color.grey;

        private void Reset()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            if (!coloredObjects.ContainsKey(gameObject)) // notify editor of new color
            {
                coloredObjects.Add(gameObject, colorInHierarchy);
            }
            else if (coloredObjects[gameObject] != colorInHierarchy) // color has changed
            {
                coloredObjects[gameObject] = colorInHierarchy;
            }
        }
        #endregion [ instanced -  Subscribe color to editor ]
#endif
    }
}