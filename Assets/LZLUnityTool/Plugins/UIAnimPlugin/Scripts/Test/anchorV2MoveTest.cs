using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace LZLUnityTool.Plugins.UIAnimPlugin
{
    public class anchorV2MoveTest : MonoBehaviour
    {
        public List<UISampleAnimUnit> fullScreenUIList = new List<UISampleAnimUnit>();
        void Start()
        {
        
        }

        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //FullScreen();
                StartCoroutine(FullScreen());
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                StartCoroutine(LocalRotate());
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                StartCoroutine(Size());
            }
        }

        private IEnumerator FullScreen()
        {
            var maxduration = fullScreenUIList.Max(item => item._anchorV2.duration);
            UISampleAnimMgr.DoAnchorV2Move(fullScreenUIList,true);
            yield return new WaitForSecondsRealtime(maxduration);
            UISampleAnimMgr.DoAnchorV2Move(fullScreenUIList,false);
        }
        private IEnumerator LocalRotate()
        {
            var maxduration = fullScreenUIList.Max(item => item._localRotateV3.duration);
            UISampleAnimMgr.DoLocalRotateV3(fullScreenUIList,true);
            yield return new WaitForSecondsRealtime(maxduration);
            UISampleAnimMgr.DoLocalRotateV3(fullScreenUIList,false);
        }

        private IEnumerator Size()
        {
            var maxduration = fullScreenUIList.Max(item => item._sizeV3.duration);
            UISampleAnimMgr.DoSizeV3(fullScreenUIList,true);
            yield return new WaitForSecondsRealtime(maxduration);
            UISampleAnimMgr.DoSizeV3(fullScreenUIList,false);
        }
    }
}

