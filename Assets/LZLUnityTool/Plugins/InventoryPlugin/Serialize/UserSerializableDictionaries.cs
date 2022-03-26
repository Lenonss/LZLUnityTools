using System;
using LZLUnityTool.Plugins.CommonPlugin.Serialize;
using LZLUnityTool.Plugins.InvntoryPlugin;

namespace LZLUnityTool.Plugins.InvntoryPlugin.Serialize
{
    using System;
    using UnityEngine;

    [Serializable]
    public class StringStringDictionary : SerializableDictionary<string, string> {}

    [Serializable]
    public class ObjectColorDictionary : SerializableDictionary<UnityEngine.Object, Color> {}

    [Serializable]
    public class ColorArrayStorage : SerializableDictionary.Storage<Color[]> {}

    [Serializable]
    public class StringColorArrayDictionary : SerializableDictionary<string, Color[], ColorArrayStorage> {}

    #region Inventory

    [Serializable]
    public class StringBaseGoodsSODic : SerializableDictionary<string, BaseGoodsSO> {}

    #endregion
    
    [Serializable]
    public class MyClass
    {
        public int i;
        public string str;
    }

    [Serializable]
    public class QuaternionMyClassDictionary : SerializableDictionary<Quaternion, MyClass> {}
}