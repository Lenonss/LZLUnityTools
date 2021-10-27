using System.IO;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
namespace LZLToolBox.CommonPlugin.Tag
{


    public class TagEnumGenarator : MonoBehaviour
    {
        [MenuItem("LZLToolBox/ComonPlugin/GenTagEnum")]
        public static void GenTagEnum()
        {
            var tags = InternalEditorUtility.tags;
            var arg = "";
            foreach (var tag in tags)
            {
                arg += "\t\t" + tag + ",\n";
            }

            var nameSpace = "namespace LZLToolBox.CommonPlugin.Tag\n";
            var res = nameSpace +"{\n" 
                                + "\tpublic enum UnityEnumTag\n\t{\n" 
                                + arg 
                                + "\t}\n"
                                +"}\n";
            var path = Application.dataPath + "/LZLToolBox/Res/UnityTagEnum.cs";
            File.WriteAllText(path, res, Encoding.UTF8);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}