namespace LZLToolBox.PlayerController
{
    public class LZLAtkConfigManager
    {
        private LZLAtkConfigManager instance;
        public LZLAtkConfigManager Instance
        {
            get
            {
                if (instance==null)
                {
                    instance = this;
                }

                return instance;
            }
        }
        
        //属性
        
    }
}