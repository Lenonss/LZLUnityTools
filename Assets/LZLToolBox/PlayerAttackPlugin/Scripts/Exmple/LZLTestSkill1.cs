using System.Collections;
using System.Collections.Generic;
using LZLToolBox.PlayerController;
using UnityEngine;

public class LZLTestSkill1 : LZLBaseSkillGOBJ
{

    public override LZLSkillDamageArg GetDamageValue(float magicRatio, float physicRatio,AtkDamageType dmgType)
    {
        
        Debug.Log("in child");
        LZLSkillDamageArg result = new LZLSkillDamageArg();
        switch (dmgType)
        {
            case AtkDamageType.Both:
                result.magicDamage = 10 + magicRatio * _selfAtkCtrl.CurMagicAtk;
                result.physicDamge = 0 + physicRatio * _selfAtkCtrl.CurPhysicAtk;
                break;
            case AtkDamageType.Magic:
                result.magicDamage = 10 + magicRatio * _selfAtkCtrl.CurMagicAtk;
                result.physicDamge = 0;
                break;
            case AtkDamageType.Physic:
                result.magicDamage = 0;
                result.physicDamge = 10 + physicRatio * _selfAtkCtrl.CurPhysicAtk;
                break;
        }
        base.GetDamageValue(magicRatio, physicRatio, dmgType);
        return result;
    }
}
