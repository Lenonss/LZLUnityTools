using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettAccountPanelHandler : UISettlePanelCtrl_Base
{
    public AnimationCurve Curve;
    public Text goldText;
    protected override void InitDataOnEnable()
    {

    }

    private void Update()
    {
        Debug.Log($"{Time.time}: "+Curve.Evaluate(Time.time));
    }

    protected override void GoAheadButtonFunc()
    {
       
        CurrencyAnimUp(ref goldText,0,300,Time.deltaTime);
    }

    protected override void MutiplyRecieveButtonFunc()
    {

    }

    protected override void GiveUpButtonFunc()
    {

    }
}
