using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LZLUnityTool.Plugins.UIPlugin.UI_Gray
{
    public class UIGrayMethod : MonoBehaviour
    {
        public void SetImageGray(Image img)
        {
            img.SetGray();
        }
        public void SetImageGray(RawImage img)
        {
            img.SetGray();
        }

        public void RecoveryImageGray(Image img)
        {
            img.GrayRecovery();
        }
        public void RecoveryImageGray(RawImage img)
        {
            img.GrayRecovery();
        }
    }

}
