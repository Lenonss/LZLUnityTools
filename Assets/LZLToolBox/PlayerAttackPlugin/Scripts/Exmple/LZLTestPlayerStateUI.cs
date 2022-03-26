using System.Collections;
using System.Collections.Generic;
using LZLToolBox.PlayerController;
using UnityEngine;
using UnityEngine.UI;

public class LZLTestPlayerStateUI : MonoBehaviour
{
    public Text _lifeValue;

    public Text _manaValue;

    public Text _s1cTime;

    public Text _atkState;

    public GameObject targetPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var atkBaseCtrl = targetPlayer.GetComponent<LZLBaseAttackCtrl>();
        _lifeValue.text = atkBaseCtrl.CurHealth.ToString();
        _manaValue.text = atkBaseCtrl.CurMana.ToString();
        _atkState.text = atkBaseCtrl.GetAttackState().ToString();
        if (atkBaseCtrl.GetSkillObjBySkillId_CanCreate(0) != null)
        {
            _s1cTime.text = atkBaseCtrl.GetSkillObjBySkillId_CanCreate(0).GetComponent<LZLBaseSkillGOBJ>()
                .GetCurrentCoolTime().ToString();
        }
        else
        {
            _s1cTime.text = 0.ToString();
        }
    }
}
