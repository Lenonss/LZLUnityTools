using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LZLUnityTool.Plugins.UIPlugin.UI_Gray
{
    public static class UIGrayManager
    {
        private static Material grayMat;
        
        /// <summary>
        /// 创建置灰材质球
        /// </summary>
        /// <returns></returns>
        private static Material GetGrayMat()
        {
            if(grayMat==null)
            {
                Shader shader = Shader.Find("Custom/LZLUnityTool/UI/UI-Gray");
                if(shader==null)
                {
                    Debug.Log("null");
                    return null;
                }
                Material mat = new Material(shader);
                grayMat = mat;
            }

            return grayMat;
        }

        /// <summary>
        /// 图片置灰
        /// </summary>
        /// <param name="img"></param>
        public static void SetUIGray(Image img)
        {
            img.material = GetGrayMat();
            img.SetMaterialDirty();
        }
        /// <summary>
        /// 图片置灰
        /// </summary>
        /// <param name="img"></param>
        public static void SetUIGray(RawImage img)
        {
            img.material = GetGrayMat();
            img.SetMaterialDirty();
        }

        /// <summary>
        /// 图片回复
        /// </summary>
        /// <param name="img"></param>
        public static void Recovery(Image img)
        {
            img.material = null;
        }
        /// <summary>
        /// 图片回复
        /// </summary>
        /// <param name="img"></param>
        public static void Recovery(RawImage img)
        {
            img.material = null;
        }

        private static Dictionary<Image, Material> matRecordTable = new Dictionary<Image, Material>();
        private static Dictionary<RawImage, Material> matRecordTable_raw = new Dictionary<RawImage, Material>();
        /// <summary>
        /// 图片置灰
        /// </summary>
        /// <param name="img"></param>
        public static void SetGray(this Image img)
        {
            //已经记录原材质
            if (matRecordTable.ContainsKey(img))
            {
                
            }
            else
            {
                matRecordTable.Add(img, img.material);
            }
            img.material = GetGrayMat();
            img.SetMaterialDirty();
        }
        /// <summary>
        /// 图片置灰
        /// </summary>
        /// <param name="img"></param>
        public static void SetGray(this RawImage img)
        {
            //已经记录原材质
            if (matRecordTable_raw.ContainsKey(img))
            {
                
            }
            else
            {
                matRecordTable_raw.Add(img, img.material);
            }
            img.material = GetGrayMat();
            img.SetMaterialDirty();
        }

        /// <summary>
        /// 置灰恢复
        /// </summary>
        /// <param name="img"></param>
        public static void GrayRecovery(this Image img)
        {
            if (matRecordTable.ContainsKey(img))
            {
                img.material = matRecordTable[img];
                matRecordTable.Remove(img);
            }
            else
            {
                img.material = null;
            }
        }
        /// <summary>
        /// 置灰恢复
        /// </summary>
        /// <param name="img"></param>
        public static void GrayRecovery(this RawImage img)
        {
            if (matRecordTable_raw.ContainsKey(img))
            {
                img.material = matRecordTable_raw[img];
                matRecordTable_raw.Remove(img);
            }
            else
            {
                img.material = null;
            }
        }
    }

    
}
