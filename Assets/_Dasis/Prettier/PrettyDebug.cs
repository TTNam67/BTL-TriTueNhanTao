using UnityEngine;

namespace Dasis.Prettier
{
    public class PrettyDebug : MonoBehaviour
    {
        public static bool enableLog = true;

        public static void LogError(string content, Color color, bool setBold = false)
        {
            if (!enableLog)
                return;
            Debug.LogError(ColorizedText(content, color, setBold));
            return;
        }

        public static void LogWarning(string content, Color color, bool setBold = false)
        {
            if (!enableLog)
                return;
            Debug.LogWarning(ColorizedText(content, color, setBold));
            return;
        }

        public static void Log(string content, Color color, bool setBold = false)
        {
            if (!enableLog)
                return;
            Debug.Log(ColorizedText(content, color, setBold));
            return;
        }

        public static string ColorizedText(string content, Color color, bool setBold = false)
        {
            if (setBold)
                return $"<b><color=#{ColorUtility.ToHtmlStringRGB(color)}>{content}</color></b>";
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{content}</color>";
        }
    }
}
