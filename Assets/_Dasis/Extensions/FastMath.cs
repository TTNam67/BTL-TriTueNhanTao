using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dasis.Extensions
{
    public static class FastMath
    {
        public static System.Random random = new System.Random();

        public static float Max(float a, float b)
        {
            return a > b ? a : b;
        }

        public static int Sign(int x)
        {
            if (x < 0)
                return -1;
            if (x > 0)
                return 1;
            return 0;
        }

        public static int Abs(int x)
        {
            if (x >= 0) return x;
            return -x;
        }

        public static float Abs(float x)
        {
            if (x >= 0) return x;
            return -x;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (min > value) return min;
            if (max < value) return max;
            return value;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (min > value) return min;
            if (max < value) return max;
            return value;
        }

        public static bool IsEqual(List<Vector2Int> list1, List<Vector2Int> list2)
        {
            if (list1.Count != list2.Count)
                return false;
            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] != list2[i]) return false;
            }
            return true;
        }

        public static void Swap(ref int element1, ref int element2)
        {
            int tmp = element1;
            element1 = element2;
            element2 = tmp;
        }

        public static void Swap(ref Vector2Int vec1, ref Vector2Int vec2)
        {
            Vector2Int tmp = vec1;
            vec1 = vec2;
            vec2 = tmp;
        }

        public static List<int> GetRandomizedOrderList(int startIndex, int endIndex)
        {
            List<int> list = new List<int>();
            for (int i = startIndex; i <= endIndex; i++)
            {
                list.Add(i);
            }
            return Shuffle(list);
        }

        public static bool WithChanceOf(float probability)
        {
            if (probability == 0) return false;
            return UnityEngine.Random.Range(0, 1f) <= probability;
        }

        public static List<int> Shuffle(List<int> list)
        {
            return list.OrderBy(val => val = random.Next()).ToList();
        }

        public static List<int> ShuffleWithProbability(List<int> list, float probability)
        {
            int swapCount = 0;
            for (int i = 0; i < list.Count; i++)
            {
                List<int> shuffleIds = GetRandomizedOrderList(i + 1, list.Count - 1);
                foreach (int j in shuffleIds)
                {
                    if (!WithChanceOf(probability / shuffleIds.Count)) continue;
                    swapCount++;
                    int tmp = list[i];
                    list[i] = list[j];
                    list[j] = tmp;
                }

            }
            return list;
        }

        public static float CosineSimilarity(Vector2 vec1, Vector2 vec2)
        {
            return (vec1.x * vec2.x + vec1.y * vec2.y)
                / Mathf.Sqrt((vec1.x * vec1.x + vec1.y * vec1.y) * (vec2.x * vec2.x + vec2.y * vec2.y));
        }

        public static int DMYComparision(DateTime time1, DateTime time2)
        {
            int compareMY = MYComparision(time1, time2);
            if (MYComparision(time1, time2) != 0)
                return compareMY;
            if (time1.Day > time2.Day)
                return 1;
            if (time1.Day < time2.Day)
                return -1;
            return 0;
        }

        public static int MYComparision(DateTime time1, DateTime time2)
        {
            if (time1.Year > time2.Year)
                return 1;
            if (time1.Year < time2.Year)
                return -1;
            if (time1.Month > time2.Month)
                return 1;
            if (time1.Month < time2.Month)
                return -1;
            return 0;
        }

        public static class Bitmask
        {
            public static int OnBit(int mask, int position)
            {
                return mask | (1 << position);
            }

            public static int OffBit(int mask, int position)
            {
                return mask & ~(1 << position);
            }

            public static bool IsOnBit(int mask, int position)
            {
                return ((mask >> position) & 1) == 1;
            }

            public static int GetBit(int mask, int position)
            {
                return (mask >> position) & 1;
            }

            public static int GetOnBitAmount(int mask)
            {
                return Convert.ToString(mask, 2).Replace("0", "").Length;
            }

            public static string ToString(int mask)
            {
                return Convert.ToString(mask, 2);
            }
        }
    }
}
