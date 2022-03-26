using System.Collections.Generic;
using UnityEngine;

namespace LZLToolBox.PlayerController
{
    public class LZLAtkCenterManager
    {
        private static LZLAtkCenterManager instance;
        public static LZLAtkCenterManager Instance
        {
            get
            {
                if (instance==null)
                {
                    instance = new LZLAtkCenterManager();
                }

                return instance;
            }
        }

        #region 事件定义

        public delegate void IntAndListInt(int atk, List<int> defs);
        public delegate void GiveDamageDel(SampleDmgArg atk, List<int> defs);
        public static event GiveDamageDel GiveDamagePipeLine;
        #endregion

        #region 事件触发

        /// <summary>
        /// 伤害给予管道触发
        /// </summary>
        /// <param name="atk"></param>
        /// <param name="defs"></param>
        public static void Invoke_GiveDamagePipeLine(SampleDmgArg atk, List<int> defs)
        {
            GiveDamagePipeLine(atk, defs);
        }

        #endregion
        
        #region 属性字段
        /// <summary>
        /// 存放场景中所有的LZLBaseAtkCtrl
        /// </summary>
        private Dictionary<int, LZLBaseAttackCtrl> _GlobalAtkCtrlCategory;
        #endregion

        #region 外部可修改字段
        [Tooltip("分配id号的前缀"),Range(1,1000),SerializeField]
        private int idPrefix = 1000;

        #endregion

        #region _GlobalAtkCtrlCategory 相关函数

        /// <summary>
        /// LZLBaseAttackCtrl的注册函数
        /// </summary>
        /// <param name="consumer"></param>
        /// <returns></returns>
        public RegisterArg RegisterAtkCtrl(LZLBaseAttackCtrl consumer)
        {
            RegisterArg result = new RegisterArg();
            if (_GlobalAtkCtrlCategory==null)
            {
                _GlobalAtkCtrlCategory = new Dictionary<int, LZLBaseAttackCtrl>();
            }
            //判断是否已经注册过
            if (_GlobalAtkCtrlCategory.ContainsValue(consumer))
            {
                result.id = -9999;
                result.RegistSuccess = false;
                return result;
            }

            
            //生成不重复的随机id
            int id = 0;
            do
            {
                id = BornRandomId(idPrefix);
            } while (_GlobalAtkCtrlCategory.ContainsKey(id));
            //注册
            _GlobalAtkCtrlCategory.Add(id,consumer);
            result.id = id;
            result.RegistSuccess = true;
            return result;
        }

        /// <summary>
        /// 判断对应的id是否注册
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckIdInGlobalAtkCtrlCategory(int id)
        {
            if (_GlobalAtkCtrlCategory==null || _GlobalAtkCtrlCategory.Count==0)
            {
                return false;
            }

            return _GlobalAtkCtrlCategory.ContainsKey(id);
        }

        /// <summary>
        /// 根据ID号码来获取LZLBaseAttackCtrl
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LZLBaseAttackCtrl GetBaseAttackVtrlById(int id)
        {
            if (!CheckIdInGlobalAtkCtrlCategory(id))
            {
                return null;
            }

            return _GlobalAtkCtrlCategory[id];
        }

        #endregion


        /// <summary>
        /// 生成随机id
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private int BornRandomId(int prefix)
        {
            int result = 0;
            //获得id后缀
            int suffix = Random.Range(1, 10000);
            result = prefix * (int) Mathf.Pow(10, suffix.ToString().Length) + suffix;
            return result;
        }

        #region 返回类

        public struct RegisterArg
        {
            public int id;
            public bool RegistSuccess;
        }
        #endregion
    }
}