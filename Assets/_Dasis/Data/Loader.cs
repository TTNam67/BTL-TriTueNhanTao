using System.IO;
using UnityEngine;

namespace Dasis.Data
{
    public class Loader : MonoBehaviour
    {
        public static bool IsFileExist(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }
            return true;
        }

        public static string ReadTextFile(string path)
        {
            string data = string.Empty;
            try
            {
                var reader = new StreamReader(path);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line += '\n';
                    data += line;
                }
            }
            catch
            {
                return string.Empty;
            }
            data = data.Remove(data.Length - 1);
            return data;
        }
    }
}
