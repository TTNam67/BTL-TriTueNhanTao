using System;
using System.Collections;
using UnityEngine;

namespace Dasis.Extensions
{
    public static class WaitExtension
    {
        public static void WaitForCondition(this MonoBehaviour monoBehaviour, Func<bool> condition, Action action)
        {
            monoBehaviour.StartCoroutine(WaitingForCondition(condition, action));
        }

        private static IEnumerator WaitingForCondition(Func<bool> condition, Action callback)
        {
            while (!condition())
            {
                yield return null;
            }
            callback();
        }

        public static void WaitForSeconds(this MonoBehaviour monoBehaviour, float seconds, Action action)
        {
            monoBehaviour.StartCoroutine(WaitingForSeconds(seconds, action));
        }

        private static IEnumerator WaitingForSeconds(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback();
        }

        public static void WaitForFrames(this MonoBehaviour monoBehaviour, int frames, Action action)
        {
            monoBehaviour.StartCoroutine(WaitingForFrames(frames, action));
        }

        private static IEnumerator WaitingForFrames(int frames, Action callback)
        {
            while (frames > 0)
            {
                frames--;
                yield return null;
            }
            callback();
        }
    }
}