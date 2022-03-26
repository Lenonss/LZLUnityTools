namespace LZLUnityTool.Plugins.InvntoryPlugin.Serialize
{

    using UnityEngine;
    using UnityEditor;
    
    [CustomPropertyDrawer(typeof(StringStringDictionary))]
    [CustomPropertyDrawer(typeof(ObjectColorDictionary))]
    [CustomPropertyDrawer(typeof(StringColorArrayDictionary))]
    [CustomPropertyDrawer(typeof(StringBaseGoodsSODic))]
    public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

    [CustomPropertyDrawer(typeof(ColorArrayStorage))]
    public class AnySerializableDictionaryStoragePropertyDrawer: SerializableDictionaryStoragePropertyDrawer {}

}