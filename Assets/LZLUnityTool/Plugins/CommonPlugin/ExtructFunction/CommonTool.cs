using LZLUnityTool.Plugins.CommonPlugin.Interface;
using UnityEngine;

namespace LZLUnityTool.Plugins.CommonPlugin.ExtructFunction
{
    public static class CommonTool
    {
        /// <summary>
        /// 概率计算
        /// </summary>
        /// <param name="probability">百分比数值</param>
        /// <returns>是否命中</returns>
        public static bool Probability(float probability)
        {
            if (probability < 0) return false;
            return UnityEngine.Random.Range(100, 10001) * 0.01f <= probability;
        }
        
        /// <summary>
        /// 到目标透明值
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="duration"></param>
        /// <param name="fader"></param>
        /// <typeparam name="T"></typeparam>
        public static void FadeTo<T>(float alpha, float duration, IFadeAble<T> fader) where T : MonoBehaviour
        {
            if (fader.FadeCoroutine != null) 
                fader.MonoBehaviour.StopCoroutine(fader.FadeCoroutine);
            fader.FadeCoroutine = fader.MonoBehaviour.StartCoroutine(fader.Fade(alpha, duration));
        }
        
        /// <summary>
        /// 设置游戏体激活状态
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="value"></param>
        public static void SetActive(GameObject gameObject, bool value)
        {
            if (!gameObject) return;
            if (gameObject.activeSelf != value) gameObject.SetActive(value);
        }
        public static void SetActive(Component component, bool value)
        {
            if (!component) return;
            SetActive(component.gameObject, value);
        }
    }
}