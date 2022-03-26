using System.Collections;
using System.Collections.Generic;
using LZLToolBox.PlayerController;
using UnityEngine;
using UnityEngine.UI;

public class LZLLifeText : MonoBehaviour
{
    private Text text;

    public LZLBaseAttackCtrl target;
    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (text==null)
        {
            return;
        }

        if (target==null)
        {
            return;
        }

        text.text = target.CurHealth.ToString();
    }
}
