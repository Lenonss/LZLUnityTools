using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LZLToolBox.PlayerController
{
    public class LZLAttackNormalAttribute : MonoBehaviour
    {
        [Tooltip("角色的动画状态机")]
        public Animator _selfAnimator;

        [Tooltip("普攻以几种样式循环")]
        public int _nolAttackCycles = 1;

        public List<string> _nolAtkAnimParameters = new List<string>();
        
    }
}

