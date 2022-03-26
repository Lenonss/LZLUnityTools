using System.ComponentModel;
using LZLUnityTool.Plugins.CommonPlugin.ExtructFunction;
using LZLUnityTool.Plugins.InvntoryPlugin.Loot;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LZLUnityTool.Plugins.InvntoryPlugin
{
[DisallowMultipleComponent]
public class ItemSlotBase : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
#if UNITY_EDITOR
    [Header("图标")]
#endif
    protected Image icon;

    [SerializeField]
#if UNITY_EDITOR
    [Header("数量")]
#endif
    protected Text amount;

    [SerializeField]
#if UNITY_EDITOR
    [Header("品质识别框")]
#endif
    protected Image qualityEdge;

    [SerializeField]
#if UNITY_EDITOR
    [Header("复选框")]
#endif
    protected GameObject mark;

    public bool IsDark { get; protected set; }

    [HideInInspector]
    public ItemInfo MItemInfo { get; protected set; }

    public bool IsEmpty { get { return MItemInfo == null || !MItemInfo.item; } }

    public void Init()
    {
        Empty();
    }
    public void Empty()
    {
        Mark(false);
        Light();
        icon.overrideSprite = null;
        amount.text = string.Empty;
        MItemInfo = null;
        qualityEdge.color = Color.white;
    }
    public void Recycle()
    {
        Empty();
       // ObjectPool<GameObject>.Put(gameObject);
    }

    public void Dark()
    {
        icon.color = Color.grey;
        IsDark = true;
    }
    public void Light()
    {
        icon.color = Color.white;
        IsDark = false;
    }

    public void Select()
    {
        if (IsDark)
        {
            icon.color = (Color.yellow + Color.grey) / 2;
        }
        else
        {
            icon.color = Color.yellow;
        }
    }
    public void DeSelect()
    {
        if (IsDark)
        {
            Dark();
        }
        else
        {
            Light();
        }
    }

    public void Show()
    {
        CommonTool.SetActive(gameObject, true);
    }
    public void Hide()
    {
        CommonTool.SetActive(gameObject, false);
    }

    public void Mark(bool mark)
    {
        CommonTool.SetActive(this.mark, mark);
    }

    public virtual void SetItem(ItemInfo info)
    {
    }

    public virtual void SetItem(ItemInfoBase info)
    {
        if (info == null) return;
        MItemInfo = new ItemInfo(info.item, info.Amount);
        // if (GameManager.QualityColors.Count >= 5)
        // {
        //     qualityEdge.color = GameManager.QualityColors[(int)info.item.Quality];
        // }
        UpdateInfo();
    }

    public virtual void UpdateInfo()
    {
        if (MItemInfo == null || !MItemInfo.item)
        {
            Empty();
            return;
        }
        if (MItemInfo.item.GoodsIcon) icon.overrideSprite = MItemInfo.item.GoodsIcon;
        amount.text = MItemInfo.Amount > 0 && MItemInfo.item.StackAble ? MItemInfo.Amount.ToString() : string.Empty;
        if (MItemInfo.Amount < 1) Empty();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        // if (eventData.button == PointerEventData.InputButton.Left)
        // {
        //     if (!IsEmpty) ItemWindowManager.Instance.ShowItemInfo(this);
        //     return;
        // }
    }
}
}