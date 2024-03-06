using Dasis.DesignPattern;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Dasis.Extensions
{
    public static class Stringify
    {
        public static string ToString(List<Object> list)
        {
            string content = string.Empty;
            foreach (var ele in list) content += $"{ele}, ";
            return content;
        }

        public static string ToString(List<GameObject> list)
        {
            string content = string.Empty;
            foreach (var ele in list) content += $"{ele.name}, ";
            return content;
        }

        public static List<string> ToStringList(List<INameable> list)
        {
            List<string> stringList = new List<string>();
            foreach (var ele in list)
            {
                stringList.Add(ele.Name);
            }
            return stringList;
        }

        public static List<string> ToStringList(List<GameObject> list)
        {
            List<string> stringList = new List<string>();
            foreach (var ele in list)
            {
                stringList.Add(ele.name);
            }
            return stringList;
        }

        public static List<string> ToStringList(List<PoolPrefab> list)
        {
            List<string> stringList = new List<string>();
            foreach (var ele in list)
            {
                stringList.Add(ele.gameObject.name);
            }
            return stringList;
        }

        public static string CapitalizeEachWord(string s)
        {
            return Regex.Replace(s, @"\b([a-z])", m => m.Value.ToUpper());
        }
    }

    public interface INameable
    {
        public string Name { get; }
    }
}
