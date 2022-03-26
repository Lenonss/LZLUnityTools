using System.Collections;
using System.Collections.Generic;
using LZLUnityTool.Plugins.InvntoryPlugin;
using UnityEngine;

[CreateAssetMenu(menuName = "LZLUnityTool/ScriptableObjects/TaskSystem/TestWeaponGoods")]
public class TestWeaponGoods : BaseGoodsSO
{


    public override void GoodsEffect()
    {
        Debug.Log("获得武器："+GoodsNum);
    }

    public override bool UnLockGoods()
    {
        return false;
    }
}
