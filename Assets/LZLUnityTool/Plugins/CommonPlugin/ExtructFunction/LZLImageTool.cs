using UnityEngine;
using UnityEngine.UI;

namespace LZLUnityTool.Plugins.CommonPlugin.ExtructFunction
{
    public static class LZLImageTool
    {

        public static void SetImage(this Image image, Texture tex)
        {
            var tex2D = (Texture2D) tex;
            image.sprite = Sprite.Create(tex2D,new Rect(0,0,tex.width,tex.height),Vector2.zero);
        }
    }
}