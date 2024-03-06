using System.Collections.Generic;
using System.IO;

namespace Dasis.Extensions
{
    public class EnumGenerator
    {
        public static void Generate(string enumName, List<string> elements)
        {
            string enumContent = string.Empty;
            enumContent += "namespace Dasis.Enum \n{\n";
            enumContent += $"\tpublic enum {enumName}" + "\n\t{\n";
            foreach (var element in elements)
            {
                string name = element;
                if (!char.IsLetter(name[0]) && !name[0].Equals("_"))
                {
                    name = $"_{name}";
                }
                enumContent += $"\t\t{name},\n";
            }
            enumContent += "\t}\n}";

            var path = $"Assets/_Dasis/Enum/{enumName}.cs";

            var scriptFile = new StreamWriter(path);
            scriptFile.Write(enumContent);
            scriptFile.Close();

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.ImportAsset(path, UnityEditor.ImportAssetOptions.ForceSynchronousImport);
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}
