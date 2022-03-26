using UnityEngine;
using UnityEngine.EventSystems;

namespace LZLUnityTool.Plugins.SpecialEffectPlugin.UnitControls_Mono
{
    public class ScreenSparkUnitCtrl : MonoBehaviour
    {
        
        public static ScreenSparkUnitCtrl instance;
        public static bool prepared = false;
        
        void Start () 
        {
            instance = this;
            gameObject.SetActive(false);
        }
        
        //设置特效的位置在点击处
        public static void Prepare()
        {
            var go = EventSystem.current.currentSelectedGameObject;
            if (go)
                Prepare(go.transform.position);
        }
        //设置特效的位置在指定位置
        public static void Prepare(Vector3 position)
        {
            if (instance)
            {
                instance.transform.position = new Vector3(position.x, position.y, instance.transform.position.z);
                prepared = true;
            }
        }

        /// <summary>
        /// 显示特效在准备好的位置
        /// </summary>
        public static void Show()
        {
            if (prepared == true && instance)
            {
                instance.gameObject.SetActive(false);
                instance.gameObject.SetActive(true);
                prepared = false;
            }
            else
            {
                var go = EventSystem.current.currentSelectedGameObject;
                if(go)
                    Show(go.transform.position);
            }
        }
        /// <summary>
        /// 显示特效在指定位置
        /// </summary>
        /// <param name="position"></param>
        public static void Show(Vector3 position)
        {
            if (instance)
            {
                instance.transform.position = new Vector3(position.x, position.y, instance.transform.position.z);
                instance.gameObject.SetActive(false);
                instance.gameObject.SetActive(true);
            }
        }

    }
}