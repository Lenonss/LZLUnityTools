using LZLUnityTool.Plugins.CommercializePlugin.Manager;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LZLUnityTool.Plugins.CommercializePlugin.UnitItem
{
	public class UIComponent_Chinesize : MonoBehaviour
    {
	    #region 枚举定义
	    public enum MyType 
	    {
		    Text,
		    Image
	    }
	    public enum FuncType
	    {
		    Disable,Switch
	    }
	    #endregion


	    [Title("组件类型"),EnumToggleButtons]
	    public MyType mytype;
    	[EnumToggleButtons,Title("功能类型")]
    	public FuncType selectType;
        
    
        
        [ShowIf("selectType",FuncType.Switch),
         ShowIf("mytype",MyType.Image)]
        public ChinesizeManager.ChinesizeSpriteGroup _chinesizeSpriteGroup;
    	[ShowIf("selectType",FuncType.Switch),
         ShowIf("mytype",MyType.Text)]
    	public ChinesizeManager.ChinesizeStringGroup _chinesizeStringGroup;
        
        //Text
        [ShowIf("mytype",MyType.Text),ShowIf("selectType",FuncType.Switch),InfoBox("是否启用换行符号换行",InfoMessageType.None)]
        public bool useSwitchLine = false;
        [ShowIf("useSwitchLine",true)]
        public char switchChar;
        [ShowIf("mytype",MyType.Text),ShowIf("selectType",FuncType.Switch),InfoBox("是否启用更换颜色字体",InfoMessageType.None)]
        public bool switchTextColor = false;
        [ShowIf("switchTextColor",true)]
        public ChinesizeManager.ChinesizeColorGroup chinesizeColorGroup;
    
    	[ShowIf("selectType",FuncType.Disable),InfoBox("在英文版隐藏",InfoMessageType.None)]
    	public bool disableSelfInEnglish;
    	[ShowIf("selectType",FuncType.Disable),InfoBox("在中文版隐藏",InfoMessageType.None)]
    	public bool disableSelfInChinese;
    
        [Title("公共")] 
        [InfoBox("是否切换尺寸",InfoMessageType.None)]
        public bool useSwitchSize = false;
        [ShowIf("useSwitchSize",true)]
        public ChinesizeManager.ChinesizeSizeGroup chinesizeSizeGroup;
        [Button("设置中文尺寸"),ShowIf("useSwitchSize",true),ShowIf("useSwitchSize",true)]
        public void GetChinsizeRTSize()
        {
	        if (GetComponent<RectTransform>())
	        {
		        chinesizeSizeGroup.chansizeSize = GetComponent<RectTransform>().sizeDelta;
	        }
        }
        [Button("设置英文尺寸"),ShowIf("useSwitchSize",true),ShowIf("useSwitchSize",true)]
        public void GetEnglishRTSize()
        {
	        if (GetComponent<RectTransform>())
	        {
		        chinesizeSizeGroup.englishSize = GetComponent<RectTransform>().sizeDelta;
	        }
        }
        
        
        [Title("循环检测")]
        public bool update = false;
        [InfoBox("循环监听间隔时间",InfoMessageType.None),ShowIf("update",true)]
        public float intervalSeconds = 1;


        private float counter = 0;
    	void Start () 
    	{
	        UpdateChinesize();
    	}

        private void Update()
        {
	        if (update)
	        {
		        counter += Time.deltaTime;
		        if (counter >= intervalSeconds)
		        {
			        counter = 0;
			        UpdateChinesize();
		        }
	        }
        }
        private void UpdateChinesize()
        {
	        switch (mytype)
	        {
		        case MyType.Image:
			        ImageChinesize();
			        break;
		        case MyType.Text:
			        TextChinesize();
			        break;
	        }
        }

        private void TextChinesize()
        {
	        var comp = GetComponent<Text>();
	        if (comp)
	        {
		        switch (selectType)
		        {
			        case FuncType.Disable:
				        ctrlSelfActState();
				        break;
			        case FuncType.Switch:
				        ChinesizeManager.Instance.SetTextSrcByLanguageType(ref comp, _chinesizeStringGroup);
				        if (useSwitchLine)
				        {
					        var list = comp.text.Split(switchChar);
					        if (list.Length > 1)
					        {
						        comp.text = "";
						        for (int i = 0; i < list.Length; i++)
						        {
							        comp.text += list[i];
							        if (i!=list.Length-1)
							        {
								        comp.text += "\n";
							        }
						        }
					        }
				        }

				        if (switchTextColor)
				        {
					        ChinesizeManager.Instance.SetTextColorByLanguageType(ref comp,chinesizeColorGroup);
				        }
				        break;
		        }
	        }
        }

        private void ImageChinesize()
        {
	        var comp = GetComponent<Image>();
	        if (comp)
	        {
		        switch (selectType)
		        {
			        case FuncType.Disable:
				        ctrlSelfActState();
				        break;
			        case FuncType.Switch:
				        ChinesizeManager.Instance.SetImgSrcByLanguageType(ref comp, _chinesizeSpriteGroup);
				        break;
		        }
	        }
        }
        
        private void ctrlSelfActState()
        {
	        if (disableSelfInChinese && ChinesizeManager.Instance.isChinese)
	        {
		        gameObject.SetActive(false);
	        }

	        if (disableSelfInEnglish && !ChinesizeManager.Instance.isChinese)
	        {
		        gameObject.SetActive(false);
	        }
        }


    }
}