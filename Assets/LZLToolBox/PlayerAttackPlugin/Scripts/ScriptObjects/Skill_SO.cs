using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SO : ScriptableObject
{
    [Tooltip("消耗的法力值")]
    public float _needMana;
    [Tooltip("技能冷却时间")]
    public float _coolTime;
}
