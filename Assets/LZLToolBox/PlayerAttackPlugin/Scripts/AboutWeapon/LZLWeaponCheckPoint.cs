using System;
using System.Collections;
using System.Collections.Generic;
using LZLToolBox.PlayerController;
using Sirenix.OdinInspector;
using UnityEngine;

public class LZLWeaponCheckPoint : MonoBehaviour
{
    [ReadOnly]
    public string DebugStr;
    
    [EnumToggleButtons]
    public SelfInpectorEnum _select;
    
    [BoxGroup("显示器")]
    [Title("显示球体的颜色"),ShowIf("_select",SelfInpectorEnum.显示球)]
    public Color _ballColor = Color.red;
    
    [BoxGroup("显示器")]
    [Title("显示球体的半径"),ShowIf("_select",SelfInpectorEnum.显示球)]
    [Range(0.05f,0.2f)]
    public float _ridus;
    
    LZLCheckPointManager _mgr;

    private Vector3 tempPos;

    private bool _canCheck = false;

    /// <summary>
    /// <para>设置检测点的中心管理器</para>
    /// </summary>
    /// <param name="mgr"></param>
    public void SetPointManager(LZLCheckPointManager mgr)
    {
        _mgr = mgr;
    }

    private void Awake()
    {
        tempPos = transform.position;
    }

    private void Update()
    {
        if (_mgr==null)
        {
            DebugStr = "mgr hasnot value";
        }

        if (_canCheck)
        {
            CheckTarget();
        }
        
    }

    /// <summary>
    /// <para>设置是否开启检测的字段的值</para>
    /// </summary>
    /// <param name="state"></param>
    public void SetCanCheckState(bool state)
    {
        _canCheck = state;
    }

    /// <summary>
    /// 射线检测检测目标
    /// </summary>
    private void CheckTarget()
    {
        var direction = transform.position - tempPos;
        var distance = Vector3.Distance(transform.position, tempPos);
        var hit = new RaycastHit();
        Debug.DrawRay(tempPos, direction, Color.red, 0.3f);
        //射线检测
        if (Physics.Raycast(tempPos, direction, out hit, distance))
        {
            if (hit.collider.GetComponent<LZLBaseAttackCtrl>())
            {
                _mgr.GiveDamage(hit.collider.GetComponent<LZLBaseAttackCtrl>().id);
            }
        }
        tempPos = transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _ballColor;
        Gizmos.DrawSphere(transform.position,_ridus);
    }
    
    public enum SelfInpectorEnum
    {
        None,显示球
    }
}
