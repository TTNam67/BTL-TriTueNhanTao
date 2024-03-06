using System.IO;
using UnityEditor;
using UnityEngine;

namespace Dasis.Data
{
    public class Saver : MonoBehaviour
    {
        public static void WriteTextFile(string path, string data)
        {
            var textFile = new StreamWriter(path);
            textFile.Write(data);
            textFile.Close();
#if UNITY_EDITOR
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
            AssetDatabase.Refresh();
#endif
        }
    }
}
