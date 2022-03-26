namespace LZLToolBox.CommonPlugin.Interfaces
{
    public interface ISubscribeEvent
    {
        /// <summary>
        /// 订阅事件，一般在Awake中调用
        /// </summary>
        void SubscribeEvent();

        /// <summary>
        /// 取消订阅事件，一般在生命周期末尾时调用
        /// </summary>
        void UnSubscribeEvent();
    }
}