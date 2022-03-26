using LZLUnityTool.Plugins.UIPlugin.UI_Gray;
using UnityEngine;
using UnityEngine.UI;

namespace LZLUnityTool.Plugins.UIPlugin.NewerGuid
{
    public class EventPenetrate : MonoBehaviour, ICanvasRaycastFilter
    {
        //作为目标点击事件渗透区域
        private Image target;

        public void SetTargetImage(Image tg)
        {
            target = tg;
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            //没有目标则捕捉事件渗透
            if (target == null)
            {
                return true;
            }
            //在目标范围内做事件渗透,遮挡的阻隔是否在该区域有效
            return !RectTransformUtility.RectangleContainsScreenPoint(target.rectTransform,
                sp, eventCamera);
        }

    }
}

