using System.Collections;
using UnityEngine;

namespace LZLUnityTool.Plugins.CommonPlugin.Interface
{
    public interface IFadeAble<T> where T : MonoBehaviour
    {
        T MonoBehaviour { get; }

        Coroutine FadeCoroutine { get; set; }

        IEnumerator Fade(float alpha, float duration);
    }
}